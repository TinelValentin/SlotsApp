using Rg.Plugins.Popup.Services;
using SevenSlots.Model;
using SevenSlots.Services;
using SevenSlots.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.Commands
{
    internal class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        IDatabaseService databaseService;

        public LoginCommand()
        {
            databaseService = DependencyService.Get<IDatabaseService>();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            List<User> users = await databaseService.getAllUsers();
            var result = users.FirstOrDefault(u => u.Username == (parameter as User).Username &&
                             u.Password == (parameter as User).Password);

            if (result == null) {
                await PopupNavigation.Instance.PushAsync(new LoginErrorPopup());
            }
            else
            {
                Application.Current.MainPage = new AppShell();
            }
        }
    }
}
