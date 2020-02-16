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
	public class ProjectViewModel : BaseViewModel<Project>
	{
		public Project Project { get => _Project; set => SetProperty(ref _Project, value); }
		public Command LoadItemCommand { get; set; }

		Projects _Projects;
		string _ProjectID;
		private Project _Project;

		public ProjectViewModel(string projectId)
		{
			LoadItemCommand = new Command(async () => await LoadItem());
			_Projects = new Projects();
			_ProjectID = projectId;
		}

		/// <summary>
		/// Load the specific project data
		/// </summary>
		/// <returns></returns>
		async Task LoadItem()
		{
			// Avoid double loading the data into the view
			if (IsBusy)
			{
				return;
			}

			IsBusy = true;

			try
			{
				Project = await _Projects.GetProject(_ProjectID).ConfigureAwait(false);
				Title = Project?.Name ?? AppStrings.ProjectNotFound;
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
