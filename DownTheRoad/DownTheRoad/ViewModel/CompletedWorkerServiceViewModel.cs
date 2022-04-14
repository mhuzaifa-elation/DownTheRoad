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
    public class CompletedWorkerServiceViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        private List<WorkService> _WorkerServices;
        private WorkService _selectedService;
        private bool _isRefreshing;
        public ICommand BackCommand { get; set; }
        public List<WorkService> ServicesB
        {
            get
            {
                return _WorkerServices;
            }
            set
            {
                _WorkerServices = value;
                OnPropertyChanged(nameof(ServicesB));
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
        public CompletedWorkerServiceViewModel()//Initializing commands and getting Services from firebase
        {
            GetServices();
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }
        #endregion
        #region Methods 

        private async Task GetServices() //Gets latest Services from firebase
        {
            List<WorkService> AllServices = await FirebaseServices.GetAllServices();
            ServicesB = AllServices.FindAll(x => (x.ServiceBy ?? "").Length > 0 && (x.RequestedBy ?? "").Length == 0 && (x.AssignedTo ?? "") == SessionInfo.Username && x.Completed == true);
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