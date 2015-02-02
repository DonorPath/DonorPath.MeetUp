using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonorPath.MeetUp
{
    public class Settings
    {
        /// <summary>
        /// Gets the mail port.
        /// </summary>
        /// <value>
        /// The mail port.
        /// </value>
        public static int MailPort { get
        {
            int mailPort = 25;
            int.TryParse(CloudConfigurationManager.GetSetting("mailPort"), out mailPort);
            return mailPort;
        }}

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public static string Email
        {
            get
            {
                return CloudConfigurationManager.GetSetting("email");
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name
        {
            get
            {
                return CloudConfigurationManager.GetSetting("name");
            }
        }

        /// <summary>
        /// Gets the mail host.
        /// </summary>
        /// <value>
        /// The mail host.
        /// </value>
        public static string MailHost 
        {
            get
            {
                return CloudConfigurationManager.GetSetting("mailHost");
            } 
        }

        /// <summary>
        /// Gets the mail username.
        /// </summary>
        /// <value>
        /// The mail username.
        /// </value>
        public static string MailUsername
        {
            get
            {
                return CloudConfigurationManager.GetSetting("mailUsername");
            }
        }

        /// <summary>
        /// Gets the mail password.
        /// </summary>
        /// <value>
        /// The mail password.
        /// </value>
        public static string MailPassword
        {
            get
            {
                return CloudConfigurationManager.GetSetting("mailPassword");
            }
        }

        /// <summary>
        /// Gets a value indicating whether [mail enable SSL].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [mail enable SSL]; otherwise, <c>false</c>.
        /// </value>
        public static bool MailEnableSsl
        {
            get
            {

                bool enableSsl = false;
                bool.TryParse(CloudConfigurationManager.GetSetting("mailEnableSSL"), out enableSsl);
                return enableSsl;
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public static string Location 
        {
            get
            {
                return CloudConfigurationManager.GetSetting("location");
            }
        }

        /// <summary>
        /// Gets the website base URL.
        /// </summary>
        /// <value>
        /// The website base URL.
        /// </value>
        public static string WebsiteBaseUrl 
        { 
            get
            {
                return CloudConfigurationManager.GetSetting("websiteBaseUrl");
            }
        }

        /// <summary>
        /// Gets the calendar URI.
        /// </summary>
        /// <value>
        /// The calendar URI.
        /// </value>
        public static string CalendarUri
        {
            get
            {
                return CloudConfigurationManager.GetSetting("calendarUri");
            }
        }

        public static double DayStart 
        {
            get
            {
                double start = 14d;
                double.TryParse(CloudConfigurationManager.GetSetting("dayStart"), out start);
                return start;
            }
        }

        public static double DayEnd {
            get
            {
                double end = 23d;
                double.TryParse(CloudConfigurationManager.GetSetting("dayEnd"), out end);
                return end;
            }
        }
    }
}