using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using System.Configuration;
using PetaPocoWebTest.Poco;

namespace PetaPocoWebTest.Repositories
{
	public class TagRepository : BaseRepository
	{
		public TagRepository( PetaPoco.Database database) : base( database)
		{
		}

		public Tag RetrieveById( int tagId)
		{
			return _database.Fetch<Tag, Article, Tag>(
				new TagRelator().Map,
				"select * from tag " +
				"left outer join articleTag on articleTag.tagId = tag.id " +
				"left outer join article on article.id = articleTag.articleId " +
				"where tag.id=@0 order by tag.tagName asc", tagId).SingleOrDefault<Tag>();
		}

		public Tag RetrieveByTag( string tagName)
		{
			return _database.Fetch<Tag, Article, Tag>(
				new TagRelator().Map,
				"select * from tag " +
				"left outer join articleTag on articleTag.tagId = tag.id " +
				"left outer join article on article.id = articleTag.articleId " +
				"where tag.tagName=@0 order by tag.tagName asc", tagName).SingleOrDefault<Tag>();
		}

		public List<Tag> RetrieveAll()
		{
			return _database.FetchOneToMany<Tag, Article>(
				tag => tag.Id,
				article => article.Id != int.MinValue,
				"select * from tag " +
				"left outer join articleTag on articleTag.tagId = tag.id " +
				"left outer join article on article.id = articleTag.articleId ").ToList();
		}

		public List<Tag> RetrieveAllForArticle( int articleId)
		{
			return _database.Fetch<Tag>( "select * from tag where articleTag.articleId=@0", articleId).ToList();
		}

		public bool Delete( int tagId)
		{
			using( var scope = _database.GetTransaction())
			{
				_database.Execute( "delete from articleTag where articleTag.tagId=@0", tagId);
				_database.Delete( "tag", "id", null, tagId);

				scope.Complete();
			}

			return true;
		}

		public bool Update( Tag tag)
		{
			_database.Update( tag);

			return true;
		}

		public bool Insert( Tag tag)
		{
			_database.Insert( "tag", "id", true, tag);

			return true;
		}
	}
}