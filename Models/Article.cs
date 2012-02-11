using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PetaPocoWebTest.Poco
{
	public class Article
	{
		public int Id {get; set;}
		public string Title {get; set;}
		public string Body {get; set;}

		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode=true)]
		public DateTime Date {get; set;}

		[ResultColumn]
		public Author Author {get; set;}

		[Column( "author_id")]
		[DisplayName( "Author")]
		public int? AuthorId {get; set;}

		[ResultColumn]
		public List<Tag> Tags {get; set;}

		public Article()
		{
			Date = DateTime.Now;
			Id = int.MinValue;
			Tags = new List<Tag>();
		}
	}
}