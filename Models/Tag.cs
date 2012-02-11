using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PetaPocoWebTest.Poco
{
	[TableName( "tag")]
	public class Tag
	{
		public int Id {get; set;}

		[Column( "tagName")]
		public string TagName {get; set;}

		public Tag() : this( string.Empty)
		{
		}

		[ResultColumn]
		public List<Article> Articles {get; set;}

		public override string ToString()
		{
			return this.TagName;
		}

		public Tag( string tagName)
		{
			this.TagName = tagName;
			this.Id = int.MinValue;
			this.Articles = new List<Article>();
		}
	}
}