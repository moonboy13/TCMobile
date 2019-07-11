using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace TCAPIGenerator
{
	class Program
	{
		#region Templates
		static string _ClassHeaderTemplate =
			"using System;" + Environment.NewLine +
			"using System.Net.Http;" + Environment.NewLine +
			"using System.Threading.Tasks;" + Environment.NewLine + Environment.NewLine +
			"namespace FOO" + Environment.NewLine + "{{" + Environment.NewLine +
			"\tpublic class {0}" + Environment.NewLine + "\t{{" + Environment.NewLine +
			"\t\tstring _rootPath = \"{1}\";" + Environment.NewLine +
			"\t\tIServerConnection _serverConnection;" + Environment.NewLine + Environment.NewLine +
			"\t\tpublic {0}(IServerConnection connection)" + Environment.NewLine +
			"\t\t{{" + Environment.NewLine +
			"\t\t\t_serverConnection = connection;" + Environment.NewLine +
			"\t\t}}" + Environment.NewLine + Environment.NewLine;

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

		#region XML Attributes
		static XName _XPATH = XName.Get("path");
		static XName _XNAME = XName.Get("name");
		static XName _XID = XName.Get("id");
		static XName _XDOC = XName.Get("doc");
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
				ProcessRootResource(resource);
			}
		}

		static void ProcessRootResource(XElement resource)
		{
			// Pull the root path off the resource. The last portion of this name will server as the class name.
			string path = resource.Attribute(_XPATH).Value;
			string className = _TI.ToTitleCase(path.Split('/').Last());
			string filePath = @"/test/" + className + ".cs";

			File.WriteAllText(filePath, string.Format(_ClassHeaderTemplate, className, path));

			// First parse all the root level methods and generate a function for them.
			foreach (var method in resource.Elements("method"))
			{
				AddMethod(filePath, method);
			}

			// Next parse sub-resource nodes. Pull out the additional path element
			// and the new methods which utilize it.
			foreach (var subResorce in resource.Elements("resource"))
			{
				string subPath = subResorce.Attribute(_XPATH).Value;

				foreach (var method in subResorce.Elements("method"))
				{
					AddMethod(filePath, method, subPath);
				}
			}

			File.AppendAllText(filePath, _EndTemplate);
		}

		static void AddMethod(string filePath, XElement methodElement, string subUri = null)
		{
			string methodName = string.Format("{0}_{1}", methodElement.Attribute(_XNAME).Value, methodElement.Attribute(_XID).Value);
			string methodDescription = methodElement.Elements(_XDOC).FirstOrDefault()?.Value.Trim() ?? string.Empty;
			var methodParameters = new StringBuilder();
			string subUriString = string.Empty;

			if (subUri != null)
			{
				subUriString = string.Format("\t\t\tstring subUri = $\"{0}\";" + Environment.NewLine, subUri);
				foreach (Match param in Regex.Matches(subUri, @"\/{(\w+)}"))
				{
					if (methodParameters.Length != 0)
					{
						methodParameters.Append(", ");
					}

					methodParameters.Append(string.Format("string {0}", param.Groups[1].Value));
				}
			}
			else
			{
				subUriString = string.Format("\t\t\tstring subUri = string.Emtpy;" + Environment.NewLine);
			}

			// Write out the information to our file.
			File.AppendAllText(filePath, string.Format(_MethodDefintionTemplate, methodDescription, methodName, methodParameters.ToString()));
			File.AppendAllText(filePath, subUriString);
			File.AppendAllText(filePath, _MethodEndTemplate);
		}

	}
}
