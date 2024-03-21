using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Thox.Controllers
{

    [ApiController]
    public class ReservationApiController : ControllerBase
    {

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

            // Return success response
            return Ok(new { status = "success", message = "Reservation data received successfully.", link = "Date=" + request.Date});
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
