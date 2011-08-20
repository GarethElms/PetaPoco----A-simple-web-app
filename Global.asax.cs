using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcMiniProfiler;

namespace PetaPocoWebTest
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters( GlobalFilterCollection filters)
		{
			filters.Add( new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute( "{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Author", // Route name
				"Author/{action}/{authorId}", // URL with parameters
				new { controller = "Author", action = "Index", authorId = UrlParameter.Optional } // Parameter defaults
			);

			routes.MapRoute(
				"Article", // Route name
				"Article/{action}/{articleId}", // URL with parameters
				new { controller = "Article", action = "Index", articleId = UrlParameter.Optional } // Parameter defaults
			);

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_BeginRequest()
		{
			if( Request.IsLocal)
			{
				MiniProfiler.Start();
			} 
		}

		protected void Application_EndRequest()
		{
			MiniProfiler.Stop();
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters( GlobalFilters.Filters);
			RegisterRoutes( RouteTable.Routes);
		}
	}
}