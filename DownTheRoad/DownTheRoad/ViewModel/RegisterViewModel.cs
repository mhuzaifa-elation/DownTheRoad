using DownTheRoad.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DownTheRoad.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        public ICommand BackCommand { get; set; }
        public ICommand SignupCommand { get; set; }
        string Username = "";
        string Password = "";
        string PhoneNo = "";
        string _selectedRole = "";
        private List<string> Roles = Utils.Roles;

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
        public List<string> RolePickerB
        {
            get => Roles;
            set
            {
                if (value == Roles)
                {
                    return;
                }
                Roles = value;
                OnPropertyChanged(nameof(RolePickerB));
            }
        }

        public string SelectedRole
        {
            get
            {
                return _selectedRole;
            }
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }
        public string PhoneNoB
        {
            get => PhoneNo;
            set
            {
                if (value == PhoneNo)
                {
                    return;
                }
                PhoneNo = value;
                OnPropertyChanged(nameof(PhoneNoB));
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
        public RegisterViewModel()//Initializing commands
        {
            SignupCommand = new Command(SignupClicked);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }
        #endregion
        #region Methods 
        private async void SignupClicked()//Saves the user using firebase 
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
                if (PasswordB.Length < 4)
                {
                    throw new Exception("Password Cannot be less than 4 characters");
                }
                var SignupUser = new User()
                {
                    Username = UsernameB,
                    Password = PasswordB,
                    PhoneNo = PhoneNoB,
                    Role = SelectedRole
                };
                bool isSignedup = await FirebaseServices.Signup(SignupUser);
                if (!isSignedup)
                    throw new Exception("Signup Unsuccessfull.");
                await Application.Current.MainPage.DisplayAlert("Information", $"{SelectedRole} Added Successfully", "OK");
                ClearValues();//await Application.Current.MainPage.Navigation.PopAsync();

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void ClearValues() //Clears page Data
        {
            UsernameB = string.Empty;
            PasswordB = string.Empty;
            PhoneNoB = string.Empty;
            SelectedRole = string.Empty;
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
