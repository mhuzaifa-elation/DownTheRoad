using DownTheRoad.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DownTheRoad.ViewModel
{
     public class CompletedUserServicesViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        private List<WorkService> _userServices;
        private WorkService _selectedService;
        private bool _isRefreshing;
        public ICommand BackCommand { get; set; }
        public List<WorkService> UserServicesB
        {
            get
            {
                return _userServices;
            }
            set
            {
                _userServices = value;
                OnPropertyChanged(nameof(UserServicesB));
            }
        }


        public WorkService SelectedService
        {
            get
            {
                return _selectedService;
            }
            set
            {
                _selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        #endregion
        #region Constructor
        public CompletedUserServicesViewModel()//Initializing commands and getting Services from firebase
        {
            GetServices();
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }
        #endregion
        #region Methods 
        private async Task GetServices() //Gets latest Services from firebase
        {
            List<WorkService> AllServices = await FirebaseServices.GetAllServices();
            UserServicesB = AllServices.FindAll(x => x.ServiceBy == SessionInfo.Username && (x.RequestedBy ?? "").Length == 0 && (x.AssignedTo ?? "").Length > 0 && x.Completed == true);
        }
        private async void CmdRefresh() //Refreshes Page with Latest Services
        {
            IsRefreshing = true;
            await GetServices();
            IsRefreshing = false;
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
