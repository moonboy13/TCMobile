using TCMobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Resources;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MenuPage : ContentPage
	{
		MainPage RootPage { get => Application.Current.MainPage as MainPage; }
		List<HomeMenuItem> menuItems;
		public MenuPage()
		{
			InitializeComponent();

			menuItems = new List<HomeMenuItem>
			{
				new HomeMenuItem {Id = MenuItemType.RunningBuilds, Title=AppStrings.RunningBuilds },
				new HomeMenuItem {Id = MenuItemType.Projects, Title=AppStrings.Projects },
				new HomeMenuItem {Id = MenuItemType.Changes, Title=AppStrings.Changes },
				new HomeMenuItem {Id = MenuItemType.Agents, Title=AppStrings.BuildAgents },
				new HomeMenuItem {Id = MenuItemType.BuildQueue, Title=AppStrings.BuildQueue },
				new HomeMenuItem {Id = MenuItemType.About, Title=AppStrings.About }
			};

			ListViewMenu.ItemsSource = menuItems;

			ListViewMenu.SelectedItem = menuItems[0];
			ListViewMenu.ItemSelected += async (sender, e) =>
			{
				if (e.SelectedItem == null)
					return;

				var id = (int)((HomeMenuItem)e.SelectedItem).Id;
				await RootPage.NavigateFromMenu(id);
			};
		}
	}
}