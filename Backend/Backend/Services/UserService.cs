using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class UserService : IUserService<User>
    {

        List<User> _users = new List<User> {
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

        public List<User> GetAll()
        {
            return _users;
        }
    }
}
