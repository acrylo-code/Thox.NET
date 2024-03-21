using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Thox.Data;
using Thox.Models;
using Thox.Hubs;

namespace Thox.Controllers
{
    [Route("api/[controller]")]
    //require an API key to access this controller
    [ApiController]
    public class RoomPricesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<SignalHub> _signalHubContext;

        public RoomPricesController(IHubContext<SignalHub> signalHubContext, ApplicationDbContext context)
        {
            _context = context;
            _signalHubContext = signalHubContext;
        }

        // GET: api/RoomPrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomPrice>>> GetPrices()
        {
            return await _context.Prices.ToListAsync();
        }

        // GET: api/RoomPrices/5
        [ApiKeyAuth]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomPrice>> GetRoomPrice(int id)
        {
            var roomPrice = await _context.Prices.FindAsync(id);

            if (roomPrice == null)
            {
                return NotFound();
            }

            return roomPrice;
        }

        // PUT: api/RoomPrices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ApiKeyAuth]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomPrice(int id, RoomPrice roomPrice)
        {
            if (id != roomPrice.RoomPriceID)
            {
                return BadRequest();
            }

            _context.Entry(roomPrice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomPriceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RoomPrices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ApiKeyAuth]
        [HttpPost]
        public async Task<ActionResult<RoomPrice>> PostRoomPrice(RoomPrice roomPrice)
        {
            _context.Prices.Add(roomPrice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoomPrice", new { id = roomPrice.RoomPriceID }, roomPrice);
        }

        // DELETE: api/RoomPrices/5
        [ApiKeyAuth]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomPrice(int id)
        {
            var roomPrice = await _context.Prices.FindAsync(id);
            if (roomPrice == null)
            {
                return NotFound();
            }

            _context.Prices.Remove(roomPrice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomPriceExists(int id)
        {
            return _context.Prices.Any(e => e.RoomPriceID == id);
        }
    }
}
