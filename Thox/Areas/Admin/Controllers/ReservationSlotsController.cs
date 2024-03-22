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
    public class ReservationSlotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationSlotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ReservationSlots
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReservationSlots.ToListAsync());
        }

        // GET: Admin/ReservationSlots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationSlot = await _context.ReservationSlots
                .FirstOrDefaultAsync(m => m.SlotID == id);
            if (reservationSlot == null)
            {
                return NotFound();
            }

            return View(reservationSlot);
        }

        // GET: Admin/ReservationSlots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ReservationSlots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlotID,RoomID,ReservationDate,State")] ReservationSlot reservationSlot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationSlot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservationSlot);
        }

        // GET: Admin/ReservationSlots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationSlot = await _context.ReservationSlots.FindAsync(id);
            if (reservationSlot == null)
            {
                return NotFound();
            }
            return View(reservationSlot);
        }

        // POST: Admin/ReservationSlots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int SlotID, [Bind("SlotID,RoomID,ReservationDate,State")] ReservationSlot reservationSlot)
        {
            if (SlotID != reservationSlot.SlotID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationSlot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationSlotExists(reservationSlot.SlotID))
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
            return View(reservationSlot);
        }

        // GET: Admin/ReservationSlots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationSlot = await _context.ReservationSlots
                .FirstOrDefaultAsync(m => m.SlotID == id);
            if (reservationSlot == null)
            {
                return NotFound();
            }

            return View(reservationSlot);
        }

        // POST: Admin/ReservationSlots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int SlotID)
        {
            var reservationSlot = await _context.ReservationSlots.FindAsync(SlotID);
            if (reservationSlot != null)
            {
                _context.ReservationSlots.Remove(reservationSlot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationSlotExists(int SlotID)
        {
            return _context.ReservationSlots.Any(e => e.SlotID == SlotID);
        }
    }
}
