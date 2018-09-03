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
            using (var context = new UserContext(serviceProvider.GetRequiredService<DbContextOptions<UserContext>>()))
            {
                if (!context.Users.Any())
                {
                    context.Users.AddRange(

                        new User
                        {
                            Login = "Metaphoris",
                            Password = "Met"
                        },

                        new User
                        {
                            Login = "Andromeda",
                            Password = "And"
                        },

                        new User
                        {
                            Login = "Einherjar",
                            Password = "Player"
                        }

                    );

                    context.SaveChanges();
                }
            }

            using (var context = new ReservationContext(serviceProvider.GetRequiredService<DbContextOptions<ReservationContext>>()))
            {
                if (!context.Reservations.Any())
                {
                    context.Reservations.AddRange(

                        new Reservation
                        {
                            StartDateTime = DateTime.Today,
                            EndDateTime = DateTime.Today.AddHours(3)
                        },

                        new Reservation
                        {
                            StartDateTime = DateTime.Today.AddHours(4),
                            EndDateTime = DateTime.Today.AddHours(7)
                        },

                        new Reservation
                        {
                            StartDateTime = DateTime.Today.AddHours(8),
                            EndDateTime = DateTime.Today.AddHours(11)
                        }

                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
