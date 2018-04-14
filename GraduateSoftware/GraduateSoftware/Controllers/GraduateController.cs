using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraduateSoftware.Models;
using System.Diagnostics;
using System.Net;
using System.Data.Entity;

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
                GraduateModuleEntities db = new GraduateModuleEntities();
                string val = "";
                string valPass = "";
                if (Request.Cookies["user"] != null && Request.Cookies["pass"] != null)
                {
                    val = Request.Cookies["user"].Value;
                    valPass = Request.Cookies["pass"].Value;
                }

                var graduates = db.Graduates.Where(x => x.StudentID == val && x.StudentPassword==valPass);
                return View(graduates.ToList());

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

    

        }

        public ActionResult Edit(string ID)
        {

            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Graduate graduate = db.Graduates.Find(ID);
            if (graduate == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkAreaID = new SelectList(db.WorkAreas, "WAID", "WorkAreaName", graduate.WorkAreaID);
            ViewBag.WorkAreaDetailID = new SelectList(db.WorkAreaDetails, "WADID", "WorkAreaDetailName", graduate.WorkAreaDetailID);
            return View(graduate);
            
        }
        [HttpPost]
        public ActionResult Edit(Graduate GraduateModify)
        {

            if (ModelState.IsValid)
            {
                db.Entry(GraduateModify).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GraduateProfile", "Graduate");
            }
            return View();
        }

    }
}