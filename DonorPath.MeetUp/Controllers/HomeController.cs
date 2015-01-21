using DonorPath.MeetUp.Models;
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

            ViewBag.Name = Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("name");
            if (String.IsNullOrEmpty(ViewBag.Name))
                ViewBag.Name = "I";
            return View(new ScheduleModel());
        }

        public ActionResult Appointments()
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            string calendarUri1 = Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("calendarUri");
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
                (start < x.StartTime && end > x.EndTime) ||    //overlaps front of existing appointment
                (start < x.StartTime && end > x.EndTime) ||    //overlaps back of existing appointment
                (start >= x.StartTime && end <= x.EndTime) ||     //contained by existing appointment
                (start < x.StartTime && end > x.EndTime)))     //contains an existing appointment
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

            string email = Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("email");
            MailMessage mailMessage = new MailMessage("no-reply@donorpath.org", model.Email + "," + email)
            {
                Subject = string.Empty,
                Body = "http://www.join.me/donorpath",
                IsBodyHtml = true
            };
            string ics = Calendar.GetIcalString(string.Format("DonorPath demo for {0}", model.Email), "http://www.join.me/donorpath", model.AppointmentTime.Value, model.AppointmentTime.Value.Add(DemoDuration));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ics)))
            {
                using (Attachment attachment = new Attachment(memoryStream, "demo.ics"))
                {
                    mailMessage.Attachments.Add(attachment);
                    mailMessage.ReplyToList.Add(new MailAddress("support@donorpath.org", "DonorPath Community Support"));
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Send(mailMessage);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}