using DownTheRoad.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DownTheRoad.ViewModel
{
    public class WorkerProfileViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        string _workerName = "";
        string _bio = "";
        string ImageBase64Text = "";
        private ImageSource _image;
        private bool _isWorker = false;
        private string Key = string.Empty;

        public ICommand SelectImageCommand { get; set; }
        public ICommand SaveProfileCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public bool IsWokerB
        {
            get => _isWorker;
            set
            {
                if (value == _isWorker)
                {
                    return;
                }
                _isWorker = value;
                OnPropertyChanged(nameof(IsWokerB));
            }
        }
        public string WorkerNameB
        {
            get => _workerName;
            set
            {
                if (value == _workerName)
                {
                    return;
                }
                _workerName = value;
                OnPropertyChanged(nameof(WorkerNameB));
            }
        }

        public string BioB
        {
            get => _bio;
            set
            {
                if (value == _bio)
                {
                    return;
                }
                _bio = value;
                OnPropertyChanged(nameof(BioB));
            }
        }
        public ImageSource ImageSourceB
        {
            get { return this._image; }
            set
            {
                if (value == _image)
                {
                    return;
                }
                this._image = value;
                this.OnPropertyChanged(nameof(ImageSourceB));
            }
        }
        #endregion
        #region Constructor
        public WorkerProfileViewModel( string workerName)//Initializing Worker Page 
        {
            var page = App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 1];
            var CurrentPageTitle = page.Title;
            if (CurrentPageTitle == "Down The Road")
            {
                if (SessionInfo.Role == "Worker")
                {
                    Initialize(workerName??SessionInfo.Username);
                }

            }
            else
                Initialize(workerName);


        }
        #endregion
        #region Methods   
        private async void Initialize(string workerName)//Initializing commands
        {
            if (SessionInfo.Role == "Worker")
                IsWokerB = true;
            else
                IsWokerB = false;
            SelectImageCommand = new Command(SelectImage);
            SaveProfileCommand = new Command(Save);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
            await CheckWorkerProfile(workerName);

        }

        private async Task CheckWorkerProfile(string workerName) //Checking for existing profile in firebase
        {
            WorkerNameB = workerName;
            var WorkerProfileExists = await FirebaseServices.GetWorkerProfile(workerName);
            if (WorkerProfileExists != null)
            {
                BioB = WorkerProfileExists.Bio;
                if ((WorkerProfileExists.ImageText ?? "").Length > 0)
                {
                    ImageSourceB = ConvertBase64TexttoImage(WorkerProfileExists.ImageText);
                    ImageBase64Text = WorkerProfileExists.ImageText;
                }
                Key = WorkerProfileExists.Key;
            }
        }
        public async void Save() //To Save the Profile in Firebase
        {
            try
            {
                var WorkerProfile = new WorkerProfile()
                {
                    Name = WorkerNameB,
                    Bio = BioB,
                    ImageText = ImageBase64Text,
                };
                if (Key.Length>0)
                 await FirebaseServices.UpdateWorkerProfile(Key,WorkerProfile);
                else
                    await FirebaseServices.SaveWorkerProfile(WorkerProfile);

                MessagingCenter.Send<string>(string.Empty, "Refresh");
                await Application.Current.MainPage.DisplayAlert("Information", "Profile Saved Successfully.", "OK");


            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        public async void SelectImage()//TO Select the image from Gallery
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "Please Select an Image."
            });
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                ImageSourceB = ImageSource.FromStream(() => stream);
            }
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                await ConvertImagetoBase64Text(stream);
            }
        }
        private ImageSource ConvertBase64TexttoImage(string Base64Text)// Converting Image to Base 64 string format
        {
            var bytes = Convert.FromBase64String(Base64Text);
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        private async Task ConvertImagetoBase64Text(Stream stream)// Converting Base 64 string to Image format
        {
            byte[] data;
            var ImgMemoryStream = new MemoryStream();
            stream.CopyTo(ImgMemoryStream);
            data = ImgMemoryStream.ToArray();
            ImageBase64Text = Convert.ToBase64String(data);
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
