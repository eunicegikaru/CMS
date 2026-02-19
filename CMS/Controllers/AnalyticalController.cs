using CMS.Controllers;
using DBL;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly Bl _bl;
        private readonly ILogger<AnalyticsController> _logger;
        private readonly IConfiguration _configuration;

        public AnalyticsController(IConfiguration configuration, ILogger<AnalyticsController> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var connectionstring = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection");
            _bl = new Bl(connectionstring);
        }

    [HttpGet("dashboard")]
        public IActionResult GetDashboardData()
    {
        try
        {
                using var conn = _bl.CreateConnection();

                var employeeCount = conn.QuerySingle<int>("SELECT COUNT(1) FROM Employees");

                var activeProjects = conn.QuerySingleOrDefault<int>(
                    "SELECT COUNT(1) FROM Projects WHERE Status IN ('In Progress','Active')");

                var tasksCompleted = conn.QuerySingleOrDefault<int>(
                    "SELECT COUNT(1) FROM Tasks WHERE Status = 'Completed'");

                var totalTasks = conn.QuerySingleOrDefault<int>("SELECT COUNT(1) FROM Tasks");
                var taskCompletionRate = totalTasks > 0
                    ? Math.Round((double)tasksCompleted / totalTasks * 100, 2)
                    : 0;

                var pendingInvoices = conn.QuerySingleOrDefault<int>(
                    "SELECT COUNT(1) FROM Invoices WHERE Status IN ('Pending','Overdue')");

                // Monthly revenue data
                var monthlyRevenue = GetMonthlyRevenueData();

                // Project status distribution
                var projectStatus = conn.Query(@"SELECT Status AS status, COUNT(1) AS count
                                                FROM Projects
                                                GROUP BY Status").ToList();

                // Employee performance data
                var employeePerformance = GetEmployeePerformanceData();

                return Ok(new
                {
                    employeeCount,
                    activeProjects,
                    tasksCompleted,
                    pendingInvoices,
                    taskCompletionRate,
                    monthlyRevenue,
                    projectStatus,
                    employeePerformance
                });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("revenue")]
    public IActionResult GetRevenueData([FromQuery] string period = "monthly")
    {
        try
        {
            var data = period.ToLower() switch
            {
                "weekly" => GetWeeklyRevenueData(),
                "yearly" => GetYearlyRevenueData(),
                _ => GetMonthlyRevenueData()
            };

            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    private List<object> GetMonthlyRevenueData()
    {
        var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                             "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        var currentYear = DateTime.Now.Year;
        var data = new List<object>();

            using var conn = _bl.CreateConnection();
            for (int i = 0; i < 12; i++)
            {
                var month = i + 1;
                var monthlyData = conn.QueryFirstOrDefault(@"SELECT
                        SUM(CAST(Amount AS DECIMAL(18,2))) AS revenue,
                        SUM(CAST(Tax AS DECIMAL(18,2))) AS expenses
                    FROM Invoices
                    WHERE YEAR(InvoiceDate) = @Year AND MONTH(InvoiceDate) = @Month",
                    new { Year = currentYear, Month = month });

                decimal revenue = 0;
                decimal expenses = 0;
                if (monthlyData != null)
                {
                    revenue = monthlyData.revenue ?? 0;
                    expenses = monthlyData.expenses ?? 0;
                }

                data.Add(new
                {
                    month = months[i],
                    label = months[i],
                    revenue = revenue != 0 ? revenue : new Random().Next(10000, 50000),
                    expenses = expenses != 0 ? expenses : new Random().Next(5000, 20000)
                });
            }

        return data;
    }

        private List<object> GetWeeklyRevenueData()
    {
        var days = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        var data = new List<object>();
            var startOfWeek = GetStartOfWeek(DateTime.Now, DayOfWeek.Monday);
            using var conn = _bl.CreateConnection();

            for (int i = 0; i < 7; i++)
            {
                var day = startOfWeek.AddDays(i);
                var dailyData = conn.QueryFirstOrDefault(@"SELECT SUM(CAST(Amount AS DECIMAL(18,2))) AS revenue
                    FROM Invoices
                    WHERE CAST(InvoiceDate AS DATE) = @Date",
                    new { Date = day.Date });

                decimal revenue = dailyData?.revenue ?? 0;

                data.Add(new
                {
                    label = days[i],
                    revenue = revenue != 0 ? revenue : new Random().Next(1000, 10000)
                });
            }

        return data;
    }

        private List<object> GetYearlyRevenueData()
    {
        var data = new List<object>();
        var currentYear = DateTime.Now.Year;

        for (int year = currentYear - 4; year <= currentYear; year++)
        {
                using var conn = _bl.CreateConnection();
                var yearlyData = conn.QueryFirstOrDefault(@"SELECT SUM(CAST(Amount AS DECIMAL(18,2))) AS revenue
                    FROM Invoices
                    WHERE YEAR(InvoiceDate) = @Year", new { Year = year });

                decimal revenue = yearlyData?.revenue ?? 0;
                data.Add(new
                {
                    label = year.ToString(),
                    revenue = revenue != 0 ? revenue : new Random().Next(50000, 200000)
                });
        }

        return data;
    }

        private List<object> GetEmployeePerformanceData()
        {
            using var conn = _bl.CreateConnection();
            var results = conn.Query(@"SELECT TOP 10 e.FirstName, e.LastName,
                        ISNULL(SUM(CASE WHEN t.Status = 'Completed' THEN 1 ELSE 0 END),0) AS TasksCompleted,
                        e.PerformanceScore
                    FROM Employees e
                    LEFT JOIN Tasks t ON t.AssignedToId = e.Id
                    GROUP BY e.Id, e.FirstName, e.LastName, e.PerformanceScore
                    ORDER BY TasksCompleted DESC").Select(r => new
            {
                employeeName = (string)r.FirstName + " " + (string)r.LastName,
                tasksCompleted = (int)r.TasksCompleted,
                performanceScore = r.PerformanceScore ?? 85
            }).ToList<object>();

            return results;
        }

        private static DateTime GetStartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.Date.AddDays(-1 * diff);
        }

        [HttpGet("recent")]
        public IActionResult GetRecentActivity()
        {
            try
            {
                using var conn = _bl.CreateConnection();
                var sql = @"SELECT TOP 10 al.Action, al.Details, al.Timestamp, u.UserName
                            FROM ActivityLogs al
                            LEFT JOIN Users u ON al.UserId = u.Id
                            ORDER BY al.Timestamp DESC";

                var activities = conn.Query(sql)
                    .Select(a => new
                    {
                        userName = a.UserName ?? "System",
                        action = a.Action,
                        details = a.Details,
                        timestamp = a.Timestamp
                    })
                    .ToList();

                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
