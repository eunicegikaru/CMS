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
    public class ClientNotification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        [StringLength(50)]
        public string Type { get; set; } // Project, Invoice, Message, Document, System

        public int? ReferenceId { get; set; } // ID of related entity

        public bool IsRead { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ReadAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
