using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TCMobile.Models
{
	public class Project
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string ParentProjectId { get; set; }
		public string href { get; set; }
		public string WebUrl { get; set; }

		public ProjectSummary ParentProject { get; set; }

		[JsonProperty("buildTypes")]
		public BuildTypes Types { get; set; }

		[JsonProperty("projects")]
		public ChildProjects ProjectChildren { get; set; }
	}

	//-- These may eventually deserve their own files, will evaluate that later.
	public class BuildTypes
	{
		public int Count { get; set; }
		[JsonProperty(Required = Required.Default)]
		public List<TypeBuild> BuildType { get; set;}
	}

	public class TypeBuild
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string ProjectName { get; set; }

		public string ProjectId { get; set; }

		public string href { get; set; }
		public string WebUrl { get; set; }
	}

	public class ChildProjects
	{
		public int Count { get; set; }

		[JsonProperty(Required = Required.Default)]
		public List<Project> Projects { get; set; }
	}
}
