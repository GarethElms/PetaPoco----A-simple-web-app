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
		private TagRepository _tagRepository {get; set;}
		public ArticleRepository( TagRepository tagRepository, PetaPoco.Database database) : base( database)
		{
			_tagRepository = tagRepository;
		}

		public Article RetrieveById( int articleId)
		{
			return _database.Fetch<Article, Author, Tag, Article>(
				new ArticleRelator().Map,
				"select * from article " + 
				"join author on author.id = article.author_id " +
				"left outer join articleTag on articleTag.articleId = article.id " + 
				"left outer join tag on tag.id=articleTag.tagId " + 
				"where article.id=@0 " + 
				"order by article.title asc", articleId).Single();
		}

		public List<Article> RetrieveAll()
		{
			return _database.Fetch<Article, Author, Tag, Article>(
				new ArticleRelator().Map,
				"select * from article " + 
				"join author on author.id = article.author_id " +
				"left outer join articleTag on articleTag.articleId = article.id " + 
				"left outer join tag on tag.id=articleTag.tagId " + 
				"order by article.title asc").ToList();
		}

		public List<Article> RetrieveAllByAuthor( int authorId)
		{
			return _database.Fetch<Article, Author, Tag, Article>(
				new ArticleRelator().Map,
				"select * from article " + 
				"join author on author.id = article.author_id " +
				"left outer join articleTag on articleTag.articleId = article.id " + 
				"left outer join tag on tag.id=articleTag.tagId " + 
				"where article.authorId=@0" + 
				"order by article.title asc", authorId).ToList();
		}

		public bool Delete( int articleId)
		{
			using( var scope = _database.GetTransaction())
			{
				_database.Execute( "delete from articleTag where articleTag.articleId=@0", articleId);
				_database.Delete( "article", "id", null, articleId);
				
				scope.Complete();
			}

			return true;
		}

		public bool Update( Article article)
		{
			using( var scope = _database.GetTransaction())
			{
				_database.Update( article);
				_database.Execute( "delete from articleTag where articleTag.articleId=@0", article.Id);
				PersistTags( article);

				scope.Complete();
			}

			return true;
		}

		public bool Insert( Article article)
		{
			if( article.AuthorId == null) throw new Exception( "An article must have an author. Have you created any authors yet?");

			using( var scope = _database.GetTransaction())
			{
				_database.Insert( "article", "id", true, article);
				PersistTags( article);

				scope.Complete();
			}

			return true;
		}

		private void PersistTags( Article article)
		{
			if( article.Tags != null && article.Tags.Count > 0)
			{
				foreach( var tag in article.Tags.GroupBy( t => t.TagName).Select( tagsWithSameName => tagsWithSameName.First()))
				{
					if( ! string.IsNullOrEmpty( tag.TagName))
					{
						var existingTag = _tagRepository.RetrieveByTag( tag.TagName);
						var tagId = 0;
						if( existingTag == null)
						{
							var newTagId = _database.Insert( "tag", "id", true, tag);
							tagId = tag.Id;
						}
						else
						{
							tagId = existingTag.Id;
						}

						_database.Execute( "insert into articleTag values(@0, @1)", article.Id, tagId);
					}
				}
			}
		}
	}
}