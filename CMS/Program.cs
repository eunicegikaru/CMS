// Program.cs
using Microsoft.AspNetCore.Authentication.Cookies;
using DBL.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Configure Database Context


        // Register repositories
        builder.Services.AddScoped<IClientsRepository>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionstring = configuration.GetConnectionString("DefaultConnection");

            return new ClientsRepository(connectionstring!);
        });
        builder.Services.AddScoped<IEmployeesRepository>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionstring = configuration.GetConnectionString("DefaultConnection");

            return new EmployeesRepository(connectionstring!);
        });

        builder.Services.AddScoped<IInvoicesRepository>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            return new InvoicesRepository(connectionString!);
        });


        // Configure Cookie Authentication
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

        // Add authorization policies
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy("ManagerOrAdmin", policy =>
                policy.RequireRole("Admin", "Manager"));

            options.AddPolicy("ClientOrHigher", policy =>
                policy.RequireRole("Admin", "Manager", "Client"));
        });

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Auth}/{action=Login}/{id?}");

        app.Run();
    }
}