using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetaPocoWebTest.Poco;

namespace PetaPocoWebTest.Repositories
{
    public class ArticleRelator
	{
		public Article current;
		public Article Map( Article article, Author author, Tag tag)
		{
			// Terminating call.  Since we can return null from this function
			// we need to be ready for PetaPoco to callback later with null
			// parameters
			if( article == null)
				return current;

			// Is this the same article as the current one we're processing
			if( current != null && current.Id == article.Id)
			{
				if( current.Author == null) current.Author = author;

				// Yes, just add this tag to the current article's collection of tags
				current.Tags.Add( tag);

				// Return null to indicate we're not done with this article yet
				return null;
			}

			// This is a different author to the current one, or this is the 
			// first time through and we don't have an author yet

			// Save the current author
			var prev = current;

			// Setup the new current author
			current = article;
			current.Author = author;
			if( tag.Id != int.MinValue) current.Tags.Add( tag);
			//current.Tags = new List<Tag>();
			//current.posts.Add(p);

			// Return the now populated previous author (or null if first time through)
			return prev;
		}
	}
}
