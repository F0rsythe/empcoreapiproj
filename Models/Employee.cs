using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace empcoreapiproj.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Department { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Role { get; set; }
    }
}