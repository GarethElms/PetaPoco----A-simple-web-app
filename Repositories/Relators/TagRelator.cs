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
		public Tag Map( Tag tag, Article article)
		{
			// Terminating call.  Since we can return null from this function
			// we need to be ready for PetaPoco to callback later with null
			// parameters
			if( tag == null)
				return current;

			// Is this the same author as the current one we're processing
			if( current != null && current.Id == tag.Id)
			{
				if( article != null) current.Articles.Add( article);

				// Return null to indicate we're not done with this author yet
				return null;
			}

			// This is a different author to the current one, or this is the 
			// first time through and we don't have an author yet

			// Save the current author
			var prev = current;

			// Setup the new current author
			current = tag;
			if( article != null && article.Id != int.MinValue) current.Articles.Add( article);
			//current.Tags = new List<Tag>();
			//current.posts.Add(p);

			// Return the now populated previous author (or null if first time through)
			return prev;
		}
	}
}
