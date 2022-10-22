using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            if (users == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(users);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            await _userService.Create(user);
            return Ok();
        }
    }
}
