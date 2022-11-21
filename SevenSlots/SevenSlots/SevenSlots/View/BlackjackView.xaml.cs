﻿using SevenSlots.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
    }
}