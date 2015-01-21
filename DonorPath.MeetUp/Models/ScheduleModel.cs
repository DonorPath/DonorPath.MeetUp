using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DonorPath.MeetUp.Models
{
    public class ScheduleModel
    {
        [Display(Name = "Meeting Time")]
        [Required(ErrorMessage = "Required")]
        public DateTime? AppointmentTime { get; set; }

        [EmailAddress(ErrorMessage="Bad email address")]
        [Display(Name="Email Address")]
        [Required(ErrorMessage="Required")]
        public string Email { get; set; }
    }
}