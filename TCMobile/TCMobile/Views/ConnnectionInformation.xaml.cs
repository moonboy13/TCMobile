using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TCMobile.ViewModels;
using TeamCityAPI;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConnectionInformation : ContentPage
	{
		public static readonly BindableProperty RequiredFieldProperty =
			BindableProperty.CreateAttached("RequiredField", typeof(bool), typeof(View), false);

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
		{
			IServerConnection serverConnection = null;
			// Test that the connection. If it works then save the results.
			try
			{
				switch (_ViewModel.SelectedConnection)
				{
					case ConnectionType.Guest:
						serverConnection = new ServerConnection(_ViewModel.ConnectionData.Url,
												 _ViewModel.ConnectionData.Port);
						break;
					case ConnectionType.Basic:
						serverConnection = new ServerConnection(_ViewModel.ConnectionData.Url,
												 _ViewModel.ConnectionData.Port,
												 _ViewModel.ConnectionData.Username,
												 _ViewModel.ConnectionData.Password);
						break;
					case ConnectionType.Token:
					default:
						serverConnection = new ServerConnection(_ViewModel.ConnectionData.Url,
												 _ViewModel.ConnectionData.Port,
												 _ViewModel.ConnectionData.AuthToken);
						break;
				}
			}
			finally
			{
				serverConnection?.Dispose();
			}
		}
	}
}
