using SevenSlots.Services;
using SevenSlots.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SevenSlots
{
    public partial class App : Application
    {
        public App()
        {
            DependencyService.Register<IUserRepository, UserRepository>();
            DependencyService.Register<IUserService, UserService>();
            InitializeComponent();
           


            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
