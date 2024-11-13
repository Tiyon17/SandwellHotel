using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SandwellHotel.Data;
using SandwellHotel.Models;

namespace SandwellHotel.Controllers
{
    public class HotelRoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelRoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HotelRooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.HotelRooms.ToListAsync());
        }

        // GET: HotelRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelRooms = await _context.HotelRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelRooms == null)
            {
                return NotFound();
            }

            return View(hotelRooms);
        }

        // GET: HotelRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HotelRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,PricePerNight,StarRating,GuestRating,NumberOfBeds,IsAvailable")] HotelRooms hotelRooms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotelRooms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotelRooms);
        }

        // GET: HotelRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelRooms = await _context.HotelRooms.FindAsync(id);
            if (hotelRooms == null)
            {
                return NotFound();
            }
            return View(hotelRooms);
        }

        // POST: HotelRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,PricePerNight,StarRating,GuestRating,NumberOfBeds,IsAvailable")] HotelRooms hotelRooms)
        {
            if (id != hotelRooms.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotelRooms);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelRoomsExists(hotelRooms.Id))
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
            return View(hotelRooms);
        }

        // GET: HotelRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelRooms = await _context.HotelRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelRooms == null)
            {
                return NotFound();
            }

            return View(hotelRooms);
        }

        // POST: HotelRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotelRooms = await _context.HotelRooms.FindAsync(id);
            if (hotelRooms != null)
            {
                _context.HotelRooms.Remove(hotelRooms);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelRoomsExists(int id)
        {
            return _context.HotelRooms.Any(e => e.Id == id);
        }

        //Filter by Beds in hotel rooms
        public async Task<IActionResult> FilterByBedrooms(int bedrooms)
        {
            var filteredhotels = await _context.HotelRooms.Where(h => h.NumberOfBeds == bedrooms).ToListAsync();
            return View("Index", filteredhotels);
        }
        //Display by available rooms.
        public async Task<IActionResult> AvailableRooms()
        {
            var availablehotels = await _context.HotelRooms.Where(h => h.IsAvailable == true).ToListAsync();
            return View("Index", availablehotels);
        }
        //Display by Unavailable rooms.
        public async Task<IActionResult> UnavailableRooms()
        {
            var unavailablehotels = await _context.HotelRooms.Where(h => h.IsAvailable == false).ToListAsync();
            return View("Index", unavailablehotels);
        }
        //Find all the rooms in a specific location
        public async Task<IActionResult> Location(string tiyon)
        {
            var location = await _context.HotelRooms.Where(h => h.Location == tiyon).ToListAsync();
            return View("Index", location);
        }


        //Sort by Guest Rating
        public async Task<IActionResult> SortByGuestRating(bool ascendings)
        {
            if (ascendings == true)
            {
                var sortedHotels = await _context.HotelRooms.OrderBy(h => h.GuestRating).ToListAsync();
                return View("Index", sortedHotels);
            }

            else if (ascendings == false)
            {
                var sortedHotels2 = await _context.HotelRooms.OrderByDescending(h => h.GuestRating).ToListAsync();
                return View("Index", sortedHotels2);
            }

            return View("Index");
        }
        //Filter By Price and allow the user to find the max or min price without entering a value into one of the boxes.
        public async Task<IActionResult> FilterPrice(int? minPrice,int? maxPrice)
        {
            if (minPrice == null )
            {
                var minimumprice= await _context.HotelRooms.Where(h => h.PricePerNight <= maxPrice).ToListAsync();
                return View("Index", minimumprice);
            }
            else if (maxPrice == null) 
            {
                var maximumprice = await _context.HotelRooms.Where(h => h.PricePerNight >= minPrice).ToListAsync();
                return View("Index", maximumprice);
            }
            return View();
        }
        //Filter by Name in hotel rooms
        public async Task<IActionResult> FilterByName(string name)
        {
            var Name = await _context.HotelRooms.Where(h => h.Name == name).ToListAsync();
            return View("Index", Name);
        }
        //Sort by Star Rating
        public async Task<IActionResult> SortByStarRating(bool ascendings)
        {
            if (ascendings == true)
            {
                var sortedhotels = await _context.HotelRooms.OrderBy(h => h.StarRating).ToListAsync();
                return View("Index", sortedhotels);
            }

            else if (ascendings == false)
            {
                var sortedhotels2 = await _context.HotelRooms.OrderByDescending(h => h.StarRating).ToListAsync();
                return View("Index", sortedhotels2);
            }

            return View("Index");
        }
    }
}

