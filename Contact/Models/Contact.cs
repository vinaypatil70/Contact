using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Models
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string PhotoUri { get; set; }
        public DateTime DOB { get; set; }
        public double Height { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanantAddress { get; set; }
        public string MaritalStatus { get; set; }
    }
}
