using DownTheRoad.Model;
using DownTheRoad.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DownTheRoad.ViewModel
{
    public class UserServicesViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        private List<WorkService> _userServices;
        private WorkService _selectedService;
        private bool _isRefreshing;
        public ICommand RefreshCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand ShowCompletedCommand { get; set; }
        public ICommand AssignedCommand { get; set; }
        public ICommand RequestCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
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
        public UserServicesViewModel() //Initializing commands and getting Services from firebase
        {
            if (SessionInfo.Role=="User")
            {
                GetServices();
                RefreshCommand = new Command(CmdRefresh);
                AddCommand = new Command(AddService);
                RequestCommand = new Command(RequestService);
                DeleteCommand = new Command(DeleteService);
                AssignedCommand = new Command(AssignedService);
                ShowCompletedCommand = new Command(ShowCompleted);
            }
        }
        #endregion
        #region Methods 
        private async Task GetServices() //Gets latest Services from firebase
        {
            List<WorkService> AllServices = await FirebaseServices.GetAllServices();
            UserServicesB = AllServices.FindAll(x => x.ServiceBy == SessionInfo.Username&& (x.RequestedBy ?? "").Length == 0 && (x.AssignedTo ?? "").Length == 0);
        }
        private async void AssignedService() //Shows Assigned Services
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AssignedUserServicesPage());
        }
        private async void ShowCompleted() //Shows Completed Services
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CompletedUserServicesPage());
        }
        private async void RequestService() //Shows Requested Services
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RequestedServicesPage());
            MessagingCenter.Subscribe<string>(this, "Refresh", (v) => { CmdRefresh(); });

        }
        private async void DeleteService() //Delete Selected Service from firebase
        {
            try
            {
                if (SelectedService != null)
                {
                    await FirebaseServices.DeleteExercise(SelectedService.Key);
                    CmdRefresh();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }
        private void AddService() //GOto Add Service Page
        {
            try
            {
                Application.Current.MainPage.Navigation.PushAsync(new AddUserServicePage());
                MessagingCenter.Subscribe<string>(this, "Refresh", (v) => { CmdRefresh(); });
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
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
