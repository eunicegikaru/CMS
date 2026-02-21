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
    public class ClientInvoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int? ProjectId { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal TaxRate { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal TaxAmount => Subtotal * TaxRate / 100;

        [Column(TypeName = "decimal(18,2")]
        public decimal TotalAmount => Subtotal + TaxAmount;

        [Column(TypeName = "decimal(18,2")]
        public decimal AmountPaid { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal BalanceDue => TotalAmount - AmountPaid;

        [StringLength(50)]
        public string Status { get; set; } // Draft, Sent, Paid, Overdue, Cancelled

        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public string PDFPath { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ProjectId")]
        public virtual ClientProject Project { get; set; }

        public virtual ICollection<InvoiceItem> Items { get; set; }
    }
}
