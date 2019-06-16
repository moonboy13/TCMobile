using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TCMobile.Models;
using TCMobile.ViewModels;
using TeamCityAPI;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[DesignTimeVisible(false)]
	public partial class AllProjectsPage : ContentPage
	{
		ProjectSummaryViewModel _ViewModel;
		

		public AllProjectsPage(ProjectSummaryViewModel ViewModel)
		{
			InitializeComponent();

			BindingContext = _ViewModel = ViewModel;
		}

		public AllProjectsPage()
		{
			InitializeComponent();

			var project = new ProjectSummary
			{
				Name = "Test",
				Description = "FooBar this is a description"
			};

			_ViewModel = new ProjectSummaryViewModel();
			_ViewModel.DataStore.AddItemAsync(project).Wait();
			BindingContext = _ViewModel;

		}

		async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e.Item == null)
				return;

			await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

			//Deselect Item
			((ListView)sender).SelectedItem = null;
		}
	}
}
