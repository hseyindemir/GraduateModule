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

namespace GraduateSoftware.Controllers
{
    public class HomeController : Controller
    {
        private GraduateModuleEntities db = new GraduateModuleEntities();

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
                return RedirectToAction("GraduateProfile", "Graduate");
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
                    UserCookie.Expires.AddDays(7);
                    UserCookiePass.Expires.AddDays(7);
                    HttpContext.Response.SetCookie(UserCookie);
                    HttpContext.Response.SetCookie(UserCookiePass);
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