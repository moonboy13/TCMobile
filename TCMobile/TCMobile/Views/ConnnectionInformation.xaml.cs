using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TCMobile.ViewModels;
using TCConnection;
using Xamarin.Essentials;
using Newtonsoft.Json;

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

		async void Save_Clicked(object sender, EventArgs e)
		{
			try
			{
				Connection.Instance.InitializeConnection(_ViewModel.ConnectionData);
				bool results = await Connection.Instance.TestConnection().ConfigureAwait(false);
				if (!results)
				{
					// stay here and display message.
					DisplayError(AppStrings.ConnectionError);
				}
				else
				{
					// Cache the connection information to the device.
					await SecureStorage.SetAsync("Foo", JsonConvert.SerializeObject(_ViewModel.ConnectionData));
					await this.Navigation.PopModalAsync();
				}
			}
			catch (Exception ex)
			{
				_ViewModel.DisplayErrorHandler?.Invoke(ex.Message);
			}
		}
	}
}
