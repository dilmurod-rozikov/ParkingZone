using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Data;
using ParkingZoneApp.Repository;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.Services;

namespace ParkingZoneApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IParkingZoneRepository, ParkingZoneRepository>();
            builder.Services.AddScoped<IParkingZoneServices, ParkingZoneServices>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                app.UseMigrationsEndPoint();
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area:exists}/{controller=ParkingZone}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.Run();
        }
    }
}
