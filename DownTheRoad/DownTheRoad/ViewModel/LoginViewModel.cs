using DownTheRoad.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DownTheRoad.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        public ICommand ClearCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        string Username = "";
        string Password = "";
        public string PasswordB
        {
            get => Password;
            set
            {
                if (value == Password)
                {
                    return;
                }
                Password = value;
                OnPropertyChanged(nameof(PasswordB));
            }
        }
        public string UsernameB
        {
            get => Username;
            set
            {
                if (value == Username)
                {
                    return;
                }
                Username = value;
                OnPropertyChanged(nameof(UsernameB));
            }
        }
        #endregion
        #region Constructor
        public LoginViewModel()//Initializing commands 
        {
            LoginCommand = new Command(LoginClicked);
            RegisterCommand = new Command(Register);
            ClearCommand = new Command(ClearClicked);
        }
        #endregion
        #region Methods 
        private async void Register() //Navigate to Registration Page
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());

        }
        private void ClearClicked() //Clears Credentials
        {
            UsernameB = string.Empty;
            PasswordB = string.Empty;
        }
        private async void LoginClicked()//Log the user using firebase 
        {
            try
            {
                if (UsernameB.Length == 0)
                {
                    throw new Exception("Username Cannot be Empty");
                }
                if (PasswordB.Length == 0)
                {
                    throw new Exception("Password Cannot be Empty");
                }
                var LoggedUser = await FirebaseServices.Login(UsernameB, PasswordB);
                ClearClicked();
                Utils.InitializeSession(LoggedUser);
                await Application.Current.MainPage.Navigation.PushAsync(new HomePage(LoggedUser.Role));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }


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
