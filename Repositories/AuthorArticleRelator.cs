using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPocoWebTest.Poco;

namespace PetaPocoWebTest.Repositories
{
	public class AuthorArticleRelator
	{
		private Author _currentAuthor;

		public Author MapArticleToAuthor( Author author, Article article)
		{
			// Terminating call.  Since we can return null from this function
			// we need to be ready for PetaPoco to callback later with null
			// parameters
			if( author == null)
				return _currentAuthor;

			// Is this the same author as the current one we're processing
			if( _currentAuthor != null && _currentAuthor.Id == author.Id)
			{
				// Yes, just add this post to the current author's collection of posts
				_currentAuthor.Articles.Add( article);

				// Return null to indicate we're not done with this author yet
				return null;
			}

			// This is a different author to the current one, or this is the 
			// first time through and we don't have an author yet

			// Save the current author
			var previousAuthor = _currentAuthor;

			// Setup the new current author
			_currentAuthor = author;
			_currentAuthor.Articles = new List<Article>();

			if( article.Id != int.MinValue)
			{
				// Only add this article if the details aren't null
				_currentAuthor.Articles.Add( article);
			}

			// Return the now populated previous author (or null if first time through)
			return previousAuthor;
		}
	}
}