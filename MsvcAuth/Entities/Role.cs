namespace MsvcAuth.Entities
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
