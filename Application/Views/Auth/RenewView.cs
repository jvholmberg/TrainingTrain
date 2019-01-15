using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Views
{

	public class RenewResponse
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public DateTime ValidUntil { get; set; }
	}
}