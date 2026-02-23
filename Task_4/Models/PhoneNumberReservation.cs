using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_4.Models
{
    public class PhoneNumberReservation
    {
        public int ID { get; set; }

        public int ClientID { get; set; }
        public string ClientName { get; set; } // For display

        public int PhoneNumberID { get; set; }
        public string PhoneNumber { get; set; } // For display

        public DateTime BED { get; set; } // Begin Effective Date
        public DateTime? EED { get; set; } // End Effective Date (Nullable)
    }
}