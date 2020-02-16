using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TCMobile.Models;
using TCMobile.Views;
using System.Collections.Generic;
using TCConnection;

namespace TCMobile.ViewModels
{
	public class ProjectSummaryViewModel : BaseViewModel<ProjectSummary>
	{
		public ObservableCollection<ProjectSummary> Projects { get; set; }
		public Command LoadItemsCommand { get; set; }

		Projects _Projects;

		public ProjectSummaryViewModel()
		{
			Title = AppStrings.ProjectsSummary;
			LoadItemsCommand = new Command(async () => await LoadItems());
			_Projects = new Projects();
		}

		/// <summary>
		/// Responsible for firing off the request to the server and parsing the response.
		/// </summary>
		/// <returns></returns>
		async Task LoadItems()
		{
			// Avoid double loading the data into the view
			if(IsBusy)
			{
				return;
			}

			IsBusy = true;

			try
			{
				var projectsTask = _Projects.GetProjects();
				var updatedProjects = new ObservableCollection<ProjectSummary>();
				Projects?.Clear();
				IEnumerable<ProjectSummary> projects = await projectsTask;
				foreach (var project in projects)
				{
					if(!project.Id.Trim().ToLower().Equals("_root"))
						updatedProjects.Add(project);
				}

				Projects = updatedProjects;
				OnPropertyChanged(nameof(Projects));
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				DisplayErrorHandler?.Invoke(ex.Message);
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
