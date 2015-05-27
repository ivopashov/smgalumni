using System;
using System.Web.Mvc;

namespace SmgAlumni.App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}