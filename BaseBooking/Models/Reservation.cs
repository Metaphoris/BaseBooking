﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BaseBooking.Models
{
    public class Reservation
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        [Display(Name = "User")]
        public User User { get; set; }
        [Display(Name = "StartDateTime")]
        public DateTime StartDateTime { get; set; }
        [Display(Name = "EndDateTime")]
        public DateTime EndDateTime { get; set; }
    }
}
