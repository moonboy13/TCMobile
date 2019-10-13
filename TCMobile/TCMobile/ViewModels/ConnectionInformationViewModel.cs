using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCConnector;
using TeamCityAPI;

namespace TCMobile.ViewModels
{
	public class ConnectionInformationViewModel : BaseViewModel<TCConnectionData>
	{
		public TCConnectionData ConnectionData;
		public List<ConnectionType> ConnectionTypes;

		public ConnectionInformationViewModel(TCConnectionData connectionData)
		{
			Title = AppStrings.ConnectionInfo;
			ConnectionData = connectionData ?? new TCConnectionData();
			ConnectionTypes = Enum.GetValues(typeof(ConnectionType)).Cast<ConnectionType>().ToList();
		}
	}
}
