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
    public class ClientDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        public int? ProjectId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        public long FileSize { get; set; }

        [StringLength(100)]
        public string FileType { get; set; }

        [StringLength(50)]
        public string Category { get; set; } // Contract, Report, Design, Specification, Other

        public int UploadedById { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ProjectId")]
        public virtual ClientProject Project { get; set; }

        [ForeignKey("UploadedById")]
        public virtual Employee UploadedBy { get; set; }
    }
}
