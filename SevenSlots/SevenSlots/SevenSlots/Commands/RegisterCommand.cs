using SevenSlots.Model;
using SevenSlots.Services;
using SevenSlots.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SevenSlots.Helpers;

namespace SevenSlots.Commands
{
    internal class RegisterCommand : ICommand
    {
        IUserService userService;
        public RegisterCommand()
        {
            userService = DependencyService.Get<IUserService>();
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            string message = await userService.register(parameter as User);

            if (message == "Ok") {
                string userString = JsonSerializer.Serialize(parameter as User);
                Session.GeneralSettings = userString;
                Application.Current.MainPage = new AppShell();
            }
            else if (message.Length > 30)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Sign up failed!",
                    "You can't leave empty fields!",
                    "Ok"
                );
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Sign up failed!",
                    message,
                    "Ok"
                );
            }
        }
    }
}
