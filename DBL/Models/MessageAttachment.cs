using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class MessageAttachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MessageId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        public long FileSize { get; set; }

        [StringLength(100)]
        public string FileType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("MessageId")]
        public virtual ClientMessage Message { get; set; }
    }
}
