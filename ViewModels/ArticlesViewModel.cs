using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPocoWebTest.Poco;

namespace PetaPocoWebTest.ViewModels
{
	public class ArticlesViewModel
	{
		public List<Article> Articles {get; set;}
		public List<Author> Authors {get; set;}
	}
}