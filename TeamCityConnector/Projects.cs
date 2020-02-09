using System;
using System.Collections.Generic;
using System.Net.Http;
using TCMobile.Models;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using TeamCityAPI;

namespace TCConnection
{
	public class Projects
	{
		ProjectsConnector _Projects;

		public Projects()
		{
			// TODO: This is weird. This responsibility belongs on the API
			_Projects = new ProjectsConnector(Connection.ServerConnection);
		}


		public async Task<List<ProjectSummary>> GetProjects()
		{
			HttpResponseMessage response = await _Projects.GET_serveProjects(null, null);

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException(response.ReasonPhrase);
			}

			string data = await response.Content.ReadAsStringAsync();
			var responseData = JObject.Parse(data);
			var projectSummaries = new List<ProjectSummary>();

			foreach (JToken proj in responseData["project"].Children())
			{
				var project = proj.ToObject<ProjectSummary>();
				projectSummaries.Add(project);
			}

			return projectSummaries;
		}

		public async Task<Project> GetProject(string projId)
		{
			HttpResponseMessage response = await _Projects.GET_serveProject_projectLocator(null, $"id:{projId.Trim()}");

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException(response.ReasonPhrase);
			}

			string data = await response.Content.ReadAsStringAsync();
			return JObject.Parse(data).ToObject<Project>();
		}
	}
}
