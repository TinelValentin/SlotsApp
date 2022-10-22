using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

        static List<User> _users = new List<User> {
            new User { Id=Guid.NewGuid(),
            Username="Tainal",
            Password="Zanussi",
            FirstName="Tinel",
            LastName="Carmaciu",
            Email="tinel_vali@yahoo.com",
            SSN="5010614100145",
            Phone="0763217542",
            Wallet=10000000}
        };

        [HttpGet]
        public IActionResult GetNotes()
        {
            return Ok(_users);
        }

    }
}
