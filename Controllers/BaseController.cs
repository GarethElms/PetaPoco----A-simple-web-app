using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetaPocoWebTest.Repositories;
using PetaPocoWebTest.Database;

namespace PetaPocoWebTest.Controllers
{
    public class BaseController : Controller
    {
		protected DatabaseWithProfiling _database;
		protected ArticleRepository _articles;
		protected AuthorRepository _authors;

		protected override RedirectToRouteResult RedirectToAction(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues)
		{
			DatabaseWithProfiling.PersistGlimpseAfterRedirect( TempData);
			return base.RedirectToAction(actionName, controllerName, routeValues);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting( filterContext);

			_database = new DatabaseWithProfiling( "PetaPocoWebTest");
			_database.EnsureDatabase();

			_articles = new ArticleRepository( _database);
			_authors = new AuthorRepository( _database);

			HttpContext.Items[DatabaseWithProfiling.PetaKey] = TempData[DatabaseWithProfiling.PetaKey];

			/*
			if( filterContext.HttpContext.Session[PetaPocoWebTest.Database.DatabaseWithProfiling.PetaKey] != null)
			{
				var petaPocoGlimpseFromPreviousRequest = ((List<DatabaseWithProfiling.PetaPocoSqlInfo>)filterContext.HttpContext.Session[DatabaseWithProfiling.PetaKey]);

				foreach( var petaPocoGlimpse in petaPocoGlimpseFromPreviousRequest)
				{
					DatabaseWithProfiling.CurrentRequestInfo.InsertRange( 0, petaPocoGlimpseFromPreviousRequest);
				}

				filterContext.HttpContext.Session[PetaPocoWebTest.Database.DatabaseWithProfiling.PetaKey] = null;
			}
			 * */
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);

			/*
			if( filterContext.HttpContext.Items[PetaPocoWebTest.Database.DatabaseWithProfiling.PetaKey] != null)
			{
				filterContext.HttpContext.Session[PetaPocoWebTest.Database.DatabaseWithProfiling.PetaKey]
					= filterContext.HttpContext.Items[PetaPocoWebTest.Database.DatabaseWithProfiling.PetaKey];
			}*/
		}

		protected void ThisControllerLivesInWebsiteSection( string sectionName)
		{
			ViewData["WebsiteSectionName"] = sectionName;
		}
    }
}
