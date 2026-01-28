using DBL;
using DBL.Services;
using DBL.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllersWithViews();

        // Register database-dependent services with DI
        builder.Services.AddScoped<Bl>(provider =>
        {
            var conn = provider.GetRequiredService<IConfiguration>()
                               .GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string missing");
            return new Bl(conn);
        });

        builder.Services.AddScoped<ClientsRepository>(provider =>
        {
            var conn = provider.GetRequiredService<IConfiguration>()
                               .GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string missing");
            return new ClientsRepository(conn);
        });

        // Email service
        builder.Services.AddScoped<EmailServices>();

        // Cookie authentication
        builder.Services.AddAuthentication("MyCookie")
            .AddCookie("MyCookie", options =>
            {
                options.LoginPath = "/Account/Login";
            });

        var app = builder.Build();

        // Optional: Test DB connection at startup
        //using (var scope = app.Services.CreateScope())
        //{
        //    var repo = scope.ServiceProvider.GetRequiredService<IClientsRepository>();
        //    repo.TestConnection();
        //}

        // Configure middleware pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles(); // Serve static files before routing

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // Default route
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

        app.Run();
    }
}
