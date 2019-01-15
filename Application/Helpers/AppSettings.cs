using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helpers
{
	public class AppSettings
	{
		// JWT-token validation settings
		public string Secret { get; set; }
		public bool RequireHttpsMetadata { get; set; }
		public bool SaveToken { get; set; }
		public bool ValidateIssuerSigningKey { get; set; }
		public bool ValidateIssuer { get; set; }
		public bool ValidateAudience { get; set; }
		public bool ValidateLifeTime { get; set; }
		public string Issuer { get; set; }
	}
}
