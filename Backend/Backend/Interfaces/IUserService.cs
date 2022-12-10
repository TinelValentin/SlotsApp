using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAll();

        Task<User> Get(Guid id);

        Task<List<User>> Get(string username);

        Task<string> Create(User user);

        Task<bool> Update(Guid id, User user);

        Task<bool> Delete(Guid id);

        Task<bool> PatchWallet(Guid id, double wallet);

        Task<bool> DeleteAll();
    }
}
