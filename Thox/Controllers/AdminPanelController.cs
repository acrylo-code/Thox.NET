using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Thox.Models;

namespace Thox.Controllers
{
    //allow admin and moderator roles //block other roles
    [Authorize(Roles = "Admin, Moderator")]
    public class AdminPanelController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AdminPanelController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            // Retrieve users from the database
            var users = _userManager.Users;

            // Map users to UserModel objects
            var userModelList = new UserModelList();
            userModelList.UserModels = new List<UserModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userModel = new UserModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault()
                };
                userModelList.UserModels.Add(userModel);
            }

            return View(userModelList);
        }
    }
}
