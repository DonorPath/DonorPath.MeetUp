
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonorPath.MeetUp
{
    public class Calendar
    {


        /// <summary>
        /// Gets the scheduled appointments.
        /// </summary>
        /// <param name="calendarPublicUri">The calendar public URI.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public static Appointment[] GetScheduledAppointments(Uri calendarPublicUri, DateTime start, DateTime end)
        {
            IICalendarCollection c = iCalendar.LoadFromUri(calendarPublicUri);
            var occu = c.GetOccurrences(start, end);
            return occu.Select(x => new Appointment
            {
                StartTime = x.Period.StartTime.Value.ToUniversalTime(),
                EndTime = x.Period.EndTime.Value.ToUniversalTime()
            }).ToArray();
        }

        public static string GetIcalString(string summary, string location, DateTime start, DateTime end)
        {
            iCalendarSerializer serializer = new iCalendarSerializer();
            start = DateTime.SpecifyKind(start, DateTimeKind.Utc);
            end = DateTime.SpecifyKind(end, DateTimeKind.Utc);
            iCalendar iCal = new iCalendar
            {
                Method = "PUBLISH",
                Version = "2.0"
            };
            iCalTimeZone timezone = iCalTimeZone.FromSystemTimeZone(System.TimeZoneInfo.Utc);

            iCal.AddTimeZone(timezone);
            Event evnt = iCal.Create<Event>();
            evnt.Summary = summary;
            evnt.UID = Guid.NewGuid().ToString();
            evnt.Start = new iCalDateTime(start);
            evnt.Start.HasTime = true;
            evnt.Duration = end.Subtract(start);
            evnt.Location = location;
            evnt.Alarms.Add(new Alarm
            {
                Duration = TimeSpan.FromMinutes(-15),
                Trigger = new Trigger(TimeSpan.FromMinutes(-15)),
                Action = AlarmAction.Display,
                Description = "Reminder"
            });
            return serializer.SerializeToString(iCal);
        }
    }
}