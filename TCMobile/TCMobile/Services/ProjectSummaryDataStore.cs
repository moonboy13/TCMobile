using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using TCMobile.Models;

namespace TCMobile.Services
{
	public class ProjectSummaryDataStore : IDataStore<ProjectSummary>
	{
		List<ProjectSummary> _Projects;

		public ProjectSummaryDataStore()
		{
			_Projects = new List<ProjectSummary>();
		}

		public Task<bool> AddItemAsync(ProjectSummary item)
		{
			_Projects.Add(item);
			return Task.FromResult(true);
		}

		public Task<bool> SetItems(IEnumerable<ProjectSummary> items)
		{
			_Projects = items.ToList();

			return Task.FromResult(true);
		}

		public Task<bool> DeleteItemAsync(string id) => throw new NotImplementedException();
		public Task<ProjectSummary> GetItemAsync(string id) => throw new NotImplementedException();
		public Task<IEnumerable<ProjectSummary>> GetItemsAsync(bool forceRefresh = false) => throw new NotImplementedException();
		public Task<bool> UpdateItemAsync(ProjectSummary item) => throw new NotImplementedException();
	}
}
