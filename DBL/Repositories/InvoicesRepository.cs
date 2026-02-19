using Dapper;
using DBL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public class InvoicesRepository : BaseRepository, IInvoicesRepository
    {
        public InvoicesRepository(string connectionstring) : base(connectionstring)
        {
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoices()
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            string sql = "SELECT * FROM Invoices ORDER BY CreatedAt DESC";
            return await conn.QueryAsync<Invoice>(sql);
        }

        public async Task<Invoice?> GetInvoiceById(int id)
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            return await conn.QueryFirstOrDefaultAsync<Invoice>(
                "SELECT * FROM Invoices WHERE Id=@Id",
                new { Id = id });
        }

        public async Task<int> CreateInvoice(Invoice invoice)
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            string sql = @"
            INSERT INTO Invoices
            (InvoiceNumber, ClientName, ClientEmail, Amount, Tax, TotalAmount,
             Status, IssueDate, DueDate, PaymentDate, PaymentMethod,
             CreatedBy, PDFPath, CreatedAt)
            VALUES
            (@InvoiceNumber, @ClientName, @ClientEmail, @Amount, @Tax, @TotalAmount,
             @Status, @IssueDate, @DueDate, @PaymentDate, @PaymentMethod,
             @CreatedBy, @PDFPath, GETDATE());

            SELECT CAST(SCOPE_IDENTITY() as int);";

            return await conn.ExecuteScalarAsync<int>(sql, invoice);
        }

        public async Task<bool> UpdateInvoice(Invoice invoice)
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            string sql = @"
            UPDATE Invoices SET
                ClientName=@ClientName,
                ClientEmail=@ClientEmail,
                Amount=@Amount,
                Tax=@Tax,
                TotalAmount=@TotalAmount,
                Status=@Status,
                DueDate=@DueDate,
                PaymentDate=@PaymentDate,
                PaymentMethod=@PaymentMethod,
                PDFPath=@PDFPath
            WHERE Id=@Id";

            return await conn.ExecuteAsync(sql, invoice) > 0;
        }

        public async Task<bool> DeleteInvoice(int id)
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            return await conn.ExecuteAsync(
                "DELETE FROM Invoices WHERE Id=@Id",
                new { Id = id }) > 0;
        }

        public async Task<int> GetPendingInvoicesCount()
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            return await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Invoices WHERE Status='Pending'");
        }

        public async Task<decimal> GetTotalRevenue()
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            return await conn.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(SUM(TotalAmount),0) FROM Invoices WHERE Status='Paid'");
        }
        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionstring);
            return await connection.QueryAsync<Invoice>(
                "SELECT * FROM Invoices ORDER BY Id DESC");
        }

    }
}
