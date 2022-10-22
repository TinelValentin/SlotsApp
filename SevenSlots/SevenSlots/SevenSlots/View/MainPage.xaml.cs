using SevenSlots.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace SevenSlots
{
    public partial class MainPage : Xamarin.Forms.MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            Detail = new NavigationPage(new HomePage());

            IsPresented = false;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Page1());

            IsPresented = false;
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Page2());

            IsPresented = false;
        }

        private void HomePageButton(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HomePage());

            IsPresented = false;
        }
    }
}
