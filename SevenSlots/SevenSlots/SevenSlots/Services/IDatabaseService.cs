using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SevenSlots.Services
{
    internal interface IDatabaseService
    {
        Task getAllUsers();

        Task addUser(User newUser);
    }
}
