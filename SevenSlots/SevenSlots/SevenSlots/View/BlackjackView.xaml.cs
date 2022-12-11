using SevenSlots.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SevenSlots.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SevenSlots.Model;

namespace SevenSlots.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlackjackView : ContentPage
    {
        public BlackjackView()
        {
            InitializeComponent();
            BindingContext = new BlackjackViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Session.GeneralSettings != "")
            {
                (BindingContext as BlackjackViewModel).User = JsonSerializer.Deserialize<User>(Session.GeneralSettings);
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            var bc = BindingContext as BlackjackViewModel;
            if (bc.User.Username != null)
            {
                //Save the wallet locally as well
                string userString = JsonSerializer.Serialize((BindingContext as BlackjackViewModel).User);
                Session.GeneralSettings = userString;

                await bc.UpdateWallet();
            }
        }
    }
}