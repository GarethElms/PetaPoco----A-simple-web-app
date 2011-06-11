using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPocoWebTest.Poco;

namespace PetaPocoWebTest.Repositories
{
	public class AuthorRepository : BaseRepository
	{
		public AuthorRepository( PetaPoco.Database database) : base( database)
		{
		}

		public Author RetrieveById( int authorId)
		{
			return _database.Fetch<Author, Article, Author>(
				new AuthorArticleRelator().MapArticleToAuthor,
				"select * from author left join article on article.author_id=author.id where author.id=@0 order by author.name asc", authorId).Single();
		}

		public List<Author> RetrieveAll()
		{
			return _database.Fetch<Author, Article, Author>(
				new AuthorArticleRelator().MapArticleToAuthor,
				"select * from author left join article on article.author_id=author.id order by author.name asc").ToList();
		}

		public bool Delete( int authorId)
		{
			using( var scope = _database.GetTransaction())
			{
				_database.Execute( "delete from article where author_id=@0", authorId);
				_database.Execute( "delete from author where id=@0", authorId);

				scope.Complete();
			}

			return true;
		}

		public bool Update( Author author)
		{
			_database.Update( author);

			return true;
		}

		public bool Insert( Author author)
		{
			_database.Insert( "author", "id", true, author);

			return true;
		}
	}
}