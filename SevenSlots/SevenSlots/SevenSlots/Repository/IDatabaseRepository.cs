using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SevenSlots.Services
{
    internal interface IDatabaseRepository
    {
        Task<List<User>> getAllUsers();

        Task addUser(User newUser);
    }
}
