using System;
using System.IO;
using System.Xml;

namespace TCAPIGenerator
{
	class Program
	{
		static string _inputFile = Path.Combine("APIDefinition", "2019.1.65998", "APIDefinition.xml");
		// TODO: Verify these. Also, if this is done right then it belongs in its own project to be consumed by the app.
		string _modelOutput = Path.Combine("..", "..", "DataModels", "Models");
		string _apiOutput = Path.Combine("..", "..", "TeamCityAPI");

		/// <summary>
		/// The goal here is that this helper will automadically generate the requisite TeamCity API(s) based off a provided
		/// XML file generated from the rest/app/application.wadl file.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			using(var reader = XmlReader.Create(new FileStream(_inputFile, FileMode.Open)))
			{
				while (reader.Read())
				{
					// Only create classes for resources and ignore the swagger resources.
					if (reader.LocalName.Trim().ToLower().Equals("resource")
						&& !reader.GetAttribute("path").ToLower().Trim().Contains("swagger"))
					{
						Console.WriteLine(reader.LocalName);
					}
				}
			}
		}

		static void ParseResource(XmlReader reader)
		{ }
	}
}
