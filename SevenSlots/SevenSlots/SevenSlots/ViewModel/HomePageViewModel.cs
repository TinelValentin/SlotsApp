using SevenSlots.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace SevenSlots.ViewModel
{
    internal class HomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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

        public HomePageViewModel()
        {
            AccordionText = "TestValue";
            AccordionAnimationCommand = new AccordionAnimationCommand();
        }

        

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
