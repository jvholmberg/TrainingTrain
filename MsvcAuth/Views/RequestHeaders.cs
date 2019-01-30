namespace MsvcAuth.Views
{
	public class MsvcAuthRequestHeaders
	{
		public string MsvcAuthUserId { get; set; }
		public string MsvcAuthUserRole { get; set; }

		/// <summary>
		/// Returns UserId stored in jwt-token
		/// </summary>
		public int? GetUserId()
		{
			if (int.TryParse(MsvcAuthUserId, out int userId))
			{
				return userId;
			}
			return null;
		}

		/// <summary>
		/// Returns UserRole stored in jwt-token
		/// </summary>
		public string GetUserRole()
		{
			return MsvcAuthUserRole;
		}
	}
}
