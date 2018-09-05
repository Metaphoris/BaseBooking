using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BaseBooking.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>()))
            {
                if ((!context.Users.Any()) && (!context.Reservations.Any()))
                {
                    User Metaphoris = new User { Login = "Metaphoris", Password = "Met" };
                    User Andromeda = new User { Login = "Andromeda", Password = "And" };
                    User Einherjar = new User { Login = "Einherjar", Password = "Player" };

                    context.Users.AddRange(

                        Metaphoris,
                        Andromeda,
                        Einherjar

                    );

                    context.Reservations.AddRange(

                        new Reservation
                        {
                            StartDateTime = DateTime.Today,
                            EndDateTime = DateTime.Today.AddHours(3),
                            User = Metaphoris
                        },

                        new Reservation
                        {
                            StartDateTime = DateTime.Today.AddHours(4),
                            EndDateTime = DateTime.Today.AddHours(7),
                            User = Andromeda
                        },

                        new Reservation
                        {
                            StartDateTime = DateTime.Today.AddHours(8),
                            EndDateTime = DateTime.Today.AddHours(11),
                            User = Einherjar
                        }

                    );
                }

                context.SaveChanges();
            }
        }
    }
}
