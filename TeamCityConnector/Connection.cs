using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeamCityAPI;

namespace TCConnection
{
	public class Connection
	{
		private static object locker = new object();

		private static Connection _instance;
		public static Connection Instance
		{
			get
			{
				lock (locker)
				{
					if (_instance == null)
					{
						lock(locker)
						{
							_instance = new Connection();
						}
					}
				}
				return _instance;
			}
		}

		private static IServerConnection _ServerConnection;
		private static TCConnectionData _ConnectionData;

		// Initialize the connection to the server.
		public void InitializeConnection(TCConnectionData tCConnectionData)
		{
			_ConnectionData = tCConnectionData;

			// If we have an existing connection, reset it.
			if (_ServerConnection != null)
			{
				_ServerConnection.Dispose();
				_ServerConnection = null;
			}

			switch (_ConnectionData.ConnectionType)
			{
				case ConnectionType.Guest:
					_ServerConnection = new ServerConnection(_ConnectionData.Url,
											 _ConnectionData.Port);
					break;
				case ConnectionType.Basic:
					_ServerConnection = new ServerConnection(_ConnectionData.Url,
											 _ConnectionData.Port,
											 _ConnectionData.Username,
											 _ConnectionData.Password);
					break;
				case ConnectionType.Token:
					_ServerConnection = new ServerConnection(_ConnectionData.Url,
											 _ConnectionData.Port,
											 _ConnectionData.AuthToken);
					break;
				default:
					throw new ArgumentException(ConnectionStrings.UnrecognizedType, _ConnectionData.ConnectionType.ToString());
			}
		}

		public async Task<bool> TestConnection()
		{
			_ServerConnection.SetTimeout(new TimeSpan(0, 0, 15));
			return await _ServerConnection.TestConnection().ConfigureAwait(false);
		}

		public async Task<HttpResponseMessage> MakeRequest(string requestURI)
		{
			return await _ServerConnection.MakeRequest(requestURI).ConfigureAwait(false);
		}
	}
}
