using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class InvoiceItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal Amount => Quantity * UnitPrice;

        // Navigation properties
        [ForeignKey("InvoiceId")]
        public virtual ClientInvoice Invoice { get; set; }
    }
}
