using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NCForm.Controllers
{
    public class ErrorhandlerController : Controller
    {
        // GET: Errorhandler
        public ActionResult Index()
        {
            Response.StatusCode = 400;
            return View();
        }
        public ActionResult NotFound()
        {
            Response.StatusCode = 400;
            return View();
        }
    }
}