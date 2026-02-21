// Models/ClientModels.cs
using DBL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDashboard.Models.Client
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactPerson { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Industry { get; set; }

        public string Logo { get; set; }

        public bool IsActive { get; set; } = true;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<ClientProject> Projects { get; set; }
        public virtual ICollection<ClientInvoice> Invoices { get; set; }
        public virtual ICollection<ClientMessage> Messages { get; set; }
        public virtual ICollection<ClientDocument> Documents { get; set; }
    }

}