using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TCMobile.Models;
using System.Diagnostics;
using TCConnection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TeamCityAPI;
using Xamarin.Essentials;

namespace TCMobile.Views
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : MasterDetailPage
	{
		Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

		public MainPage()
		{
			InitializeComponent();

			MasterBehavior = MasterBehavior.Popover;

			MenuPages.Add((int)MenuItemType.RunningBuilds, (NavigationPage)Detail);

			if (!InitializeConnection().Result)
			{
				// Show connection configuration page
				Navigation.PushModalAsync(new NavigationPage(new ConnectionInformation(new ViewModels.ConnectionInformationViewModel(null))));
			}
		}

		public async Task NavigateFromMenu(int id)
		{
			if (!MenuPages.ContainsKey(id))
			{
				switch (id)
				{
					case (int)MenuItemType.RunningBuilds:
						MenuPages.Add(id, new NavigationPage(new ItemsPage()));
						break;
					case (int)MenuItemType.About:
						MenuPages.Add(id, new NavigationPage(new AboutPage()));
						break;
					case (int)MenuItemType.Projects:
						MenuPages.Add(id, new NavigationPage(new AllProjectsPage(new ViewModels.ProjectSummaryViewModel())));
						break;
					default:
						// avoid crashing the app if everything isn't hooked up.
						Debug.WriteLine(String.Format("Unknown menu type: {0}", id));
						return;
				}
			}

			var newPage = MenuPages[id];

			if (newPage != null && Detail != newPage)
			{
				Detail = newPage;

				if (Device.RuntimePlatform == Device.Android)
					await Task.Delay(100);

				IsPresented = false;
			}
		}

		private async Task<bool> InitializeConnection()
		{
			bool connectionEstablished = false;
			// TODO: Secure Storage isn't working, need to figure that out. Probs cause the app is removed and readded each debug session.
			var infoJson = await SecureStorage.GetAsync("Foo");
			var conInfo = (infoJson != null) ? JsonConvert.DeserializeObject<TCConnectionData>(infoJson) : null;

#if DEBUG
			conInfo = new TCConnectionData()
			{
				Url = "192.168.56.1",
				Port = 8080,
				ConnectionType = ConnectionType.Guest
			};
#endif

			// Attempt to connect
			if (conInfo != null)
			{
				try
				{
					Connection.Instance.InitializeConnection(conInfo);
					connectionEstablished = await Connection.Instance.TestConnection().ConfigureAwait(false);
				}
				catch
				{
					// TODO: may want to be a bit more helpful here, maybe a toast message.
					connectionEstablished = false;
				}
			}

			return connectionEstablished;
		}
	}
}
