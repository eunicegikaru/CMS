using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? ClientName { get; set; }
        public string? ClientEmail { get; set; }
        public decimal Amount { get; set; }
        public decimal? Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public int CreatedBy { get; set; }
        public string? PDFPath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
