using System;
using Microsoft.EntityFrameworkCore;

namespace BaseBooking.Models
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options)
            : base(options)
        {
        }

        public DbSet<BaseBooking.Models.Reservation> Reservation { get; set; }
    }
}
