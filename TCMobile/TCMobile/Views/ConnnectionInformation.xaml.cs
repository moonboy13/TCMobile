using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TCMobile.ViewModels;
using TCConnection;

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
			try
			{
				Connection.Instance.InitializeConnection(_ViewModel.ConnectionData);
				if (!Connection.Instance.TestConnection().Result)
				{
					// stay here and display message.
				}
				else
				{
					this.Navigation.PopAsync();
				}
			}
			catch (Exception ex)
			{
				_ViewModel.DisplayErrorHandler?.Invoke(ex.Message);
			}
		}
	}
}
