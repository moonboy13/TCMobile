using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TCMobile.Models;
using TCMobile.Views;

namespace TCMobile.ViewModels
{
	public class ProjectSummaryViewModel : BaseViewModel<ProjectSummary>
	{
		public ObservableCollection<ProjectSummary> Projects { get; set; }
		public Command LoadItemsCommand { get; set; }

		public ProjectSummaryViewModel()
		{
			Title = AppStrings.ProjectsSummary;
			Projects = new ObservableCollection<ProjectSummary>();
		}
	}
}
