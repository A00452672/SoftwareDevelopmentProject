namespace SoftwareDevelopmentProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public partial class CustomerPayment
    {
        public int payment_Id { get; set; }
        public int reservation_id { get; set; }

        [Display(Name = "Card Type")]
        public string cardType { get; set; }


        [Required(ErrorMessage = "Please Enter Credit Card Number")]
        [Display(Name = "Credit Card Number")]
        public int CardNumber { get; set; }


        [Display(Name = "Valid Until")]
        [Required(ErrorMessage = "Required Expiration Date")]
        [RegularExpression(@"^((0[1-9])|(1[0-2]))\/((201[6-9])|(202[0-9])|(203[0-1]))$", ErrorMessage = "Please Enter Valid MM/YYYY format")]
        public string ExpiryDate { get; set; }


        [Display(Name = "CVV")]
        [Required(ErrorMessage = "Required CVV")]
        [RegularExpression(@"^[\d]{3,3}$", ErrorMessage = "Please Enter Valid CVV")]
        public int cvv { get; set; }

        [Display(Name = "Card Holder Name")]
        [Required(ErrorMessage = "Please Enter the Name As in the card")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Name Should Contain Only Alphabets or Spaces")]
        public string nameOnTheCard { get; set; }
        [Display(Name = "Total Amount")]
        public int totalamount { get; set; }
    }
}
