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
    public class ClientMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        public int? SenderEmployeeId { get; set; }

        public int? SenderClientId { get; set; }

        public int? ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }

        public bool IsRead { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ReadAt { get; set; }

        public bool IsUrgent { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("SenderEmployeeId")]
        public virtual Employee SenderEmployee { get; set; }

        [ForeignKey("ProjectId")]
        public virtual ClientProject Project { get; set; }

        public virtual ICollection<MessageAttachment> Attachments { get; set; }
        public virtual ICollection<MessageReply> Replies { get; set; }
    }
}
