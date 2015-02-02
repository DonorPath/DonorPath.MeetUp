﻿using DonorPath.MeetUp.Models;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace DonorPath.MeetUp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            ViewBag.Name = Settings.Name;
            if (String.IsNullOrEmpty(ViewBag.Name))
                ViewBag.Name = "I";
            return View(new ScheduleModel());
        }

        public ActionResult Appointments()
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            string calendarUri1 = Settings.CalendarUri;
            Uri calendarPublicUri = 
                new Uri(calendarUri1);
            Appointment[] scheduledAppointments = Calendar.GetScheduledAppointments(calendarPublicUri, DateTime.Today, DateTime.Today.AddDays(30));

            for (DateTime d = DateTime.Today; d < DateTime.Today.AddDays(30); d = d.AddDays(1))
            {
                if(d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                {
                    for(DateTime start = d.AddHours(9); start < d.AddHours(17); start = start.AddMinutes(15))
                    {
                        DateTime end = start.Add(DemoDuration);
                        if(start > DateTime.Now && !scheduledAppointments.Any(x =>
                (start < x.StartTime.ToLocalTime() && end > x.StartTime.ToLocalTime()) ||    //overlaps front of existing appointment
                (start < x.EndTime.ToLocalTime() && end > x.EndTime.ToLocalTime()) ||    //overlaps back of existing appointment
                (start >= x.StartTime.ToLocalTime() && end <= x.EndTime.ToLocalTime()) ||   //contained by existing appointment
                (start < x.StartTime.ToLocalTime() && end > x.EndTime.ToLocalTime())))     //contains an existing appointment
                        {
                            availableAppointments.Add(new Appointment { StartTime = start, EndTime = end });
                        }
                    }
                }
            }
            return Json(availableAppointments.Select(x => x.StartTime.ToString("M/dd/yyyy h:mm t")).ToArray(), JsonRequestBehavior.AllowGet);
        }

        private static TimeSpan DemoDuration
        {
            get
            {
                TimeSpan demoDuration = new TimeSpan(0, 30, 0);
                return demoDuration;
            }
        }

        public ActionResult ScheduleAppointment(ScheduleModel model)
        {

            string email = Settings.Email;

            string domainUser = email.Split('@').First();

            string templateFilePath = Server.MapPath(string.Format("~\\Content\\EmailTemplates\\{0}.html", domainUser));

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(templateFilePath))
            {
                body = reader.ReadToEnd();
            }

            string mergedBody = string.Format(body, Settings.WebsiteBaseUrl, DateTime.Now.Year.ToString());



            MailMessage mailMessage = new MailMessage(Settings.MailUsername, model.Email + "," + email)
            {
                Subject = string.Empty,
                Body = mergedBody,
                IsBodyHtml = true
            };
            string ics = Calendar.GetIcalString(string.Format("Meeting with {1} and {0}", model.Email, Settings.Name), Settings.Location, model.AppointmentTime.Value, model.AppointmentTime.Value.Add(DemoDuration));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ics)))
            {
                using (Attachment attachment = new Attachment(memoryStream, "meeting.ics"))
                {
                    mailMessage.Attachments.Add(attachment);
                    mailMessage.ReplyToList.Add(new MailAddress(Settings.Email, Settings.Name));

                    SmtpClient smtpClient = new SmtpClient(Settings.MailHost, Settings.MailPort);

                    smtpClient.Credentials = new NetworkCredential(Settings.MailUsername, Settings.MailPassword);

                    smtpClient.EnableSsl = Settings.MailEnableSsl;

                    smtpClient.Send(mailMessage);

                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}