using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System.Diagnostics;
using Thox.Data;

namespace Thox.Controllers
{

    [ApiController]
    public class ReservationApiController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ReservationApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("api/reservation/personCount")]
        public IActionResult Post([FromBody] PersonCountRequest request)
        {

            Dictionary<string, string> fieldRegexMap = new Dictionary<string, string>
            {
                { "PersonCount", @"^[2-6]$" }
            };

            var checkDataResult = Validation.checkData(fieldRegexMap, request);
            if (checkDataResult != null)
            {
                return checkDataResult;
            }
            return Ok(new { status = "success", message = "Reservation data received successfully.", link = "PersonCount=" + request.PersonCount });
        }


        [HttpPost("api/reservation/getTimeSlots")]
        public IActionResult GetTimeSlots([FromBody] TimeSlotRequest request)
        {

            // List to hold the parsed dates
            List<DateTime> parsedDates = new List<DateTime>();

            foreach (var dateString in request.Dates)
            {
                if (!DateTime.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:ss.fffZ",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal,
                                            out DateTime parsedDate))
                {
                    return BadRequest($"Invalid date format: {dateString}");
                }

                parsedDates.Add(parsedDate);
            }

            // Get all the reservations for the provided dates
            var reservations = _context.ReservationSlots
                .Where(r => parsedDates.Select(pd => pd.Date).Contains(r.ReservationDate.Date))
                .Select(r => new
                {
                    ReservationDate = r.ReservationDate,
                    AvailabilityState = r.State // Assuming the name of the property for availability state
                })
                .ToList();

            // Return success response
            return Ok(new { status = "success", message = "Reservation data received successfully.", data = reservations });
        }

    }
    public class PersonCountRequest
    {
        public int PersonCount { get; set; }
    }

    public class TimeSlotRequest
    {
        public List<string> Dates { get; set; }
    }
}
