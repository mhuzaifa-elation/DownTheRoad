using DownTheRoad.Model;
using DownTheRoad.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DownTheRoad.ViewModel
{
    class LogoutViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        public ICommand YesCommand { get; set; }
        public ICommand NoCommand { get; set; }
        #endregion
        #region Constructor
        public LogoutViewModel()//Initializing commands
        {
            YesCommand = new Command(YesClicked);
            NoCommand = new Command(NoClicked);
        }
        #endregion
        #region Methods 
        private async void NoClicked()// GO Back to Homepage
        {
             Application.Current.MainPage.Navigation.PopAsync(); 
             Application.Current.MainPage.Navigation.PushAsync(new HomePage(SessionInfo.Role));
            


        }
        private async void YesClicked()// Logs Out the user
        {
            await Application.Current.MainPage.Navigation.PopAsync();

        }
        #endregion
        #region  INotifyPropertyChanged Methods
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}

