using System;
using System.Collections.Generic;
using System.Net.Http;
using TCMobile.Models;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace TCConnection
{
	public class Projects
	{
		public async Task<List<ProjectSummary>> GetProjects()
		{
			HttpResponseMessage response = await Connection.Instance.MakeRequest("projects");

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException(response.ReasonPhrase);
			}

			string data = await response.Content.ReadAsStringAsync();
			JObject responseData = JObject.Parse(data);
			List<ProjectSummary> projectSummaries = new List<ProjectSummary>();

			foreach (JToken proj in responseData["project"].Children())
			{
				ProjectSummary project = proj.ToObject<ProjectSummary>();
				projectSummaries.Add(project);
			}

			return projectSummaries;
		}


	}
}
