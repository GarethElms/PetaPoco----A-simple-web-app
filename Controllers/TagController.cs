using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetaPocoWebTest.Poco;
using PetaPocoWebTest.ViewModels;
using PetaPocoWebTest.Database;
using System.Text.RegularExpressions;

namespace PetaPocoWebTest.Controllers
{
    public class TagController : BaseController
    {
		public TagController()
		{
			ThisControllerLivesInWebsiteSection( "tag");
		}

        //
        // GET: /tag/
        public ActionResult Index()
        {
			return View( _tags.RetrieveAll());
        }

        //
        // GET: /tag/Details/tagName
        public ActionResult Details( string tagName)
        {
			var tag = _tags.RetrieveByTag( tagName);
            return View( tag);
        }

        //
        // GET: /tag/Create
        public ActionResult Create()
        {
            return View( new Tag());
        } 

        //
        // POST: /tag/Create
        [HttpPost]
        public ActionResult Create( Tag tag)
        {
            try
            {
                _tags.Insert( tag);
                return RedirectToAction("Index");
            }
            catch( Exception exception)
            {
				ViewBag.ErrorMessage = exception.Message;
                return View( tag);
            }
        }
        
        //
        // GET: /tag/Edit/5
        public ActionResult Edit( string tagName)
        {
            return View( _tags.RetrieveByTag( tagName));
        }

        //
        // POST: /tag/Edit/5
        [HttpPost]
        public ActionResult Edit( Tag tag)
        {
            try
            {
                _tags.Update( tag);
                return RedirectToAction("Index");
            }
            catch( Exception exception)
            {
				ViewBag.ErrorMessage = exception.Message;
                return View( tag);
            }
        }

        //
        // GET: /tag/Delete/5
        public ActionResult Delete( string tagName)
        {
            return View( _tags.RetrieveByTag( tagName));
        }

        //
        // POST: /tag/Delete/5
        [HttpPost]
        public ActionResult Delete( int tagId)
        {
            try
            {
                _tags.Delete( tagId);
                return RedirectToAction("Index");
            }
            catch( Exception exception)
            {
				ViewBag.ErrorMessage = exception.Message;
                return View( _tags.RetrieveById( tagId));
            }
        }
    }
}
