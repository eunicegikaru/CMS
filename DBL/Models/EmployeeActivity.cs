using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class EmployeeActivity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Action { get; set; }
        public string? Details { get; set; }
        public string? IPAddress { get; set; }
    }
}
