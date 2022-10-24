using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.Commands
{
    class AccordionAnimationCommand : ICommand
    {
        int number = 0;
        //How many seconds the button will stay in the center
        const int waitTime = 2;
        const int startPoint = 0;
        const int middlePoint = 130;
        const int finalPoint = 350;
        //Animation time (speed)
        const int animationTime = 1000;
        //Needs to be changed when the real implementation with the games is decided
        string[] arr = new string[5] { "BlackJack", "Roulette", "Slots", "Bingo", "Loto" };

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            Device.StartTimer(TimeSpan.FromSeconds(5), () => AnimationTimer(parameter as Button));
            return true;
        }

        /// <summary>
        /// Calls a coroutine every 5 seconds which will move the button
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        { 
        }

        async Task MoveBackAsync(Button button)
        {
            button.Opacity = 0;
            await button.TranslateTo(startPoint, 0, animationTime);
        }

        async Task MoveToCenterAsync(Button button)
        {
            button.Opacity = 1;
            button.Text = arr[number];
            await button.TranslateTo(middlePoint, 0, animationTime);
        }

        async Task MoveAwayAsync(Button button)
        {
            await button.TranslateTo(finalPoint, 0, animationTime);
        }

        bool AnimationTimer(Button button)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await MoveToCenterAsync(button);
                await Task.Delay(TimeSpan.FromSeconds(waitTime));
                await MoveAwayAsync(button);
                await MoveBackAsync(button);
                number++;
                if (number == arr.Length-1)
                {
                    number = 0;
                }
            });
            return true;
        }
    }
}
