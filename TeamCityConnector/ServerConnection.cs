using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TeamCityAPI
{
	/// <summary>
	/// Responsible for creating the connection to the TeamCity server instance.
	/// Consider using the factory design pattern here (no public constructors).
	/// </summary>
	public class ServerConnection : IDisposable
	{
		//TODO abstract to an interface for easy injection
		//TODO implement access token for authentication
		//TODO implement guest user for authentication
		string _ServerURL = "http://192.168.56.1:8080/basicAuth/app/rest/2018.1";
		string _Username;
		string _Password;
		string _Authorization = "Basic";
		AuthenticationHeaderValue _AuthenticationHeaderValue;
		HttpClient _client;

		/// <summary>
		/// Initialize a new instance of the TeamCity server connection class.
		/// </summary>
		/// <param name="serverURL"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public ServerConnection(string username, string password)
		{
			_Username = username;
			_Password = password;
			_AuthenticationHeaderValue = new AuthenticationHeaderValue(_Authorization, 
				Convert.ToBase64String(Encoding.ASCII.GetBytes(String.Format("{0}:{1}", _Username, _Password))));
			_client = new HttpClient();
		}

		/// <summary>
		/// Dispose uneeded resources.
		/// </summary>
		public void Dispose() => ((IDisposable)_client).Dispose();

		/// <summary> 
		/// Validate we can connect succesfully to the server.
		/// </summary>
		public async void TestConnection()
		{
			_client.DefaultRequestHeaders.Authorization = _AuthenticationHeaderValue;
			HttpResponseMessage response = await _client.GetAsync(_ServerURL);

			// Will eventually need this to give some error handling if the connection is bad.
		}


	}
}
