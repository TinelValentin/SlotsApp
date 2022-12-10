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
            var client = new MongoClient("mongodb+srv://sorin:lemmegetmyslots2@dam.w1uadly.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("slots");

            _users = database.GetCollection<User>("users");
        }

        public async Task<List<User>> GetAll()
        {
            var result = await _users.FindAsync(user => true);
            return result.ToList();

        }

        public async Task<User> Get(Guid id)
        {
            return (await _users.FindAsync(n => n.Id == id)).FirstOrDefault();
        }

        public async Task<List<User>> Get(string username)
        {
            var result = await _users.FindAsync(n => n.Username == username);

            return result.ToList();
        }

        public async Task<string> Create(User user)
        {
            var sameUsername = await _users.FindAsync(u => u.Username == user.Username);
            if (sameUsername.ToList().Count != 0)
            {
                return "Username already exists";
            }

            var sameSSN = await _users.FindAsync(u => u.SSN == user.SSN);

            if (sameSSN.ToList().Count != 0)
            {
                return "SSN already exists";
            }

            var sameEmail = await _users.FindAsync(u => u.Email == user.Email);
            if (sameEmail.ToList().Count != 0)
            {
                return "This email is already used";
            }

            user.Id = Guid.NewGuid();

            await _users.InsertOneAsync(user);

            return "Succes";
        }

        public async Task<bool> Update(Guid id, User user)
        {
            user.Id = id;
            var result = await _users.ReplaceOneAsync(u => u.Id == id, user);

            if (!result.IsAcknowledged)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _users.DeleteOneAsync(note => note.Id == id);

            if (result.IsAcknowledged && result.DeletedCount != 0)
            {
                return true;
            }

            return false;

        }

        public async Task<bool> PatchWallet(Guid id, double wallet)
        {
            var result = await _users.UpdateOneAsync(u => u.Id == id, 
                          Builders<User>.Update.Set(u => u.Wallet, wallet));

            if (!result.IsAcknowledged)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteAll()
        {
            var result = await _users.DeleteManyAsync(u => true);

            if (!result.IsAcknowledged)
            {
                return false;
            }

            return true;
        }
    }
}
