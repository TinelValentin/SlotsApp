using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAll();

        Task<bool> Create (User user);
    }
}
