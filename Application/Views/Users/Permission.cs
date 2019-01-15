using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Views
{
	public class Role
	{
		public bool Create { get; set; }
		public bool Read { get; set; }
		public bool Update { get; set; }
		public bool Delete { get; set; }
		public bool Special { get; set; }

		public Role(Entities.Role role)
		{
			Create = role.Create;
			Read = role.Read;
			Update = role.Update;
			Delete = role.Delete;
			Special = role.Special;
		}
	}
}
