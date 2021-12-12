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
        // GET: User
        public ActionResult Registration(int id=0)
        {
            User userModel = new User();
            return View(userModel);
        }
    }
}