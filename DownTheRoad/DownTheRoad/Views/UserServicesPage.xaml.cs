using DownTheRoad.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DownTheRoad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserServicesPage : ContentPage
    {
        public UserServicesPage()
        {
            InitializeComponent();
            BindingContext = new UserServicesViewModel();
        }
    }
}