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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            var result = await _userService.Create(user);

            if (result != "Succes")
            {
                return Conflict(result);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User can't be null");
            }

            if (await _userService.Get(id) == null)
            {
                return NotFound("No user with that id found");
            }

            await _userService.Update(id, user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);

            if (result)
            {
                return Ok();
            }

            return BadRequest("No user found with that id");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var result = await _userService.DeleteAll();

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPatch]
        public async Task<IActionResult> PatchWallet([FromQuery] Guid id, [FromQuery] double wallet)
        {
            if (await _userService.PatchWallet(id, wallet))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
