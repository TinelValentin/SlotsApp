using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        IUserService<User> _userService;

        public UserController(IUserService<User> userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


        [HttpGet]
        public IActionResult GetNotes()
        {
            return Ok(_userService.GetAll());
        }

    }
}
