using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TCMobile.Models;
using TCMobile.Views;
using System.Collections.Generic;
using TeamCityAPI;

namespace TCMobile.ViewModels
{
	public class ProjectSummaryViewModel : BaseViewModel<ProjectSummary>
	{
		public ObservableCollection<ProjectSummary> Projects { get; set; }
		public Command LoadItemsCommand { get; set; }

		IServerConnection _Connection;
		Projects _Projects;

		public ProjectSummaryViewModel()
		{
			Title = AppStrings.ProjectsSummary;
			LoadItemsCommand = new Command(async () => await LoadItems());
			_Connection = new ServerConnection("http://192.168.56.1", 8080);
			_Projects = new Projects(_Connection);
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
