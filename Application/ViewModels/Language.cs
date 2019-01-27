namespace Application.ViewModels
{
	public class Language
	{
		public string Code { get; set; }
		public string Name { get; set; }

		public Language(Entities.Language language)
		{
			Code = language.Code;
			Name = language.Name;
		}
	}
}
