using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Data;
using ParkingZoneApp.DependencyInjection;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.RequestPipeline;
using ParkingZoneApp.Services;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.RegisterServices();

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            var app = builder.Build();
            app.AddPipeline();
        }
    }
}
