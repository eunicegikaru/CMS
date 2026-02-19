using DBL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface IInvoicesRepository
    {
        Task<IEnumerable<Invoice>> GetAllInvoices();
        Task<Invoice?> GetInvoiceById(int id);
        Task<int> CreateInvoice(Invoice invoice);
        Task<bool> UpdateInvoice(Invoice invoice);
        Task<bool> DeleteInvoice(int id);
        Task<int> GetPendingInvoicesCount();
        Task<decimal> GetTotalRevenue();
    }
}
