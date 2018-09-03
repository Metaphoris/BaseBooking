using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.EntityFrameworkCore;
using BaseBooking.Models;

namespace BaseBooking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            var host = BuildWebHost(args);//.Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<UserContext>();
                    context.Database.Migrate();
                    SeedData.Initialize(services);

                    //var context2 = services.GetRequiredService<ReservationContext>();
                    //context2.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
