using System;
using System.Collections.Generic;
using System.Net.Http;
using TCMobile.Models;
using System.Text;

namespace TeamCityAPI
{
	class Projects
	{
		IServerConnection _serverConnection;

		public Projects(IServerConnection connection)
		{
			_serverConnection = connection;
		}

		public async void GetProjects()
		{
			HttpResponseMessage response = await _serverConnection.MakeRequest("projects");
		}


	}
}
