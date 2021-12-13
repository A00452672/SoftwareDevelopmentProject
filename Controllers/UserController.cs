using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareDevelopmentProject.Models;

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
            User userModel = new User();
            return View(userModel);
        }
        [HttpPost]
        public ActionResult Registration(User userModel)
        {
            using (UserModel dbmodel = new UserModel())
            {
                if(dbmodel.Users.Any(x => x.username == userModel.username))
                {
                    ViewBag.duplicateMessage = "Username already taken!!!";
                    return View("Registration", userModel);
                }
                dbmodel.Users.Add(userModel);
                dbmodel.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Success";

            return View("Registration", new User());
        }

        [HttpGet]
        public ActionResult Login(int id = 0)
        {
            User userModel = new User();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Login(User userModel)
        {
            using (UserModel dbmodel = new UserModel())
            {
                if (dbmodel.Users.Any(x => x.username == userModel.username && x.password == userModel.password))
                {
                    if (userModel.username == "admin")
                        return RedirectToAction("AddASportView", "User");

                    return RedirectToAction("ViewAllSports", "User");
                }
            }
        
            ViewBag.InvalidCredentialsMessage = "Invalid credentials";
            return View("Login", new User());
        }


        [HttpGet]
        public ActionResult ViewAllSports()
        {
            using (SportModel sportModel = new SportModel())
            {
                return View(sportModel.Sports.ToList());
            }
                
        }

        [HttpGet]
        public ActionResult AddASportView()
        {
            if (errorMessage != null)
                ViewBag.duplicateMessage = errorMessage; 
            if (successMessage != null)
                ViewBag.SuccessMessage = successMessage;
            errorMessage = null;
            successMessage = null;
            return View("AddASportView", new Sport());
        }

        [HttpPost]
        public ActionResult AddASportView(Sport sport)
        {

            using (SportModel sportmodel = new SportModel())
            {
                if (sportmodel.Sports.Any(x => x.sport_id == sport.sport_id))
                {
                    ModelState.Clear();
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