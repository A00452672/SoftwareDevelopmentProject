using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareDevelopmentProject.Models;
using System.Text.RegularExpressions;

namespace SoftwareDevelopmentProject.Controllers
{
    public class UserController : Controller
    {
        public static string errorMessage = null;
        public static string successMessage = null;
        // GET: Use
        [HttpGet]
        public ActionResult Registration(int id = 0)
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj != null && cookieObj["id"]!=null)
            {
                using (UserModel dbmodel = new UserModel())
                {
                    var user_id = Int32.Parse(cookieObj["id"]);
                    var user = dbmodel.users.Where(x => x.user_id == user_id).First();
                    if (user.is_admin==1)
                    {
                        return RedirectToAction("AddASportView", "User");
                    }
                    else
                    {
                        return RedirectToAction("ViewAllSports", "User");
                    }
                }
                    
            }

            user userModel = new user();
            return View(userModel);
        }

        public ActionResult Logout()
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            var a = Response.Cookies;
            if (cookieObj != null )
            {
                Response.Cookies["SportUser"].Expires = DateTime.Now.AddMonths(-1);
            }

            return RedirectToAction("Login", "User");

        }

        [HttpPost]
        public ActionResult Registration(user userModel)
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj != null && cookieObj["id"] != null)
            {
                using (UserModel dbmodel = new UserModel())
                {
                    var user_id = Int32.Parse(cookieObj["id"]);
                    var user = dbmodel.users.Where(x => x.user_id == user_id).First();
                    if (user.is_admin == 1)
                    {
                        return RedirectToAction("AddASportView", "User");
                    }
                    else
                    {
                        return RedirectToAction("ViewAllSports", "User");
                    }
                }

            }

            using (UserModel dbmodel = new UserModel())
            {
                if (userModel.country.Equals("CANADA") && !Regex.IsMatch(userModel.zip_code, @"^([a-zA-Z][0-9]){3}$"))
                {
                    ViewBag.zip_error = "Invalid Zip Code for Canada";
                    return View("Registration", userModel);
                }
                if (userModel.country.Equals("USA") && !Regex.IsMatch(userModel.zip_code, @"^\d{5}(-\d{4})?$"))
                {
                    ViewBag.zip_error = "Invalid Zip Code for USA";
                    return View("Registration", userModel);
                }

                if (dbmodel.users.Any(x => x.email == userModel.email))
                {
                    ViewBag.duplicateMessage = "Email already used!!!";
                    return View("Registration", userModel);
                }
                
                dbmodel.users.Add(userModel);
                dbmodel.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Success";

            //return RedirectToAction("Login", "User");
            return View("Registration", new user());
        }

        [HttpGet]
        public ActionResult Login(int id = 0)
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj != null && cookieObj["id"] != null)
            {
                using (UserModel dbmodel = new UserModel())
                {
                    var user_id = Int32.Parse(cookieObj["id"]);
                    var user = dbmodel.users.Where(x => x.user_id == user_id).First();
                    if (user.is_admin == 1)
                    {
                        return RedirectToAction("AddASportView", "User");
                    }
                    else
                    {
                        return RedirectToAction("ViewAllSports", "User");
                    }
                }

            }

            user userModel = new user();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Login(user userModel)
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj != null && cookieObj["id"] != null)
            {
                using (UserModel dbmodel = new UserModel())
                {
                    var user_id = Int32.Parse(cookieObj["id"]);
                    var user = dbmodel.users.Where(x => x.user_id == user_id).First();
                    if (user.is_admin == 1)
                    {
                        return RedirectToAction("AddASportView", "User");
                    }
                    else
                    {
                        return RedirectToAction("ViewAllSports", "User");
                    }
                }

            }

            using (UserModel dbmodel = new UserModel())
            {
                foreach(var item in (dbmodel.users.Where(x => x.email == userModel.email && x.password == userModel.password)))
                {
                    HttpCookie cookie = new HttpCookie("SportUser");
                    cookie["id"] = item.user_id.ToString();
                    // This cookie will remain  for one month.
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    //ViewBag.user = "11";
                    // Add it to the current web response.
                    Response.Cookies.Add(cookie);
                    if (item.is_admin == 1)
                    {
                        return RedirectToAction("AddASportView", "User");
                    }
                    if (item.is_admin == 0)
                    {
                        return RedirectToAction("ViewAllSports", "User");
                    }
                }
            }
            ViewBag.InvalidCredentialsMessage = "Invalid credentials";
            return View("Login", new user());
        }


        [HttpGet]
        public ActionResult ViewAllSports()
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj == null || cookieObj["id"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.user = cookieObj["id"];
            Dictionary<int, string> currencies = new Dictionary<int, string>();
            foreach (var item in new CurrencyModel().currencies.ToList())
            {
                currencies.Add(item.currency_id, item.currency_name);
            }
            ViewBag.currencies = currencies;

            using (SportModel sportModel = new SportModel())
            {
                return View(sportModel.Sports.ToList());
            }
                
        }

        [HttpGet]
        public ActionResult AddASportView()
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj == null || cookieObj["id"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                using (UserModel dbmodel = new UserModel())
                {
                    var user_id = Int32.Parse(cookieObj["id"]);
                    var user = dbmodel.users.Where(x => x.user_id == user_id).First();
                    if (user.is_admin == 0)
                    {
                        return RedirectToAction("ViewAllSports", "User");
                    }
                }
            }

            ViewBag.user = cookieObj["id"];

            if (errorMessage != null)
                ViewBag.duplicateMessage = errorMessage;
            if (successMessage != null)
                ViewBag.SuccessMessage = successMessage;
            errorMessage = null;
            successMessage = null;
            Dictionary<int, string> currencies = new Dictionary<int, string>();
            foreach (var item in new CurrencyModel().currencies.ToList())
            {
                currencies.Add(item.currency_id, item.currency_name);
            }
            ViewBag.currencies = currencies;
            return View("AddASportView", new Sport());
        }

        [HttpPost]
        public ActionResult AddASportView(Sport sport)
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj == null || cookieObj["id"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                using (UserModel dbmodel = new UserModel())
                {
                    var user_id = Int32.Parse(cookieObj["id"]);
                    var user = dbmodel.users.Where(x => x.user_id == user_id).First();
                    if (user.is_admin == 0)
                    {
                        return RedirectToAction("ViewAllSports", "User");
                    }
                }
            }

            ViewBag.user = cookieObj["id"];

            sport.sport_id = 1;
            using (SportModel sportmodel = new SportModel())
            {
                if (sportmodel.Sports.Any(x => x.sport_name == sport.sport_name))
                {
                    //ModelState.Clear();
                    errorMessage = "sport already exists!!!";
                    return RedirectToAction("AddASportView", "User");

                }
                try {
                sportmodel.Sports.Add(sport);
                sportmodel.SaveChanges();
            }
                catch(Exception e)
                {
                    ModelState.Clear();
                    return RedirectToAction("Error", "User");

                }

            }
            ModelState.Clear();
            successMessage  = "Sport created successfully";
       

            return RedirectToAction("AddASportView", "User");
        }


        [HttpGet]
        public ActionResult Error()
        {
            return View("Error");
        }
    }
}