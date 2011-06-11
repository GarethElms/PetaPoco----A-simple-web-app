using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using System.Configuration;
using PetaPocoWebTest.Database;

namespace PetaPocoWebTest.Repositories
{
	public class BaseRepository
	{
		protected PetaPoco.Database _database;

		public BaseRepository( PetaPoco.Database database)
		{
			_database = database;
		}
	}
}