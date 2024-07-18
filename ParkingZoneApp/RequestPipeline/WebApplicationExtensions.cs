namespace ParkingZoneApp.RequestPipeline
{
    public static class WebApplicationExtensions
    {
        public static WebApplication AddPipeline(this WebApplication app)
        {
            //app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area:exists}/{controller=ParkingZone}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area:exists}/{controller=ParkingSlot}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "User",
                pattern: "{area:exists}/{controller=Reservation}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "User",
                pattern: "{area:exists}/{controller=Payment}/{action=MakePayment}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.Run();

            return app;
        }
    }
}
