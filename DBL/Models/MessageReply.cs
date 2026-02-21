using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class MessageReply
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MessageId { get; set; }

        public int? SenderEmployeeId { get; set; }

        public int? SenderClientId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("MessageId")]
        public virtual ClientMessage Message { get; set; }

        [ForeignKey("SenderEmployeeId")]
        public virtual Employee SenderEmployee { get; set; }
    }
}
