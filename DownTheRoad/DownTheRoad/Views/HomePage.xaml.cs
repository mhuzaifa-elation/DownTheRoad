﻿using DownTheRoad.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DownTheRoad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : TabbedPage
    {

        public HomePage(string Role)
        {
            InitializeComponent();
            switch (Role)
            {
                case "User":
                    HideTab(4);
                    HideTab(1);
                    break;
                case "Worker":
                    HideTab(4);
                    HideTab(0);
                    break;
                case "Admin":
                    HideTab(1);
                    HideTab(0);
                    break;

                default:
                    break;
            }
            HomepageView.BindingContext = new HomePageViewModel();
        }
        public void HideTab(int index)
        {
            if (index < this.Children.Count)
            {
                Children.RemoveAt(index);
            }
        }
    }
}