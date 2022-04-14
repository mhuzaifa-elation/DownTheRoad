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
    public class RequestedServicesViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        private List<WorkService> _userServices;
        private WorkService _selectedService;
        private bool _isRefreshing;
        public ICommand ApproveCommand { get; set; }
        public ICommand ShowWorkerCommand { get; set; }
        public ICommand DeleteRequestCommand { get; set; }
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
        public RequestedServicesViewModel()//Initializing commands and getting Services from firebase
        {
            GetServices();
            ApproveCommand = new Command(ApproveService);
            DeleteRequestCommand = new Command(DeleteRequest);
            ShowWorkerCommand = new Command(ShowWorker);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }
        #endregion
        #region Methods 
        private async Task GetServices() //Gets latest Services from firebase
        {
            List<WorkService> AllServices = await FirebaseServices.GetAllServices();
            UserServicesB = AllServices.FindAll(x => x.ServiceBy == SessionInfo.Username&& (x.RequestedBy??"").Length>0 && (x.AssignedTo??"").Length == 0);
        }
        private async void DeleteRequest() //Delete Selected Service Request from firebase
        {
            try
            {
                if (SelectedService != null)
                {
                    SelectedService.RequestedBy = string.Empty;
                    await FirebaseServices.UpdateExercise(SelectedService.Key,SelectedService);
                    await Application.Current.MainPage.DisplayAlert("Information", "Service request successully removed.", "OK");
                    CmdRefresh();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }
        private async void ApproveService() //Approve Selected Service Request from firebase
        {
            try
            {
                if (SelectedService != null)
                {
                    SelectedService.AssignedTo = SelectedService.RequestedBy;
                    SelectedService.RequestedBy = string.Empty;
                    await FirebaseServices.UpdateExercise(SelectedService.Key,SelectedService);
                    await Application.Current.MainPage.DisplayAlert("Information", $"Service successully Assigned to {SelectedService.AssignedTo}", "OK");
                    CmdRefresh();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }
        private async void ShowWorker() //Shows the worker's Profile
        {
            try
            {
                if (SelectedService != null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new Views.WorkerProfile(SelectedService.RequestedBy));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

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
