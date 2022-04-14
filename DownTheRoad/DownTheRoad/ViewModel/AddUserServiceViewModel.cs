using DownTheRoad.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DownTheRoad.ViewModel
{
    public class AddUserServiceViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        string _serviceTitle = "";
        string _price = "";
        string _location = "";
        string _description = "";
        public ICommand SaveServiceCommand { get; set; }
        public ICommand BackCommand { get; set; }
        
        public string ServiceTitleB
        {
            get => _serviceTitle;
            set
            {
                if (value == _serviceTitle)
                {
                    return;
                }
                _serviceTitle = value;
                OnPropertyChanged(nameof(ServiceTitleB));
            }
        }
        public string PriceB
        {
            get => _price;
            set
            {
                if (value == _price)
                {
                    return;
                }
                _price = value;
                OnPropertyChanged(nameof(PriceB));
            }
        }
        public string LocationB
        {
            get => _location;
            set
            {
                if (value == _location)
                {
                    return;
                }
                _location = value;
                OnPropertyChanged(nameof(LocationB));
            }
        }
        public string DescriptionB
        {
            get => _description;
            set
            {
                if (value == _description)
                {
                    return;
                }
                _description = value;
                OnPropertyChanged(nameof(DescriptionB));
            }
        }
        #endregion
        #region Constructor
        public AddUserServiceViewModel()//Initializing commands
        {
           
            SaveServiceCommand = new Command(Save);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());

        }
        #endregion
        #region Methods 
        public async void Save() //To Save the Service in Firebase
        {
            try
            {
                if (ServiceTitleB.Length == 0|| DescriptionB.Length == 0|| PriceB.Length == 0|| LocationB.Length==0)
                {
                    throw new Exception("Please Recheck!\nAll fields are mandatory.");
                }
                var Exercise = new WorkService()
                {
                    Title = ServiceTitleB,
                    Description = DescriptionB,
                    Location = LocationB,
                    Price = Convert.ToDecimal(PriceB ?? "0"),
                    ServiceBy = SessionInfo.Username,
                    Completed = false
                };
                    await FirebaseServices.AddExercise(Exercise);
                await Application.Current.MainPage.DisplayAlert("Information", "Thank you for posting your required service", "OK");
                MessagingCenter.Send<string>(string.Empty, "Refresh");
                await Application.Current.MainPage.Navigation.PopAsync();


            }
            catch (System.Exception ex)
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
