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

		public static IServerConnection ServerConnection { get; private set; }
		private static TCConnectionData _ConnectionData;

		// Initialize the connection to the server.
		public void InitializeConnection(TCConnectionData tCConnectionData)
		{
			_ConnectionData = tCConnectionData;

			// If we have an existing connection, reset it.
			if (ServerConnection != null)
			{
				ServerConnection.Dispose();
				ServerConnection = null;
			}

			ServerConnection = _ConnectionData.ConnectionType switch
			{
				ConnectionType.Guest => new ServerConnection(_ConnectionData.Url,
															_ConnectionData.Port),
				ConnectionType.Basic => new ServerConnection(_ConnectionData.Url,
															_ConnectionData.Port,
															_ConnectionData.Username,
															_ConnectionData.Password),
				ConnectionType.Token => new ServerConnection(_ConnectionData.Url,
															_ConnectionData.Port,
															_ConnectionData.AuthToken),
				_ => throw new ArgumentException(ConnectionStrings.UnrecognizedType, _ConnectionData.ConnectionType.ToString()),
			};
		}

		public async Task<bool> TestConnection()
		{
			ServerConnection.SetTimeout(new TimeSpan(0, 0, 15));
			return await ServerConnection.TestConnection().ConfigureAwait(false);
		}

		public async Task<HttpResponseMessage> MakeRequest(string requestURI)
		{
			return await ServerConnection.MakeRequest(requestURI).ConfigureAwait(false);
		}
	}
}
