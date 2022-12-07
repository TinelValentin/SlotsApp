using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SevenSlots.Services
{
    internal interface IUserRepository
    {
        Task<List<User>> getAllUsers();

        Task<string> addUser(User newUser);

        Task updateWallet(Guid id, double wallet);
    }
}
