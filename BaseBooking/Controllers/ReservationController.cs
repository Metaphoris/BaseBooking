using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaseBooking.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;

namespace BaseBooking.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IStringLocalizer<ReservationController> _localizer;

        private readonly ReservationContext _context;

        public ReservationController(ReservationContext context, IStringLocalizer<ReservationController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // GET: Reservation
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservations.ToListAsync());
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .SingleOrDefaultAsync(m => m.ID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StartDateTime,EndDateTime")] Reservation reservation)
        {
            CheckDateTime(reservation);
            CheckIntersections(reservation);

            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.SingleOrDefaultAsync(m => m.ID == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StartDateTime,EndDateTime")] Reservation reservation)
        {
            if (id != reservation.ID)
            {
                return NotFound();
            }

            CheckEditDelete(reservation);
            CheckDateTime(reservation);
            CheckIntersections(reservation);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .SingleOrDefaultAsync(m => m.ID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.SingleOrDefaultAsync(m => m.ID == id);
            CheckEditDelete(reservation);

            if (!ModelState.IsValid)
            {
                return View(reservation);
            }

            if (ModelState.IsValid)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ID == id);
        }

        #region Helpers

        public async void CheckIntersections(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var reservations = await _context.Reservations.Where(r => r.ID != reservation.ID).ToListAsync();
                var conflicts = reservations.Where(r => (r.StartDateTime <= reservation.EndDateTime) && (r.EndDateTime >= reservation.StartDateTime));
                if (conflicts.Count() > 0)
                    ModelState.AddModelError(string.Empty, _localizer["You cannot create a reservation that intersects with another booking. Check the armor list."]);
            }
        }

        public void CheckDateTime(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                TimeSpan tsMin = TimeSpan.FromHours(2);
                TimeSpan tsMax = TimeSpan.FromHours(4);

                if (reservation.StartDateTime > reservation.EndDateTime)
                    ModelState.AddModelError(string.Empty, _localizer["The start date cannot be later than the end date."]);

                if (reservation.StartDateTime == reservation.EndDateTime)
                    ModelState.AddModelError(string.Empty, _localizer["The start and end date-time of the reservation cannot be identical. The minimum duration of the booking is 2 hours, the maximum is 4 hours."]);

                TimeSpan ts = reservation.EndDateTime.Subtract(reservation.StartDateTime);
                if ((ts < tsMin) || (ts > tsMax))
                {
                    ModelState.AddModelError(string.Empty, _localizer["The minimum duration of the repetition is 2 hours, the maximum is 4 hours."]);
                }
            }
        }

        public async void CheckEditDelete(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var reservationsDb = await _context.Reservations.Where(r => r.ID == reservation.ID).ToListAsync();
                Reservation reservationDb = reservationsDb.First();

                if ((reservationDb.EndDateTime <= DateTime.Now) || ((reservationDb.StartDateTime >= DateTime.Now) && (reservationDb.EndDateTime <= DateTime.Now)))
                {
                    ModelState.AddModelError(string.Empty, _localizer["You cannot delete or edit repetition information that is over or is currently in progress."]);
                }
            }
        }

        #endregion
    }
}
