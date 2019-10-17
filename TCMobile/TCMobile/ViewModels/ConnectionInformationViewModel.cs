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

		private ConnectionType _selectedConnection;
		public ConnectionType SelectedConnection
		{
			get { return _selectedConnection; }
			set { SetProperty(ref _selectedConnection, value, onChanged: SelectedConnectionChange); }
		}

		private bool _enableUsernamePassword;
		public bool EnableUsernamePassword
		{
			get { return _enableUsernamePassword; }
			set { SetProperty(ref _enableUsernamePassword, value); }
		}

		private bool _enableToken;
		public bool EnableToken
		{
			get { return _enableToken; }
			set { SetProperty(ref _enableToken, value); }
		}

		public ConnectionInformationViewModel(TCConnectionData connectionData)
		{
			Title = AppStrings.ConnectionInfo;
			ConnectionData = connectionData ?? new TCConnectionData();
			ConnectionTypes = Enum.GetValues(typeof(ConnectionType)).Cast<ConnectionType>().ToList();
		}

		private void SelectedConnectionChange()
		{
			EnableUsernamePassword = (_selectedConnection == ConnectionType.Basic);
			EnableToken = (_selectedConnection == ConnectionType.Token);
		}
	}
}
