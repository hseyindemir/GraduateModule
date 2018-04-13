using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraduateSoftware.Models;
using System.Diagnostics;

namespace GraduateSoftware.Controllers
{
    public class GraduateController : Controller
    {
        private GraduateModuleEntities db = new GraduateModuleEntities();
        // GET: Graduate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GraduateProfile()
        {
            //USER HAS TO LOG IN TO ACCESS HERE
            HttpCookie NewCookie = Request.Cookies["user"];
            if (NewCookie != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
            
            
        }
    }
}