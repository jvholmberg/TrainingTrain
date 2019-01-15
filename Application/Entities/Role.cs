using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Entities
{
	public class Role
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Create { get; set; }
		public bool Read { get; set; }
		public bool Update { get; set; }
		public bool Delete { get; set; }
		public bool Special { get; set; }
	}
}
