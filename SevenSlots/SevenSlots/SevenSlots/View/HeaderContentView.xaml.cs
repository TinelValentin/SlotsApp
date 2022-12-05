using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SevenSlots.Helpers;

namespace SevenSlots.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderContentView : ContentView
    {
        public HeaderContentView()
        {
            InitializeComponent();
        }

        private async void Logout(object sender, EventArgs e)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Log out",
                                              "Are you sure you want to log out?",
                                              "Yes",
                                              "No");
            if (answer)
            {
                Session.GeneralSettings = "";
                App.Current.MainPage = new AppShell();
            }
        }
    }
}