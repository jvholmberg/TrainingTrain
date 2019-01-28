namespace Application.ViewModels
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

		public Role(Entities.Role role)
		{
			Id = role.Id;
			Name = role.Name;
			Create = role.Create;
			Read = role.Read;
			Update = role.Update;
			Delete = role.Delete;
			Special = role.Special;
		}
	}
}
