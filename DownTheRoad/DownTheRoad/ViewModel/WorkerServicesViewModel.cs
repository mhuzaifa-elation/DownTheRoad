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

    public class WorkerServicesViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        private List<WorkService> _workerServices;
        private WorkService _selectedService;
        private bool _isRefreshing;
        public ICommand RefreshCommand { get; set; }
        public ICommand AssignedCommand { get; set; }
        public ICommand RequestCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand ShowCompletedCommand { get; set; }
        public ICommand ShowCommand { get; set; }
        public List<WorkService> WorkerServicesB
        {
            get
            {
                return _workerServices;
            }
            set
            {
                _workerServices = value;
                OnPropertyChanged(nameof(WorkerServicesB));
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
        public WorkerServicesViewModel()//Initializing commands and getting Services from firebase
        {
            if (SessionInfo.Role=="Worker")
            {
                GetServices();
                RefreshCommand = new Command(CmdRefresh);
                RequestCommand = new Command(RequestService);
                ApplyCommand = new Command(ApplyforService);
                AssignedCommand = new Command(AssignedService);
                ShowCompletedCommand = new Command(ShowCompleted);
                ShowCommand = new Command(ShowService);
            }
        }
        #endregion
        #region Methods
        private async Task GetServices() //Gets latest Services from firebase
        {
            List<WorkService> AllExercises = await FirebaseServices.GetAllServices();
            WorkerServicesB = AllExercises.FindAll(x => (x.ServiceBy ?? "").Length > 0 && (x.RequestedBy ?? "").Length == 0 && (x.AssignedTo ?? "").Length == 0);
        }
        private async void ShowCompleted() //Shows Completed Services
        {

            await Application.Current.MainPage.Navigation.PushAsync(new CompletedWorkerServicesPage());

        }
        private async void AssignedService() //Shows Assigned Services
        {

            await Application.Current.MainPage.Navigation.PushAsync(new AssignedWorkerServicesPage());

        }
        private async void RequestService() //Shows Requested Services
        {

            await Application.Current.MainPage.Navigation.PushAsync(new RequestedWorkerServicesPage());
            MessagingCenter.Subscribe<string>(this, "Refresh", (v) => { CmdRefresh(); });

        }
        private async void ShowService() //Shows the Selected Service Details
        {
            try
            {
                if (SelectedService != null)
                {
                    string message = string.Format("Service Title : {0}\nPrice : {1}\nLocatino : {2}\nDescription : {3}\nService By : {4}", SelectedService.Title, SelectedService.Price.ToString(),SelectedService.Location, SelectedService.Description, SelectedService.ServiceBy);
                    await Application.Current.MainPage.DisplayAlert("Information", message, "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }
        private async void ApplyforService() //Apply for Selected Service 
        {
            try
            {
                if (SelectedService != null)
                {
                    SelectedService.RequestedBy = SessionInfo.Username;
                    await FirebaseServices.UpdateExercise(SelectedService.Key, SelectedService);
                    await Application.Current.MainPage.DisplayAlert("Information", $"Applied Succesfully for {SelectedService.ServiceBy}'s Service", "OK");
                    CmdRefresh();
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
