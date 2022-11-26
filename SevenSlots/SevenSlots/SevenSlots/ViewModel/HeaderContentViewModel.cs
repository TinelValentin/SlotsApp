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
                OnPropertyChanged("User");
            }
        }
        public HeaderContentViewModel()
        {
            if (Session.GeneralSettings != "")
            {
                user = JsonSerializer.Deserialize<User>(Session.GeneralSettings);
            }
        }
    }
}
