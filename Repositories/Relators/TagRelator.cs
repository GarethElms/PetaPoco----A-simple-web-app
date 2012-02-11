using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetaPocoWebTest.Poco;

namespace PetaPocoWebTest.Repositories
{
    public class TagRelator
	{
		public Tag current;
		public Tag Map( Tag tag, Article article, Author author)
		{
			// Terminating call.  Since we can return null from this function
			// we need to be ready for PetaPoco to callback later with null
			// parameters
			if( tag == null)
				return current;

			// Is this the same tag as the current one we're processing
			if( current != null && current.Id == tag.Id)
			{
				article.Author = author;

				// Yes, just add this article to the current tag's collection of articles
				if( article != null) current.Articles.Add( article);

				// Return null to indicate we're not done with this tag yet
				return null;
			}

			// This is a different tag to the current one, or this is the 
			// first time through and we don't have an tag yet

			// Save the current tag
			var prev = current;

			// Setup the new current tag
			current = tag;
			if( article != null && article.Id != int.MinValue)
			{
				article.Author = author;
				current.Articles.Add( article);
			}

			return prev;
		}
	}
}
