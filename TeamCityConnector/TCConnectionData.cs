using System;
using System.Collections.Generic;
using System.Text;
using TeamCityAPI;

namespace TCConnection
{
	public class TCConnectionData
	{
		public ConnectionType ConnectionType;
		public string Username;
		public string Password;
		public string AuthToken;
		public string Url;
		public int Port;
	}
}
