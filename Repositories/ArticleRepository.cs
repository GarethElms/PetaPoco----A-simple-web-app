using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using System.Configuration;
using PetaPocoWebTest.Poco;

namespace PetaPocoWebTest.Repositories
{
	public class ArticleRepository : BaseRepository
	{
		public ArticleRepository( PetaPoco.Database database) : base( database)
		{
		}

		public Article RetrieveById( int articleId)
		{
			return _database.Query<Article, Author>( "select * from article join author on author.id = article.author_id where article.id=@0 order by article.date desc", articleId).Single<Article>();
		}

		public List<Article> RetrieveAll()
		{
			return _database.Fetch<Article, Author>( "select * from article join author on author.id = article.author_id").ToList();
		}

		public List<Article> RetrieveAllByAuthor( int authorId)
		{
			return _database.Fetch<Article>( "select * from article where author_id=@0", authorId).ToList();
		}

		public bool Delete( int articleId)
		{
			_database.Delete( "article", "id", null, articleId);

			return true;
		}

		public bool Update( Article article)
		{
			_database.Update( article);

			return true;
		}

		public bool Insert( Article article)
		{
			if( article.AuthorId == null) throw new Exception( "An article must have an author. Have you created any authors yet?");

			_database.Insert( "article", "id", true, article);

			return true;
		}
	}
}