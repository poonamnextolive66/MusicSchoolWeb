using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicSchoolWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Music_Course()
        {
            return View();
        }
        public ActionResult Sub_Category()
        {
            return View();
        }

        public ActionResult UploadAudio()
        {
            return View();
        }
    }
}