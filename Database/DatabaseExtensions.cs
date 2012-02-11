using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace PetaPocoWebTest
{
	public static class DatabaseExtensions
	{
		public static void EnsureDatabase( this PetaPoco.Database database)
		{
			if( database.ExecuteScalar<bool>( _SQL_check_if_tables_exists) == false)
			{
				database.Execute( _SQL_create_tables);
			}
		}
		public static string _SQL_check_if_tables_exists =
			@"if (exists (select * from information_schema.tables where table_schema = 'dbo' and table_name = 'article'))
				begin
					select 1
				end
				select 0";

		private static string _SQL_create_tables =
			@"if( object_id('author') is null)
				begin
					CREATE TABLE [dbo].[author](
						[id] [int] IDENTITY(1,1) NOT NULL,
						[name] [nvarchar](100) NOT NULL,
						CONSTRAINT [PK_author] PRIMARY KEY CLUSTERED 
						(
							[id] ASC
						)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY]
				end

				if( object_id('article') is null)
				begin
					CREATE TABLE [dbo].[article](
						[id] [int] IDENTITY(1,1) NOT NULL,
						[title] [nvarchar](100) NOT NULL,
						[body] [text] NOT NULL,
						[author_id] [int] NOT NULL,
						[date] [datetime] NOT NULL,
						CONSTRAINT [PK_article] PRIMARY KEY CLUSTERED 
						(
							[id] ASC
						)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

					ALTER TABLE [dbo].[article]  WITH CHECK ADD  CONSTRAINT [FK_article_author] FOREIGN KEY([author_id])
					REFERENCES [dbo].[author] ([id])
					ALTER TABLE [dbo].[article] CHECK CONSTRAINT [FK_article_author]

					ALTER TABLE [dbo].[author]  WITH CHECK ADD  CONSTRAINT [FK_author_article_author] FOREIGN KEY([id])
					REFERENCES [dbo].[author] ([id])
					ALTER TABLE [dbo].[author] CHECK CONSTRAINT [FK_author_article_author]
				end
				
				if( object_id('tag') is null)
				begin
					CREATE TABLE [dbo].[tag](
						[id] [int] IDENTITY(1,1) NOT NULL,
						[tagName] [varchar](max) NOT NULL,
						CONSTRAINT [PK_tag] PRIMARY KEY CLUSTERED 
							([id] ASC
							)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
						) ON [PRIMARY]
				end

				if( object_id('articleTag') is null)
				begin
					CREATE TABLE [dbo].[articleTag](
						[articleId] [int] NOT NULL,
						[tagId] [int] NOT NULL
					) ON [PRIMARY]

					ALTER TABLE [dbo].[articleTag]  WITH CHECK ADD  CONSTRAINT [FK_articleTag_article] FOREIGN KEY([articleId])
					REFERENCES [dbo].[article] ([id])
					ALTER TABLE [dbo].[articleTag] CHECK CONSTRAINT [FK_articleTag_article]

					ALTER TABLE [dbo].[articleTag]  WITH CHECK ADD  CONSTRAINT [FK_articleTag_tag] FOREIGN KEY([tagId])
					REFERENCES [dbo].[tag] ([id])
					ALTER TABLE [dbo].[articleTag] CHECK CONSTRAINT [FK_articleTag_tag]
				end
				";
	}
}