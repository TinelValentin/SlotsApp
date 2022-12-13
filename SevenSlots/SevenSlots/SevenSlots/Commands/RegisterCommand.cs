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
using System.Security.Cryptography;

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
            User newUser = parameter as User;
            if(string.IsNullOrWhiteSpace(newUser.Password))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Sign up failed!",
                    "You can't leave empty fields!",
                    "Ok"
                );
                return;
            }
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(newUser.Password));
                newUser.Password = Convert.ToBase64String(data);
            }

            string message = await userService.register(newUser);

            if (message == "Ok") {
                User user = await userService.login(newUser.Username, newUser.Password);

                string userString = JsonSerializer.Serialize(user);
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
