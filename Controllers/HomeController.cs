using SoftwareDevelopmentProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwareDevelopmentProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
           // ViewBag.Message = "Your application about page.";

            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }
        public ActionResult FaqView()
        {
            ViewBag.Message = "Your application contact page.";
            return View(new k_singhEntities1().faqs.ToList());
        }


    }
}