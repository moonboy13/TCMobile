using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TCMobile.ViewModels;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConnectionInformation : ContentPage
	{
		ConnectionInformationViewModel _ViewModel;

		public ConnectionInformation(ConnectionInformationViewModel ViewModel)
		{
			InitializeComponent();

			ViewModel.DisplayErrorHandler = DisplayError;
			BindingContext = _ViewModel = ViewModel;
		}

		void DisplayError(string message)
		{
			DisplayAlert(AppStrings.Error, message, AppStrings.Ok);
		}

		void Save_Clicked(object sender, EventArgs e)
		{}
	}
}
