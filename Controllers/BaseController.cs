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
		protected DatabaseWithMVCMiniProfilerAndGlimpse _database;
		protected ArticleRepository _articles;
		protected AuthorRepository _authors;

		protected override RedirectToRouteResult RedirectToAction(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues)
		{
			DatabaseWithGlimpseProfiling.PersistGlimpseAfterRedirect( TempData);
			return base.RedirectToAction(actionName, controllerName, routeValues);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting( filterContext);

			_database = new DatabaseWithMVCMiniProfilerAndGlimpse( "PetaPocoWebTest");
			_database.EnsureDatabase();

			_articles = new ArticleRepository( _database);
			_authors = new AuthorRepository( _database);

			HttpContext.Items[DatabaseWithGlimpseProfiling.PetaKey] = TempData[DatabaseWithGlimpseProfiling.PetaKey];
		}

		protected void ThisControllerLivesInWebsiteSection( string sectionName)
		{
			ViewData["WebsiteSectionName"] = sectionName;
		}
    }
}
