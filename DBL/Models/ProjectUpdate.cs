using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class ProjectUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Content { get; set; }

        public int? PostedById { get; set; }

        [StringLength(50)]
        public string UpdateType { get; set; } // Progress, Milestone, General, Alert

        public bool IsPublic { get; set; } = true;

        [DataType(DataType.DateTime)]
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ProjectId")]
        public virtual ClientProject Project { get; set; }

        [ForeignKey("PostedById")]
        public virtual Employee PostedBy { get; set; }
    }
}
