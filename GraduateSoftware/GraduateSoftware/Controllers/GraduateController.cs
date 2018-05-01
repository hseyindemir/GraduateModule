using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraduateSoftware.Models;
using System.Diagnostics;
using System.Net;
using System.Data.Entity;
using Vereyon.Web;

namespace GraduateSoftware.Controllers
{
    public class GraduateController : Controller
    {
        private GraduateModuleEntities db = new GraduateModuleEntities();

        //USER NEEDS TO LOGIN AGAIN IF HE STAYS INACTIVE FOR 30 MINUTES
        //ONACTIONEXECUTED OVERRIDE MUST BE IN EVERY CONTROLLER

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.RawUrl != "/Home/Logout")
            {

                var sessionCookie = Request.Cookies["user"];
                var sessionCookie2 = Request.Cookies["pass"];

                if (sessionCookie != null && sessionCookie2 != null)
                {
                    sessionCookie.Expires = DateTime.Now.AddMinutes(30);
                    sessionCookie2.Expires = DateTime.Now.AddMinutes(30);
                    Response.SetCookie(sessionCookie);
                    Response.SetCookie(sessionCookie2);
                }

            }



            base.OnActionExecuted(filterContext);
        }

        // GET: Graduate
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GraduateProfile()
        {
            //USER HAS TO LOG IN TO ACCESS HERE
            HttpCookie NewCookie = Request.Cookies["user"];
            if (Request.Cookies["user"] != null && Request.Cookies["pass"] != null)
            {
                GraduateModuleEntities db = new GraduateModuleEntities();
                string val = "";
                string valPass = "";

                val = Request.Cookies["user"].Value;
                valPass = Request.Cookies["pass"].Value;

                ////Same Session+Different User Login Correction
                //Request.Cookies["user"].Expires = DateTime.Now.AddDays(-1);

                var graduates = db.Graduates.Where(x => x.StudentID == val && x.StudentPassword == valPass);
                return View(graduates.ToList());

            }
            else
            {
                return RedirectToAction("Logout", "Home");
            }



        }

        public ActionResult Edit(string ID)
        {
            //USER CANNOT SEE THE EDIT PAGE OF OTHER USERS NOW
            if (Request.Cookies["user"] != null && Request.Cookies["pass"] != null)
            {
                Graduate graduate = db.Graduates.Find(ID);
                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else if (graduate == null)
                {
                    return HttpNotFound();
                }
                else if (Request.Cookies["user"].Value == graduate.StudentID && Request.Cookies["pass"].Value == graduate.StudentPassword)
                {

                    //Pump->WorkAreaList

                    GraduateModel graduateModel = new GraduateModel();
                    graduateModel.StudentID = graduate.StudentID;
                    graduateModel.GraduateLastName = graduate.GraduateLastName;
                    graduateModel.GraduateName = graduate.GraduateName;
                    graduateModel.GraduateMail = graduate.GraduateMail;
                    graduateModel.GraduateCompany = graduate.GraduateCompany;
                    graduateModel.GraduateYear = graduate.GraduateYear;
                    graduateModel.GraduateTitle = graduate.GraduateTitle;
                    graduateModel.GraduatePhone = graduate.GraduatePhone;
                    graduateModel.StudentPassword = graduate.StudentPassword;
                    graduateModel.Alanlar = new SelectList(db.WorkAreas, "WAID", "WorkAreaName");


                    FlashMessage.Confirmation("Update successful.");
                    return View(graduateModel);

                }
                else
                {
                    return RedirectToAction("GraduateProfile", "Graduate");
                }
            }
            else
            {
                return RedirectToAction("Logout", "Home");
            }
        }
        [HttpPost]
        public ActionResult Edit(GraduateModel GraduateModify)
        {

            if (ModelState.IsValid)
            {
                Graduate graduateModel = new Graduate();
                graduateModel.StudentID = GraduateModify.StudentID;
                graduateModel.GraduateLastName = GraduateModify.GraduateLastName;
                graduateModel.GraduateName = GraduateModify.GraduateName;
                graduateModel.GraduateMail = GraduateModify.GraduateMail;
                graduateModel.GraduateCompany = GraduateModify.GraduateCompany;
                graduateModel.GraduateYear = GraduateModify.GraduateYear;
                graduateModel.GraduateTitle = GraduateModify.GraduateTitle;
                graduateModel.GraduatePhone = GraduateModify.GraduatePhone;
                graduateModel.StudentPassword = GraduateModify.StudentPassword;
                graduateModel.StudentPassword = Request.Cookies["pass"].Value;
                graduateModel.WorkAreaID = GraduateModify.WorkAreaID;
                db.Entry(graduateModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GraduateProfile", "Graduate");
            }
            return View();
        }

    }
}