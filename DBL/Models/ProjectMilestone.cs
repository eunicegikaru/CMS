using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class ProjectMilestone
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CompletedDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; } // Pending, InProgress, Completed

        public decimal? PaymentAmount { get; set; }

        // Navigation properties
        [ForeignKey("ProjectId")]
        public virtual ClientProject Project { get; set; }
    }

}
