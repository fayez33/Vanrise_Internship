using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_4.Models
{
    public enum ClientType
    {
        Individual = 0,
        Organization = 1
    }
    public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ClientType Type { get; set; } // Uses the Enum
        public DateTime? BirthDate { get; set; } // Nullable DateTime [cite: 42]
    }
}