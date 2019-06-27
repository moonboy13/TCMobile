using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TCAPIGenerator
{
	class Program
	{
		#region Templates
		static string _ClassHeaderTemplate =
			"using System;" + Environment.NewLine + Environment.NewLine +
			"namespace FOO" + Environment.NewLine + "{{" + Environment.NewLine +
			"\tpublic class {0}" + Environment.NewLine + "\t{{" + Environment.NewLine +
			"\t\tstring _rootPath = \"{1}\";" + Environment.NewLine + Environment.NewLine;

		static string _MethodDefintionTemplate =
			"\t\t/// <summary>" + Environment.NewLine +
			"\t\t/// {0}" + Environment.NewLine +
			"\t\t/// </summary>" + Environment.NewLine +
			"\t\tpublic void {1}({2})" + Environment.NewLine +
			"\t\t{{" + Environment.NewLine;

		static string _MethodEndTemplate = $"\t\t}}{Environment.NewLine}{Environment.NewLine}";

		static string _EndTemplate =
			"\t}" + Environment.NewLine +
			"}";
		#endregion

		static string _inputFile = Path.Combine("APIDefinition", "2019.1.65998", "APIDefinition.xml");
		// TODO: Verify these. Also, if this is done right then it belongs in its own project to be consumed by the app.
		static string _modelOutput = Path.Combine("..", "..", "DataModels", "Models");
		static string _apiOutput = Path.Combine("..", "..", "TeamCityAPI");
		static TextInfo _TI = new CultureInfo("en-US", false).TextInfo;



		/// <summary>
		/// The goal here is that this helper will automadically generate the requisite TeamCity API(s) based off a provided
		/// XML file generated from the rest/app/application.wadl file.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			XElement definition = XElement.Load(_inputFile);

			var resources =
					from elem in definition.Descendants("resource")
					where !((string)elem.Attribute("path")).Trim().ToLower().Contains("swagger") &&
						elem.Parent.Name.LocalName.Trim().ToLower() == "resources"
					select elem;

			foreach (var resource in resources)
			{
				ProcessResource(resource);
			}
		}

		static void ProcessResource(XElement resource)
		{
			// Pull the root path off the resource. The last portion of this name will server as the class name.
			string path = resource.Attribute(XName.Get("path")).Value;
			string className = _TI.ToTitleCase(path.Split('/').Last());
			string filePath = @"/test/" + className + ".cs";

			File.WriteAllText(filePath, string.Format(_ClassHeaderTemplate, className, path));

			// First parse all the root level methods and generate a function for them.
			foreach (var method in resource.Elements("method"))
			{
				AddMethod(filePath, method);
			}

			File.AppendAllText(filePath, _EndTemplate);
		}

		static void AddMethod(string filePath, XElement methodElement)
		{
			string methodName = string.Format("{0}_{1}", methodElement.Attribute(XName.Get("name")).Value, methodElement.Attribute(XName.Get("id")).Value);
			string methodDescription = methodElement.Elements(XName.Get("doc")).FirstOrDefault()?.Value.Trim() ?? String.Empty;

			File.AppendAllText(filePath, String.Format(_MethodDefintionTemplate, methodDescription, methodName, String.Empty));
			File.AppendAllText(filePath, _MethodEndTemplate);
		}

	}
}
