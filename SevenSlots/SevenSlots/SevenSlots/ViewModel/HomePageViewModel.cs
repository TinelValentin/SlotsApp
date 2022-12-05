using SevenSlots.Commands;
using SevenSlots.Helpers;
using SevenSlots.View;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.ViewModel
{
    internal class HomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand PushPageCommand { get; }
        public ICommand RedirectToLoginCommand { get; }

        public ICommand AccordionAnimationCommand { get; }

        private string accordionText;
        public string AccordionText
        {
            get { return accordionText; }
            set
            {
                accordionText = value;
                OnPropertyChanged(nameof(AccordionText)); // test
            }
        }

        private SignUpView signUpView = new SignUpView();
        public SignUpView SignUpView { get => signUpView;}

        private bool isRegisterVisible = true;
        public bool IsRegisterVisible
        { 
            get => isRegisterVisible; 
            set { 
                isRegisterVisible = value;
                OnPropertyChanged(nameof(IsRegisterVisible));
            } 
        }

        async void RedirectToLogin()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SignInView());
        }

        public HomePageViewModel()
        {
            AccordionText = "";
            AccordionAnimationCommand = new AccordionAnimationCommand();
            PushPageCommand = new PushPageCommand();
            RedirectToLoginCommand = new Command(RedirectToLogin);
            if (Session.GeneralSettings.Length != 0)
            {
                isRegisterVisible= false;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
