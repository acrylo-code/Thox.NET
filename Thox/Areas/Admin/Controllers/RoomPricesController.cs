using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Thox.Data;
using Thox.Models;

namespace Thox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]
    public class RoomPricesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomPricesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/RoomPrices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Prices.Include(r => r.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/RoomPrices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomPrice = await _context.Prices
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.RoomPriceID == id);
            if (roomPrice == null)
            {
                return NotFound();
            }

            return View(roomPrice);
        }

        // GET: Admin/RoomPrices/Create
        public IActionResult Create()
        {
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomID");
            return View();
        }

        // POST: Admin/RoomPrices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomPriceID,GroupSize,Price,RoomID")] RoomPrice roomPrice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomID", roomPrice.RoomID);
            return View(roomPrice);
        }

        // GET: Admin/RoomPrices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                //goto index
                return RedirectToAction(nameof(Index));
            }

            var roomPrice = await _context.Prices.FindAsync(id);
            if (roomPrice == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomID", roomPrice.RoomID);
            return View(roomPrice);
        }

        // POST: Admin/RoomPrices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int RoomPriceID, [Bind("RoomPriceID,GroupSize,Price,RoomID")] RoomPrice roomPrice)
        {
            if (RoomPriceID != roomPrice.RoomPriceID)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomPriceExists(roomPrice.RoomPriceID))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomID", roomPrice.RoomID);
            return View(roomPrice);
        }

        // GET: Admin/RoomPrices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomPrice = await _context.Prices
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.RoomPriceID == id);
            if (roomPrice == null)
            {
                return NotFound();
            }

            return View(roomPrice);
        }

        // POST: Admin/RoomPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int RoomPriceID)
        {

            var roomPrice = await _context.Prices.FindAsync(RoomPriceID);
            if (roomPrice != null)
            {
                _context.Prices.Remove(roomPrice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomPriceExists(int id)
        {
            return _context.Prices.Any(e => e.RoomPriceID == id);
        }
    }
}
