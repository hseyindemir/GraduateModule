using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GraduateSoftware.Models;
using Vereyon.Web;
using System.Diagnostics;
using System.Net.Mail;

namespace GraduateSoftware.Controllers
{
    public class AdminController : Controller
    {
        private GraduateModuleEntities db = new GraduateModuleEntities();

        // GET: Admin
        public ActionResult Index()
        {
            var adminGraduateVerifications = db.AdminGraduateVerifications.Where(x => x.IsVerified == false);
            
                foreach (var item in adminGraduateVerifications.ToList())
                {
                    Debug.WriteLine(item.StudentID);
                }
                
                return View(adminGraduateVerifications.ToList());
           
        }

        [HttpPost]
        public ActionResult Index(string verify, string studentid)
        {
            try
            {
                var user = Request.Cookies["user"].Value;
                var pass = Request.Cookies["pass"].Value;
                if (ModelState.IsValid)
                {
                    if (verify == "Accept")
                    {
                        var admin = db.Admins.Where(x => x.AdminID == user && x.AdminPassword == pass).FirstOrDefault();
                        var newVerification = db.AdminGraduateVerifications.SingleOrDefault(x => x.StudentID == studentid && x.AdminID == admin.AdminID);
                        newVerification.IsVerified = true;
                        db.Entry(newVerification).State = EntityState.Modified;
                        db.SaveChanges();

                        //SEND EMAIL

                        //SmtpClient client = new SmtpClient("some.server.com");
                        //client.Credentials = new NetworkCredential("username", "password");
                        //MailMessage mailMessage = new MailMessage();
                        //mailMessage.From = "asdasd@gmail.com";
                        //mailMessage.To.Add("someone.else@somewhere-else.com");
                        //mailMessage.Subject = "Hello There";
                        //mailMessage.Body = "Hello my friend!";
                        //client.Send(mailMessage);

                        return new EmptyResult();

                    }
                    else if (verify == "Reject")
                    {
                        var admin = db.Admins.Where(x => x.AdminID == user && x.AdminPassword == pass).FirstOrDefault();
                        var newVerification = db.AdminGraduateVerifications.SingleOrDefault(x => x.StudentID == studentid && x.AdminID == admin.AdminID);
                        //newVerification.IsVerified = false;
                        //db.Entry(newVerification).State = EntityState.Modified;
                        db.AdminGraduateVerifications.Remove(newVerification);
                        var deletedUser = db.Graduates.SingleOrDefault(x => x.StudentID == studentid);
                        db.Graduates.Remove(deletedUser);
                        db.SaveChanges();
                        return new EmptyResult();
                    }

                }

                return new EmptyResult();
            }
            catch
            {
                FlashMessage.Danger("Error.");
                return View();
            }
        }

        public ActionResult AdminGraduateDetails()
        {
            
            var graduates = db.Graduates;
            var adminGraduate = db.AdminGraduateVerifications.Where(x => x.IsVerified == true);

            List<Graduate> graduateList = new List<Graduate>();
            foreach (var item in adminGraduate.ToList())
            {
                var newGraduate=db.Graduates.FirstOrDefault(x => x.StudentID == item.StudentID);
                graduateList.Add(newGraduate);
            }
            


            return View(graduateList);
            
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminGraduateVerification adminGraduateVerification = db.AdminGraduateVerifications.Find(id);
            if (adminGraduateVerification == null)
            {
                return HttpNotFound();
            }
            return View(adminGraduateVerification);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.Admins, "AdminID", "AdminName");
            ViewBag.StudentID = new SelectList(db.AdminGraduateVerifications.Where(x => x.IsVerified == false), "StudentID", "StudentID");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VerificationID,IsVerified,AdminID,StudentID")] AdminGraduateVerification adminGraduateVerification)
        {
            var user = Request.Cookies["user"].Value;
            var pass = Request.Cookies["pass"].Value;
            if (ModelState.IsValid && !db.AdminGraduateVerifications.Any(x => x.StudentID == adminGraduateVerification.StudentID))
            {

                var admin = db.Admins.Where(x => x.AdminID == user && x.AdminPassword == pass).FirstOrDefault();
                adminGraduateVerification.AdminID = admin.AdminID;
                adminGraduateVerification.IsVerified = true;
                db.AdminGraduateVerifications.Add(adminGraduateVerification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.AdminID = new SelectList(db.Admins, "AdminID", "AdminName", adminGraduateVerification.AdminID);
                ViewBag.StudentID = new SelectList(db.Graduates, "StudentID", "GraduateName", adminGraduateVerification.StudentID);
                FlashMessage.Danger("You have already verified or denied this user.");
                return View(adminGraduateVerification);
            }

        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminGraduateVerification adminGraduateVerification = db.AdminGraduateVerifications.Find(id);
            if (adminGraduateVerification == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.Admins, "AdminID", "AdminName", adminGraduateVerification.AdminID);
            ViewBag.StudentID = new SelectList(db.Graduates, "StudentID", "GraduateName", adminGraduateVerification.StudentID);
            return View(adminGraduateVerification);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VerificationID,IsVerified,AdminID,StudentID")] AdminGraduateVerification adminGraduateVerification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminGraduateVerification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.Admins, "AdminID", "AdminName", adminGraduateVerification.AdminID);
            ViewBag.StudentID = new SelectList(db.Graduates, "StudentID", "GraduateName", adminGraduateVerification.StudentID);
            return View(adminGraduateVerification);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminGraduateVerification adminGraduateVerification = db.AdminGraduateVerifications.Find(id);
            if (adminGraduateVerification == null)
            {
                return HttpNotFound();
            }
            return View(adminGraduateVerification);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminGraduateVerification adminGraduateVerification = db.AdminGraduateVerifications.Find(id);
            db.AdminGraduateVerifications.Remove(adminGraduateVerification);
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
