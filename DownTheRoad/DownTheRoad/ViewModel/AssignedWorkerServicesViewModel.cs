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
    public class AssignedWorkerServicesViewModel : INotifyPropertyChanged
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
        //Initializing commands and getting exercise from firebase
        public AssignedWorkerServicesViewModel()
        {
            GetServices();
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }
        #endregion
        #region Methods 

        private async Task GetServices() //Gets latest exercises from firebase
        {
            List<WorkService> AllExercises = await FirebaseServices.GetAllServices();
            ServicesB = AllExercises.FindAll(x => (x.ServiceBy ?? "").Length > 0 && (x.RequestedBy ?? "").Length ==0&& (x.AssignedTo ?? "")==SessionInfo.Username);
        }
        private async void CmdRefresh() //Refreshes Page with Latest exercises
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
