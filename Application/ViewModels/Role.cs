namespace Application.ViewModels
{
	public class Permission
	{
		public bool Create { get; set; }
		public bool Read { get; set; }
		public bool Update { get; set; }
		public bool Delete { get; set; }
		public bool Special { get; set; }

		public Permission(Entities.Role role)
		{
			Create = role.Create;
			Read = role.Read;
			Update = role.Update;
			Delete = role.Delete;
			Special = role.Special;
		}
	}
}
