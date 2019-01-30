using System.Collections;

namespace MsvcAuth.Views
{
	public class MsvcAuthResponseBody
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public string Expiry { get; set; }
		public IEnumerable Message { get; set; }
	}
}
