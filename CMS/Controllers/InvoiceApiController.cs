using System;
using System.Linq;
using System.Threading.Tasks;
using DBL.Models;
using DBL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoicesRepository _invoicesRepo;

        public InvoiceController(IInvoicesRepository invoicesRepo)
        {
            _invoicesRepo = invoicesRepo;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _invoicesRepo.GetAllInvoices();
            return Ok(invoices);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var invoices = (await _invoicesRepo.GetAllInvoices())?.ToList() ?? new System.Collections.Generic.List<Invoice>();

            var paidInvoices = invoices.Where(i => string.Equals(i.Status, "Paid", StringComparison.OrdinalIgnoreCase));
            var pendingInvoices = invoices.Where(i => string.Equals(i.Status, "Pending", StringComparison.OrdinalIgnoreCase));
            var overdueInvoices = invoices.Where(i => string.Equals(i.Status, "Overdue", StringComparison.OrdinalIgnoreCase)
                                                       || (string.Equals(i.Status, "Pending", StringComparison.OrdinalIgnoreCase) && i.DueDate < DateTime.Now));

            var stats = new
            {
                paid = new
                {
                    count = paidInvoices.Count(),
                    amount = paidInvoices.Sum(i => i.TotalAmount)
                },
                pending = new
                {
                    count = pendingInvoices.Count(),
                    amount = pendingInvoices.Sum(i => i.TotalAmount)
                },
                overdue = new
                {
                    count = overdueInvoices.Count(),
                    amount = overdueInvoices.Sum(i => i.TotalAmount)
                }
            };

            return Ok(stats);
        }
    }
}
