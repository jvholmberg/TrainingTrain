﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Views
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
