using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace PetaPocoWebTest.Poco
{
	public class Author
	{
		public int Id {get; set;}
		public string Name {get; set;}

		[ResultColumn]
		public List<Article> Articles {get; set;}

		public Author()
		{
			Articles = new List<Article>();
		}
	}
}