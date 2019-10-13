using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TCMobile.ViewModels
{
	public class AboutViewModel : BaseViewModel<string>
	{
		public AboutViewModel()
		{
			Title = "About";

			OpenWebCommand = new Command( async () => await Launcher.OpenAsync(new Uri("https://xamarin.com/platform")));
		}

		public ICommand OpenWebCommand { get; }
	}
}
