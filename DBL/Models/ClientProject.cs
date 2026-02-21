using AdminDashboard.Models.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class ClientProject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EstimatedEndDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; } // NotStarted, InProgress, OnHold, Completed, Cancelled

        [Range(0, 100)]
        public int Progress { get; set; }

        public int? ProjectManagerId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ProjectManagerId")]
        public virtual Employee ProjectManager { get; set; }

        public virtual ICollection<ProjectMilestone> Milestones { get; set; }
        public virtual ICollection<ProjectUpdate> Updates { get; set; }
        public virtual ICollection<ClientDocument> Documents { get; set; }
    }
}
