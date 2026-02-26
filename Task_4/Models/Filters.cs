using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_4.Models
{
        public class DeviceFilter
        {
            public string Name { get; set; }
        }

        public class ClientFilter
        {
            public string Name { get; set; }
            public ClientType? Type { get; set; } // Nullable so you can get all types if no filter is applied
        }

        public class PhoneNumberFilter
        {
            public string Number { get; set; }
            public int? DeviceID { get; set; }
        }

        public class ReservationFilter
        {
            public int? ClientID { get; set; }
            public int? PhoneNumberID { get; set; }
        }
}