using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPocoWebTest.Poco;

namespace PetaPocoWebTest.ViewModels
{
	public class ArticleEditViewModel
	{
		public Article Article {get; set;}
		public List<Author> Authors {get; set;}
		public List<Tag> Tags {get; set;}
	}
}