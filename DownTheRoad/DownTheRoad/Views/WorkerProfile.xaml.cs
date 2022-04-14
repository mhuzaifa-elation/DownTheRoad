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
	public partial class WorkerProfile : ContentPage
	{
		public WorkerProfile ( )
		{
			InitializeComponent ();
			BindingContext = new WorkerProfileViewModel(null);
		}
		public WorkerProfile ( string workerName)
		{
			InitializeComponent ();
			BindingContext = new WorkerProfileViewModel(workerName);
		}
	}
}