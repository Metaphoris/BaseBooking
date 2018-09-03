using System.Collections.Generic;

namespace BaseBooking.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Reservation> Reservations { get; set; }

        public User()
        {
            Reservations = new List<Reservation>();
        }
    }
}
