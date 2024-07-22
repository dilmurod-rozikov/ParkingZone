using ParkingZoneApp.DependencyInjection;
using ParkingZoneApp.RequestPipeline;

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
