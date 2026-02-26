using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_4.Models
{
    namespace Task_4.Models
    {
        public class ReservePhoneNumberRequest
        {
            public int ClientID { get; set; }
            public int PhoneNumberID { get; set; }
        }

        public class UnreservePhoneNumberRequest
        {
            public int ClientID { get; set; }
            public int PhoneNumberID { get; set; }
        }
    }
}