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

        public ActionResult ViewReservations()
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj != null && cookieObj["id"] != null)
            {
                var user_id = Int32.Parse(cookieObj["id"]);
                Dictionary<int, string> sports = new Dictionary<int, string>();
                
                foreach(var item in new SportModel().Sports.ToList())
                {
                    sports.Add(item.sport_id, item.sport_name);
                }

                ViewBag.sports = sports;

                var user = new UserModel().users.Where(x => x.user_id == user_id).First();
                if (user.is_admin == 1)
                {
                    return View("Reservations", new reservationModel().reservations.ToList());
                }

                return View("ViewReservations", new reservationModel().reservations.Where(x => x.user_id == user_id).ToList() );
            }
            return View("Login", new user());
        }

        [HttpPost]
        public ActionResult PayReservation(FormCollection form)
        {
            HttpCookie cookieObj = Request.Cookies["SportUser"];
            if (cookieObj != null && cookieObj["id"] != null)
            {
                ViewBag.reservationId = form["reservation_id"];
                int resId = Int32.Parse(form["reservation_id"]);
                var res = new reservationModel().reservations.Where(x => x.reservation_id == resId).First();
                ViewBag.totalAmount = res.totalBookingCost;

                return View("PaymentView");
            }
            return View("Login", new user());
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
                    reservation.no_of_people = Int32.Parse(form["no_of_people"]);
                    reservation.is_payment_done = "FALSE";
                    reservation.totalBookingCost = reservation.no_of_people * Int32.Parse(form["sportAmount"]);

                    using (reservationModel model = new reservationModel())
                    {
                        model.reservations.Add(reservation);
                        model.SaveChanges();
                    }
                    ModelState.Clear();
                    ViewBag.reservationMade = "1";
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
            Boolean invalid = false;
            string invalidMessage = "";
            
            if( cp.cardType != "Master Card" && cp.cardType != "Visa" && cp.cardType != "American Express")
            {
                invalid = true;
                invalidMessage = "Please select valid card type";
            }
            else if (cp.cardType == "Master Card")
            {
                Regex masterCard = new Regex(@"^5[1-5][0-9]{14}$");
                if (!masterCard.IsMatch(cp.CardNumber))
                {
                    invalid = true;
                    invalidMessage = "Invalid Master Card";
                }
            }
            else if (cp.cardType == "Visa")
            {
                Regex visaCard = new Regex(@"^4[0-9]{15}$");
                if (!visaCard.IsMatch(cp.CardNumber))
                {
                    invalid = true;
                    invalidMessage = "Invalid VISA Card";
                }
            }
            else if (cp.cardType == "American Express")
            {
                Regex expressCard = new Regex(@"^3[4|7][0-9]{13}$");
                if (!expressCard.IsMatch(cp.CardNumber))
                {
                    invalid = true;
                    invalidMessage = "Invalid American Express Card";
                }
            }

            if (invalid)
            {
                ViewBag.reservationId = cp.reservation_id;
                ViewBag.ErrorMessage = invalidMessage;
                ViewBag.totalAmount = cp.totalamount;
                return View("PaymentView");
            }

            
            try
            {
                using (CustomerPaymentModel model = new CustomerPaymentModel())
                {
                    model.CustomerPayments.Add(cp);
                    model.SaveChanges();

                }
                ModelState.Clear();

                using (reservationModel resModel = new reservationModel())
                {
                    var item = resModel.reservations.Where(x => x.reservation_id == cp.reservation_id).First();
                    
                    item.is_payment_done = "TRUE";
                    resModel.SaveChanges();
                    
                }
                ViewBag.SuccessMessage = "Payment Successful";
                ModelState.Clear();
                return View("PaymentSucess");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.reservationId = cp.reservation_id;
                ViewBag.totalAmount = cp.totalamount;
                ViewBag.ErrorMessage = "Payment Failed";
                return View("PaymentView");
            }
        }

    }
}
