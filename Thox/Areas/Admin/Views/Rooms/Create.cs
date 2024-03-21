using Microsoft.AspNetCore.Mvc.RazorPages;
using Thox.Models;

namespace Thox.Areas.Admin.Views.Rooms
{
    public class Create : PageModel
    {
        public IEnumerable<RoomName> RoomNames { get; } = Enum.GetValues(typeof(RoomName)).Cast<RoomName>();
    }
}
