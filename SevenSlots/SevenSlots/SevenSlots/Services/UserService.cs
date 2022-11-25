﻿using Rg.Plugins.Popup.Services;
using SevenSlots.Model;
using SevenSlots.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SevenSlots.Services
{
    internal class UserService : IUserService
    {
        IUserRepository userRepository;
        public UserService()
        {
            userRepository = DependencyService.Get<IUserRepository>();
        }

        public async Task<User> login(string username, string password)
        {
            List<User> users = await userRepository.getAllUsers();

            var result = users.FirstOrDefault(u => u.Username == username &&
                             u.Password == password);

            return result;
        }

        public async Task register(User user)
        {
            await userRepository.addUser(user);
        }
    }
}
