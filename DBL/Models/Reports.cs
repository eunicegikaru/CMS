using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class Reports
    {
        public int Id { get; set; }
        public string? ReportId { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public int SubmittedBy { get; set; }
        public string? PDFPath { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string? Status { get; set; }
        public string? Comments { get; set; }
        public DateTime CreatedAt { get; set; }

        public Employee? Submitter { get; set; }
    }
}
