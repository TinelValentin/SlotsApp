using SevenSlots.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SevenSlots.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpView : ContentPage
    {
        public SignUpView()
        {
            InitializeComponent();
            BindingContext = new SignUpViewModel();
        }
    }
}