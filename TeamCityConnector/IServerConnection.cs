using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TeamCityAPI
{
	public interface IServerConnection
	{
		Task<HttpResponseMessage> MakeRequest(string requestURI);
	}
}