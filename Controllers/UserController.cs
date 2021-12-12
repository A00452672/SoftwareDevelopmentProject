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
                    ViewBag.duplicateMessage = "damnnnnnn!! Try a different username";
                    return View("Registration", userModel);
                }
                dbmodel.Users.Add(userModel);
                dbmodel.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Success";

            return View("Registration", new User());
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}