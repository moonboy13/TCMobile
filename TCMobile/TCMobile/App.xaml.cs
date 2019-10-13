using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TCMobile.Services;
using TCMobile.Views;
using Xamarin.Essentials;
using TeamCityAPI;
using TCConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace TCMobile
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
			DependencyService.Register<ProjectSummaryDataStore>();

			MainPage = new MainPage();
		}

		private async Task<TCConnectionData> GetConnectionInfo()
		{
			var infoJson = await SecureStorage.GetAsync("TCConnection");
			if (infoJson != null)
			{
				return JsonConvert.DeserializeObject<TCConnectionData>(infoJson);
			}
			else
			{
				return null;
			}
		}

		private async void StoreConnectionInfo(TCConnectionData connectionData)
		{
			//-- THIS NEEDS ENCRYPTION
			await SecureStorage.SetAsync("TCConnection", JsonConvert.SerializeObject(connectionData));
		}

		protected override void OnStart()
		{
			// Handle when your app starts
			// Check if a connection can be established, prompt the user if it is not.
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
			// Probably want to close any connections here
			StoreConnectionInfo(new TCConnectionData());
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
			// Reopen dem connections
		}


	}
}
