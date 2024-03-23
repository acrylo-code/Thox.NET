using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Thox.Data;
using Thox.Models;

namespace Thox.Hubs
{
    public class SignalHub : Hub
    {
        private readonly ApplicationDbContext _context;
        public SignalHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task GetPrice(int personCount)
        {

            decimal price = _context.Prices.Where(p => p.GroupSize == personCount).Select(p => p.Price).FirstOrDefault();


            await Clients.Caller.SendAsync("ReceivePrice", price, personCount);
        }

        public async Task ReserveTimeSlot(string reservationDate)
        {
            //update the database
            DateTime dateTime = DateTime.Parse(reservationDate);
            var reservationSlot = _context.ReservationSlots.Where(r => r.ReservationDate == dateTime).FirstOrDefault();
            reservationSlot.State = SlotState.Reserved;
            _context.SaveChanges();
        }
    }
}
