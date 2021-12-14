using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SoftwareDevelopmentProject.Controllers
{
    public class BookingController : Controller
    {
        [HttpPost]
        public ActionResult PaymentView(int id)
        {
            ViewBag.id = id;
            return View("PaymentView");
        }
        public ActionResult BookingASport(int id)
        {
            ViewBag.id = id;
            return View("PaymentView");
        }

        public ActionResult ValidationForPay(Models.CustomerPayment cp)
        {
            Regex mc = new Regex(@"^5[1-5][0-9]{14}|^(222[1-9]|22[3-9]\\d|2[3-6]\\d{2}|27[0-1]\\d|2720)[0-9]{12}$");
            Regex v = new Regex(@"^4[0-9]{12}(?:[0-9]{3})?$");
            Regex aex = new Regex(@"^3[47][0-9]{13}$");
            Boolean invalid = false;
            string invalidMessage = "";

                if (cp.cardType == "American Express" && cp.CardNumber.ToString().Length != 15)
                {
                invalid = true;
                invalidMessage = "Enter 15 digits as you have chosen American Express";
                }
                else if (!invalid && cp.cardType != "American Express" && cp.CardNumber.ToString().Length != 16)
                {
                invalid = true;
                invalidMessage = "Enter 16 digits as you have choosed Visa or Mastercard";
                }

                if ((!invalid && cp.cardType == "MasterCard" && !mc.IsMatch(cp.CardNumber.ToString())) || (cp.cardType == "Visa" && !v.IsMatch(cp.CardNumber.ToString())) || (cp.cardType == "American Express" && !aex.IsMatch(cp.CardNumber.ToString())))
                {
                invalid = true;
                invalidMessage = "Incorrect card number and type";
            }


            if (invalid)
            {
                ViewBag.ErrorMessage = invalidMessage;
                return View("PaymentView");
            }
            return View("PaymentSucess");
        }

    }
}
