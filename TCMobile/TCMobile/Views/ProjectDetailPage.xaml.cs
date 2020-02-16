using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TCMobile.ViewModels;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProjectDetailPage : ContentPage
	{
		ProjectViewModel _ViewModel;
		public ProjectDetailPage(ProjectViewModel viewModel)
		{
			InitializeComponent();

			viewModel.DisplayErrorHandler = DisplayError;
			BindingContext = _ViewModel = viewModel;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_ViewModel.LoadItemCommand?.Execute(null);
		}

		void DisplayError(string message)
		{
			DisplayAlert(AppStrings.Error, message, AppStrings.Ok);
		}
	}
}
