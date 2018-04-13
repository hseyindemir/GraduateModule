using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GraduateSoftware.Models;

namespace GraduateSoftware.Controllers
{
    public class GraduatesController : Controller
    {
        private GraduateModuleEntities db = new GraduateModuleEntities();

        // GET: Graduates
        public ActionResult Index()
        {
            GraduateModuleEntities db = new GraduateModuleEntities();
            string val = "";
            if (Request.Cookies["user"] != null)
            {
                val = Request.Cookies["user"].Value;
            }

            var graduates = db.Graduates.Where(x => x.StudentID == val);
            return View(graduates.ToList());
        }

        // GET: Graduates/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Graduate graduate = db.Graduates.Find(id);
            if (graduate == null)
            {
                return HttpNotFound();
            }
            return View(graduate);
        }

        // GET: Graduates/Create
        public ActionResult Create()
        {
            ViewBag.WorkAreaID = new SelectList(db.WorkAreas, "WAID", "WorkAreaName");
            ViewBag.WorkAreaDetailID = new SelectList(db.WorkAreaDetails, "WADID", "WorkAreaDetailName");
            return View();
        }

        // POST: Graduates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,StudentPassword,GraduateName,GraduateLastName,GraduateYear,WorkAreaID,WorkAreaDetailID,GraduateCompany,GraduateTitle,GraduateMail,GraduatePhone")] Graduate graduate)
        {
            if (ModelState.IsValid)
            {
                db.Graduates.Add(graduate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorkAreaID = new SelectList(db.WorkAreas, "WAID", "WorkAreaName", graduate.WorkAreaID);
            ViewBag.WorkAreaDetailID = new SelectList(db.WorkAreaDetails, "WADID", "WorkAreaDetailName", graduate.WorkAreaDetailID);
            return View(graduate);
        }

        // GET: Graduates/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Graduate graduate = db.Graduates.Find(id);
            if (graduate == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkAreaID = new SelectList(db.WorkAreas, "WAID", "WorkAreaName", graduate.WorkAreaID);
            ViewBag.WorkAreaDetailID = new SelectList(db.WorkAreaDetails, "WADID", "WorkAreaDetailName", graduate.WorkAreaDetailID);
            return View(graduate);
        }

        // POST: Graduates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,StudentPassword,GraduateName,GraduateLastName,GraduateYear,WorkAreaID,WorkAreaDetailID,GraduateCompany,GraduateTitle,GraduateMail,GraduatePhone")] Graduate graduate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(graduate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkAreaID = new SelectList(db.WorkAreas, "WAID", "WorkAreaName", graduate.WorkAreaID);
            ViewBag.WorkAreaDetailID = new SelectList(db.WorkAreaDetails, "WADID", "WorkAreaDetailName", graduate.WorkAreaDetailID);
            return View(graduate);
        }

        // GET: Graduates/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Graduate graduate = db.Graduates.Find(id);
            if (graduate == null)
            {
                return HttpNotFound();
            }
            return View(graduate);
        }

        // POST: Graduates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Graduate graduate = db.Graduates.Find(id);
            db.Graduates.Remove(graduate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
