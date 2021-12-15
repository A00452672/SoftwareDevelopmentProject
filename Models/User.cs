
namespace SoftwareDevelopmentProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class user
    {
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Full Name")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z ]*$", ErrorMessage = "Name can have only characters and spaces")]
        public string name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DisplayName("Email")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_\-.]*@[a-zA-Z0-9\-]+\.[a-zA-Z.]+$", ErrorMessage = "Enter valid email address")]
        public string email { get; set; }

        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter valid 10 digit phone number")]
        public string phone { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^.{8}.*$", ErrorMessage = "Password should have atleast 8 characters")]
        public string password { get; set; }
        public int user_id { get; set; }
        public int is_admin { get; set; }


        [DisplayName("Country")]
        [Required(ErrorMessage = "Country is required")]
        public string country { get; set; }

        [DisplayName("Zip Code")]
        [Required(ErrorMessage = "Zip Code is required")]
        public string zip_code { get; set; }
    }
}
