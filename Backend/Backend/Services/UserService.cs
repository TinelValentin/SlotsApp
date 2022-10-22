using Backend.Interfaces;
using Backend.Models;
using MongoDB.Driver;

namespace Backend.Services
{

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.SlotsCollectionName);
        }

        public async Task<List<User>> GetAll()
        {
            var result = await _users.FindAsync(user => true);
            return result.ToList();

        }

        public async Task<bool> Create(User user)
        {
            user.Id = Guid.NewGuid();

            await _users.InsertOneAsync(user);
            return true;
        }
    }
}
