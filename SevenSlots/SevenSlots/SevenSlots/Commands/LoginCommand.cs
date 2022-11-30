using Rg.Plugins.Popup.Services;
using SevenSlots.Helpers;
using SevenSlots.Model;
using SevenSlots.Services;
using SevenSlots.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.Commands
{
    internal class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        IUserService userService;

        public LoginCommand()
        {
            userService = DependencyService.Get<IUserService>();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            User user = await userService.login((parameter as User).Username, (parameter as User).Password);
            if (user != null) {
                string userString = JsonSerializer.Serialize(user);
                Session.GeneralSettings = userString;
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new LoginErrorPopup());
            }
        }
    }
}
