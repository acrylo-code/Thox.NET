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

            // Assign reservationData to the appropriate property (assuming reservation is an object of a class)
            //make a int from the string
            //reservation.PersonCount = request.PersonCount;

            // Return success respons


            return Ok(new { status = "success", message = "Reservation data received successfully.", link = "PersonCount=" + request.PersonCount });
        }


        [HttpPost("api/reservation/getTimeSlots")]
        public IActionResult GetTimeSlots([FromBody] TimeSlotRequest request)
        {
            Dictionary<string, string> fieldRegexMap = new Dictionary<string, string>
        {
            { "Date", @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}Z$" },
        };

            var checkDataResult = Validation.checkData(fieldRegexMap, request);
            if (checkDataResult != null)
            {
                return checkDataResult;
            }

            // Parse the string date to DateTime
            if (!DateTime.TryParseExact(request.Date, "yyyy-MM-ddTHH:mm:ss.fffZ",
                                         System.Globalization.CultureInfo.InvariantCulture,
                                         System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format.");
            }

            // Get all the reservations for the date
            var reservations = _context.ReservationSlots
                .Where(r => r.ReservationDate.Date == parsedDate.Date)
                .Select(r => new {
                    ReservationDate = r.ReservationDate,
                    AvailabilityState = r.State // Assuming the name of the property for availability state
                })
                .ToList();
            // Return success response
            return Ok(new { status = "success", message = "Reservation data received successfully.", Data = reservations.ToJson()});
        }
    }

    public class PersonCountRequest
    {
        public int PersonCount { get; set; }
    }

    public class TimeSlotRequest
    {
        public string Date { get; set; }
    }
}
