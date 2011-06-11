using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetaPocoWebTest.Poco;
using PetaPocoWebTest.ViewModels;
using PetaPocoWebTest.Database;

namespace PetaPocoWebTest.Controllers
{
    public class ArticleController : BaseController
    {
		public ArticleController()
		{
			ThisControllerLivesInWebsiteSection( "article");
		}

        //
        // GET: /Article/
        public ActionResult Index()
        {
			ArticlesViewModel viewModel =
				new ArticlesViewModel()
				{
					Articles = _articles.RetrieveAll(),
					Authors = null
				};

            return View( viewModel);
        }

        //
        // GET: /Article/Details/5
        public ActionResult Details(int articleId)
        {
			var article = _articles.RetrieveById( articleId);
            return View( article);
        }

        //
        // GET: /Article/Create
        public ActionResult Create()
        {
			var viewModel = new ArticleEditViewModel()
			{
				Article = new Article(),
				Authors = _authors.RetrieveAll(),
			};

            return View( viewModel);
        } 

        //
        // POST: /Article/Create
        [HttpPost]
        public ActionResult Create( Article article)
        {
            try
            {
                _articles.Insert( article);
                return RedirectToAction("Index");
            }
            catch( Exception exception)
            {
				ViewBag.ErrorMessage = exception.Message;
				var viewModel = new ArticleEditViewModel()
				{
					Article = article,
					Authors = _authors.RetrieveAll(),
				};

                return View( viewModel);
            }
        }
        
        //
        // GET: /Article/Edit/5
        public ActionResult Edit(int articleId)
        {
			var viewModel = new ArticleEditViewModel()
			{
				Article = _articles.RetrieveById( articleId),
				Authors = _authors.RetrieveAll(),
			};

            return View( viewModel);
        }

        //
        // POST: /Article/Edit/5
        [HttpPost]
        public ActionResult Edit( Article article)
        {
            try
            {
                _articles.Update( article);
                return RedirectToAction("Index");
            }
            catch( Exception exception)
            {
				ViewBag.ErrorMessage = exception.Message;
				var viewModel = new ArticleEditViewModel()
				{
					Article = article,
					Authors = _authors.RetrieveAll(),
				};

                return View( viewModel);
            }
        }

        //
        // GET: /Article/Delete/5
        public ActionResult Delete( int articleId)
        {
			var article = _articles.RetrieveById( articleId);

            return View( article);
        }

        //
        // POST: /Article/Delete/5
        [HttpPost]
        public ActionResult Delete( int articleId, FormCollection formCollection/*dummy parameter for function signature difference*/)
        {
            try
            {
                _articles.Delete( articleId);
                return RedirectToAction("Index");
            }
            catch( Exception exception)
            {
				ViewBag.ErrorMessage = exception.Message;
                return View( _articles.RetrieveById( articleId));
            }
        }
    }
}
