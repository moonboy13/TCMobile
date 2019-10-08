using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TCMobile.Models;
using TCMobile.Views;
using TCMobile.ViewModels;
using TeamCityAPI;

namespace TCMobile.Views
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class ItemsPage : ContentPage
	{
		ItemsViewModel viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new ItemsViewModel();
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var item = args.SelectedItem as Item;
			if (item == null)
				return;

			await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

			// Manually deselect item.
			ItemsListView.SelectedItem = null;
		}

		void TestConnection_Clicked(object sender, EventArgs e)
		{
			try
			{
				var connection = new ServerConnection("http://192.168.56.1", 8080);
				Task.Run(async () => await connection.TestConnection()).Wait();
			}
			catch (Exception ex)
			{
				DisplayAlert(AppStrings.Error, ex.Message, AppStrings.Ok);
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (viewModel.Items.Count == 0)
				viewModel.LoadItemsCommand.Execute(null);
		}

		private void ToolbarItem_Clicked(object sender, EventArgs e)
		{

		}
		void DisplayError(string message)
		{
			DisplayAlert(AppStrings.Error, message, AppStrings.Ok);
		}
	}
}