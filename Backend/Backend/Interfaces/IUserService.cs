using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAll();

        Task<User> Get(Guid id);

        Task<bool> Create(User user);

        Task<bool> Update(Guid id, User user);
    }
}
