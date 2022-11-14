using SevenSlots.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.Commands
{
    internal class RegisterCommand : ICommand
    {
        IDatabaseService IDatabaseService;

        public RegisterCommand()
        {
            this.IDatabaseService = DependencyService.Get<IDatabaseService>();
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            IDatabaseService.getUserWithIdAsync(new Guid("a6715bbd-a234-4809-b9cb-7a1bbdc4b432"));
        }
    }
}
