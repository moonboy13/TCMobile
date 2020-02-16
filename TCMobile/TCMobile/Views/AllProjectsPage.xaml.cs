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

			ViewModel.DisplayErrorHandler = DisplayError;
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
			_ViewModel.DisplayErrorHandler = DisplayError;
			BindingContext = _ViewModel;

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_ViewModel.LoadItemsCommand?.Execute(null);
		}

		async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e.Item == null)
				return;

			// Push the Project page onto the navigation stack
			await Navigation.PushAsync(new ProjectDetailPage(new ProjectViewModel((e.Item as ProjectSummary).Id)));
		}

		void DisplayError(string message)
		{
			DisplayAlert(AppStrings.Error, message, AppStrings.Ok);
		}
	}
}
