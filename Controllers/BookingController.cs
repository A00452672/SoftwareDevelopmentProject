using SoftwareDevelopmentProject.Models;
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
        public ActionResult PaymentView()
        {
           // ViewBag.id = id;
            return View("PaymentView");
        }

        [HttpPost]
        public ActionResult PayNow(FormCollection form)
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj != null && cookieObj["id"] != null)
            {
                try
                {
                    reservation reservation = new reservation();
                    reservation.user_id = Int32.Parse(cookieObj["id"]);
                    reservation.sport_id = Int32.Parse(form["sportId"]);
                    reservation.datetime = DateTime.Parse(form["datetime"]);
                    reservation.no_of_people = 1;
                    reservation.is_payment_done = "FALSE";
                    reservation.totalBookingCost = reservation.no_of_people * Int32.Parse(form["sportAmount"]);

                    using (reservationModel model = new reservationModel())
                    {
                        model.reservations.Add(reservation);
                        model.SaveChanges();
                    }
                    ModelState.Clear();
                    ViewBag.SuccessMessage = "Reservation Success";
                    ViewBag.reservationId = reservation.reservation_id;
                    ViewBag.totalAmount = reservation.totalBookingCost;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    ViewBag.duplicateMessage = "Reservation Failure";
                    return View("ReservationView");
                }
                return View("PaymentView");
            }
            return View("Login", new user());
        }
        public ActionResult BookingASport(string name)
        {
            ViewBag.sportName = name;
              using (SportModel sportmodel = new SportModel())
              { 
                  var sports = sportmodel.Sports.ToList();
                  foreach (var item in sports)
                  {
                      if (item.sport_name == name)
                      {
                        ViewBag.SportAmount = item.sport_rate;
                        ViewBag.SportId = item.sport_id;
                    }
                  }
               } 

            return View("ReservationView");
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
                invalid = false;
                invalidMessage = "Enter 15 digits as you have chosen American Express";
                }
                else if (!invalid && cp.cardType != "American Express" && cp.CardNumber.ToString().Length != 16)
                {
                invalid = false;
                invalidMessage = "Enter 16 digits as you have choosed Visa or Mastercard";
                }

                if ((!invalid && cp.cardType == "MasterCard" && !mc.IsMatch(cp.CardNumber.ToString())) || (cp.cardType == "Visa" && !v.IsMatch(cp.CardNumber.ToString())) || (cp.cardType == "American Express" && !aex.IsMatch(cp.CardNumber.ToString())))
                {
                invalid = false;
                invalidMessage = "Incorrect card number and type";
            }


            if (invalid)
            {
                ViewBag.ErrorMessage = invalidMessage;
                return View("PaymentView");
            }

            using (CustomerPaymentModel1 model = new CustomerPaymentModel1())
            {
                try
                {
                    cp.CardNumber = Int32.Parse(cp.cardType);
                    model.CustomerPayments.Add(cp);
                    //model.SaveChanges();

                    using (reservationModel resModel = new reservationModel())
                    {
                        foreach (var item in resModel.reservations.Where(x => x.reservation_id == cp.reservation_id))
                        {
                            item.is_payment_done = "TRUE";
                            resModel.reservations.Add(item);
                            //resModel.SaveChanges();
                        }
                    }
                    ViewBag.SuccessMessage = "Payment Successful";
                    ModelState.Clear();
                    return View("PaymentSucess");
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = "Payment Failed";
                    return View("PaymentView");
                }
            }
        }

    }
}
