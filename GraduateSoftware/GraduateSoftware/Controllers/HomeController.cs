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
using Newtonsoft.Json;

namespace GraduateSoftware.Controllers
{
    public class HomeController : Controller
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
                if (db.Admins.Any(x => x.AdminID == sessionCookie.Value))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("GraduateProfile", "Graduate");
                }
            }



            return View();
        }

        //HANDLES LOGIN

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            var hashedPass = sha256(password);
            var verifiedCaptcha = false;
             if (db.Graduates.Any(x => x.StudentID == username))
            {
                //reCaptcha SERVER SIDE CODE FOR FUTURE DEVELOPMENT

                //var response = Request["g-recaptcha-response"];
                //
                //const string secret = "6LdNt1sUAAAAAKE1GejVUSIdS-PlFVMj82aWq3y_";

                //var client = new WebClient();
                //var reply =
                //    client.DownloadString(
                //        string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                //    secret, response));

                //var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                //
                //if (captchaResponse.Success!="True")
                //{
                    
                //    FlashMessage.Danger("Confirm that you are not a robot.");
                //    return View();
                //}
                //else
                //{
                //    verifiedCaptcha = true;
                //    ViewBag.Message = "Valid";
                //}
                //IF USER EXISTS
                //SAVE ID PASS TO COOKIES AND LOGIN

                Graduate user = new Graduate();
                user = db.Graduates.Where(x => x.StudentID == username && x.StudentPassword == hashedPass).FirstOrDefault();
                if (db.AdminGraduateVerifications.SingleOrDefault(x => x.StudentID == user.StudentID).IsVerified == true)
                {
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
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    FlashMessage.Info("Please wait for your verification. You will be notified via email when you are verified.");
                    return RedirectToAction("GraduateProfile", "Graduate");

                }

            }
            else if (db.Admins.Any(x => x.AdminID == username))
            {
                Admin user = new Admin();
                user = db.Admins.Where(x => x.AdminID == username && x.AdminPassword == hashedPass).FirstOrDefault();

                //IF USERNAME AND PASSWORD IS CORRECT
                if (user != null)
                {
                    HttpCookie UserCookie = new HttpCookie("user", user.AdminID.ToString());
                    HttpCookie UserCookiePass = new HttpCookie("pass", user.AdminPassword.ToString());
                    UserCookie.Expires.AddMinutes(30);
                    UserCookiePass.Expires.AddMinutes(30);
                    HttpContext.Response.SetCookie(UserCookie);
                    HttpContext.Response.SetCookie(UserCookiePass);
                    FlashMessage.Confirmation("Successfully logged in as Admin.");
                    return RedirectToAction("Index", "Admin");

                }
                //ELSE RETURN TO LOGIN FORM
                else
                {

                    return View();
                }
            }
            else
            {
                FlashMessage.Danger("Enter a valid email and password.");
                return View();
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

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "VerificationID,StudentID,Password,GraduateName,GrauateSurname,GraduateEmail")] AdminGraduateVerification adminGraduateVerification, string password)
        {
            var hashedPass = sha256(password);
            if (ModelState.IsValid && password.Length>5)
            {
                if(db.Graduates.Any(x => x.StudentID == adminGraduateVerification.StudentID))
                {
                    FlashMessage.Danger("An account with the same Student ID already exists. Please contact the head of the department.");
                    return RedirectToAction("Register", "Home");
                }
                Debug.WriteLine("Creating User: " + adminGraduateVerification.GraduateEmail + hashedPass);
                Graduate graduate = new Graduate();
                graduate.StudentID = adminGraduateVerification.StudentID;
                graduate.GraduateName = adminGraduateVerification.GraduateName;
                graduate.GraduateLastName = adminGraduateVerification.GrauateSurname;
                graduate.GraduateMail = adminGraduateVerification.GraduateEmail;
                graduate.StudentPassword = hashedPass;
                db.Graduates.Add(graduate);

                AdminGraduateVerification graduateV = new AdminGraduateVerification();
                graduateV.IsVerified = false;
                graduateV.AdminID = db.Admins.SingleOrDefault().AdminID;
                graduateV.StudentID = adminGraduateVerification.StudentID;
                graduateV.GraduateName = adminGraduateVerification.GraduateName;
                graduateV.GrauateSurname = adminGraduateVerification.GrauateSurname;
                graduateV.GraduateEmail = adminGraduateVerification.GraduateEmail;
                db.AdminGraduateVerifications.Add(graduateV);

                db.SaveChanges();

                FlashMessage.Info("Successfully registered. Please wait for your verification. You will be notified via email in 7 days when you are verified.");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                FlashMessage.Danger("Failed to register user. Please check your information.");
                return View(adminGraduateVerification);
            }
        }

        public ActionResult Logout()
        {

            try
            {
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