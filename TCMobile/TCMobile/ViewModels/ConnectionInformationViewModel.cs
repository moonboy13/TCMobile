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
		private TCConnectionData _conData;
		public TCConnectionData ConnectionData
		{
			get { return _conData; }
			set { SetProperty(ref _conData, value); }
		}

		private List<ConnectionType> _conTypes;
		public List<ConnectionType> ConnectionTypes
		{
			get { return _conTypes; }
			set { SetProperty(ref _conTypes, value); }
		}

		public ConnectionInformationViewModel(TCConnectionData connectionData)
		{
			Title = AppStrings.ConnectionInfo;
			ConnectionData = connectionData ?? new TCConnectionData();
			ConnectionTypes = Enum.GetValues(typeof(ConnectionType)).Cast<ConnectionType>().ToList();
		}
	}
}
