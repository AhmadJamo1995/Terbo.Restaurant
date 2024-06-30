using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Dtos.Customer
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }


    }
}
