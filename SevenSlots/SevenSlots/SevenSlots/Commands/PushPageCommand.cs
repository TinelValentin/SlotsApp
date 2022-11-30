using SevenSlots.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.Commands
{
    internal class PushPageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await Application.Current.MainPage.Navigation.PushAsync(parameter as SignUpView);
        }
    }
}
