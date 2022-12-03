using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using SevenSlots.Helpers;

namespace SevenSlots.ViewModel
{
    internal class HeaderContentViewModel : ViewModelBase
    {
        private User user = new User();
        public User User
        {
            get { return user; }
            set { user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private bool logoutIsVisible;
        public bool LogoutIsVisible
        {
            get { return logoutIsVisible; }
            set
            {
                logoutIsVisible = value;
                OnPropertyChanged(nameof(LogoutIsVisible));
            }
        }
        public HeaderContentViewModel()
        {
            if (Session.GeneralSettings != "")
            {
                user = JsonSerializer.Deserialize<User>(Session.GeneralSettings);
                logoutIsVisible = true;
            }
            else
            {
                logoutIsVisible = false;
            }
        }
    }
}
