using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeamCityAPI;

namespace TCConnector
{
	public class TCConnection
	{
		private static object locker;

		private static TCConnection _instance;
		public static TCConnection Instance
		{
			get
			{
				lock (locker)
				{
					if (_instance == null)
					{
						lock(locker)
						{
							_instance = new TCConnection();
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

		public bool TestCoonection()
		{
			return _ServerConnection.TestConnection().Result;
		}
	}
}
