using System;
using System.Collections.Generic;

namespace CMS.ViewModels
{
    public class DashboardViewModel
    {
        public int EmployeeCount { get; set; }
        public int ActiveProjects { get; set; }
        public int TasksCompleted { get; set; }
        public int PendingInvoices { get; set; }
        public double TaskCompletionRate { get; set; }
    }
}
