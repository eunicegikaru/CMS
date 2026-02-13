using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class Tasks
    {
        public int? Id { get; set; }
        public string? TaskId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int AssignedTo { get; set; }
        public int AssignedBy { get; set; }
        public int? TeamId { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string? PDFPath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Employee? Assignee { get; set; }
        public Employee? Assigner { get; set; }
    }
}
