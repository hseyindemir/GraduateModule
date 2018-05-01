using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using GraduateSoftware.Models;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using Vereyon.Web;

namespace GraduateSoftware.Controllers
{
    public class HomeController : Controller
    {

        private GraduateModuleEntities db = new GraduateModuleEntities();

        //USER NEEDS TO LOGIN AGAIN IF HE STAYS INACTIVE FOR 30 MINUTES
        //ONACTIONEXECUTED OVERRIDE MUST BE IN EVERY CONTROLLER

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if ( filterContext.HttpContext.Request.RawUrl != "/Home/Logout")
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

        //CREATES SHA256

        static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var sessionCookie = Request.Cookies["user"];
            var sessionCookie2 = Request.Cookies["pass"];
            if (sessionCookie != null && sessionCookie2 != null)
            {
                return RedirectToAction("GraduateProfile", "Graduate");
            }
            
            return View();
        }
        
        //HANDLES LOGIN

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            var hashedPass = sha256(password);
            if (!db.Graduates.Any(x => x.StudentID == username))
            {
                //CREATE USER IF THE USER DOES NOT EXIST AND REDIRECT TO PROFILE
                
                Debug.WriteLine("Creating User: " + username + password);
                Graduate graduate = new Graduate();
                graduate.StudentID = username;
                graduate.StudentPassword = hashedPass;
                db.Graduates.Add(graduate);
                db.SaveChanges();
                //setting cookies
                HttpCookie UserCookie = new HttpCookie("user", graduate.StudentID.ToString());
                HttpCookie UserCookiePass = new HttpCookie("pass", graduate.StudentPassword.ToString());
                UserCookie.Expires.AddMinutes(30);
                UserCookiePass.Expires.AddMinutes(30);
                HttpContext.Response.SetCookie(UserCookie);
                HttpContext.Response.SetCookie(UserCookiePass);

                return RedirectToAction("Edit", "Graduate", new { ID= username});
            }
            else
            {

                

                //IF USER EXISTS
                //SAVE ID TO COOKIES AND LOGIN

                Graduate user = new Graduate();
                user = db.Graduates.Where(x => x.StudentID == username && x.StudentPassword == hashedPass).FirstOrDefault();

                //IF USERNAME AND PASSWORD IS CORRECT
                if (user != null)
                {
                    HttpCookie UserCookie = new HttpCookie("user", user.StudentID.ToString());
                    HttpCookie UserCookiePass = new HttpCookie("pass", user.StudentPassword.ToString());
                    UserCookie.Expires.AddMinutes(30);
                    UserCookiePass.Expires.AddMinutes(30);
                    HttpContext.Response.SetCookie(UserCookie);
                    HttpContext.Response.SetCookie(UserCookiePass);
                    FlashMessage.Confirmation("Successfully logged in.");
                    return RedirectToAction("GraduateProfile", "Graduate"); 
                    
                }
                //ELSE RETURN TO LOGIN FORM
                else {
                    return View();
                }

            }
            

        }

        public ActionResult WorkAreaGraph()
        {

            //GETS THE PERCENTAGE OF GRADUATES PER WORK AREA
            var graduateCountEducation = db.Graduates.Count(x => x.WorkAreaID == 1);
            var graduateCountSoftware = db.Graduates.Count(x => x.WorkAreaID == 2);
            var graduateCountOthers = db.Graduates.Count(x => x.WorkAreaID == 3);
            var countTotal = graduateCountEducation + graduateCountSoftware + graduateCountOthers;
            var educationPercentage = ((double)graduateCountEducation / (double)countTotal) * 100;
            var softwarePercentage = ((double)graduateCountSoftware / (double)countTotal) * 100;
            var othersPercentage = ((double)graduateCountOthers / (double)countTotal) * 100;

            ViewBag.educationPercentage = educationPercentage.ToString("0.00"); ;
            ViewBag.softwarePercentage = softwarePercentage.ToString("0.00"); ;
            ViewBag.othersPercentage = othersPercentage.ToString("0.00"); ;

            return View();


        }

        public ActionResult Logout()
        {
            
            try { 
                var Cookie1 = Request.Cookies["user"];
                var Cookie2 = Request.Cookies["pass"];
                
                    Cookie1.Expires = DateTime.Now.AddDays(-1);
                    Cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.SetCookie(Cookie1);
                    Response.SetCookie(Cookie2);

                Debug.WriteLine("LOGGED OUT");
                FlashMessage.Confirmation("Successfully logged out.");
                return RedirectToAction("Index", "Home");
            }

            catch
            {
                return RedirectToAction("Index", "Home");
            }



        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}