using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetaPocoWebTest.Poco;
using PetaPocoWebTest.Database;

namespace PetaPocoWebTest.Controllers
{
    public class AuthorController : BaseController
    {
		public AuthorController()
		{
			ThisControllerLivesInWebsiteSection( "author");
		}

        //
        // GET: /Author/
        public ActionResult Index()
        {
			List<Author> Authors = _authors.RetrieveAll();
            return View( Authors);
        }

        //
        // GET: /Author/Details/5
        public ActionResult Details( int authorId)
        {
			var author = _authors.RetrieveById( authorId);
            return View( author);
        }

        //
        // GET: /Author/Create
        public ActionResult Create()
        {
            return View( new Author());
        } 

        //
        // POST: /Author/Create
        [HttpPost]
        public ActionResult Create( Author author)
        {
            try
            {
                _authors.Insert( author);
                return RedirectToAction("Index");
            }
            catch( Exception exception)
            {
				ViewBag.ErrorMessage = exception.Message;
                return View();
            }
        }
        
        //
        // GET: /Author/Edit/5
        public ActionResult Edit(int authorId)
        {
			var author = _authors.RetrieveById( authorId);
            return View( author);
        }

        //
        // POST: /Author/Edit/5
        [HttpPost]
        public ActionResult Edit( Author author)
        {
            try
            {
                _authors.Update( author);
                return RedirectToAction("Index");
            }
            catch( Exception exception)
            {
				ViewBag.ErrorMessage = exception.Message;
                return View();
            }
        }

        //
        // GET: /Author/Delete/5
        public ActionResult Delete( int authorId)
        {
			var author = _authors.RetrieveById( authorId);

            return View( author);
        }

        //
        // POST: /Author/Delete/5
        [HttpPost]
        public ActionResult Delete( int authorId, FormCollection formCollection)
        {
            try
            {
                _authors.Delete( authorId);
                return RedirectToAction("Index");
            }
            catch( Exception exception)
            {
				ViewBag.ErrorMessage = exception.Message;
                return View();
            }
        }
    }
}
