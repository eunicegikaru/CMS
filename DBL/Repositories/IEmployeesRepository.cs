using DBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface IEmployeesRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee?> GetEmployeeById(int id);
        Task<int> CreateEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
        Task<bool> DeleteEmployee(int id);
        Task<IEnumerable<EmployeeActivity>> GetEmployeeActivity(int employeeId);
        Task<int> GetEmployeeCount();
        Task<int> GetActiveProjectsCount();
        Task<int> GetCompletedTasksCount();
        Task<int> GetPendingInvoicesCount();

        Task<IEnumerable<object>> GetMonthlyRevenue();
        Task<IEnumerable<object>> GetProjectStatus();
        Task<IEnumerable<object>> GetEmployeePerformance();
        Task<IEnumerable<object>> GetRevenueByPeriod(string period);
        Task<IEnumerable<object>> GetRecentActivity();

    }
}
