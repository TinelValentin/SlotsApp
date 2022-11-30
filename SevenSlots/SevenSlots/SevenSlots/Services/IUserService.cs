using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SevenSlots.Services
{
    internal interface IUserService
    {
        Task<User> login(string username, string password);

        Task register(User user);

        Task patchWallet(Guid id, double wallet);
    }
}
