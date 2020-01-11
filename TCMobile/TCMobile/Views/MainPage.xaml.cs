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
	public partial class MainPage : MasterDetailPage, IDisposable
	{
		Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
		IServerConnection _serverConnection;

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
			var infoJson = await SecureStorage.GetAsync("TCConnection");
			var conInfo = (infoJson != null) ? JsonConvert.DeserializeObject<TCConnectionData>(infoJson) : null;

			// Attempt to connect
			if (conInfo != null)
			{
				switch (conInfo.ConnectionType)
				{
					case ConnectionType.Guest:
						_serverConnection = new ServerConnection(conInfo.Url, conInfo.Port);
						break;
					case ConnectionType.Basic:
						_serverConnection = new ServerConnection(conInfo.Url, conInfo.Port, conInfo.Username, conInfo.Password);
						break;
					case ConnectionType.Token:
					default:
						_serverConnection = new ServerConnection(conInfo.Url, conInfo.Port, conInfo.AuthToken);
						break;
				}

				try
				{
					connectionEstablished = await _serverConnection.TestConnection();
				}
				catch
				{
					// TODO: may want to be a bit more helpful here, maybe a toast message.
					connectionEstablished = false;
				}
			}

			return connectionEstablished;
		}

		public void Dispose()
		{
			_serverConnection.Dispose();
		}
	}
}
