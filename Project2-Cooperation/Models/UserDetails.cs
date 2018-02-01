using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2_Cooperation.Models
{
    public class UserDetails
    {
        [Key, ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        //First Name,Last Name, A-Z,a-z, 3-40 Characters
        //Phone 10 digits
        //Adress Letters 3-40 & Digits 1-3
        //Postal Code 5 Digits
        [Required(ErrorMessage = "Name is Required!")]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{3,40}$", ErrorMessage = "First Name is not valid.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        // [RegularExpression(@"^[a-zA-Z''-'\s]{3,40}$", ErrorMessage = "Last Name is not valid.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number Required!")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "City is Required!")]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{2,40}$", ErrorMessage = "City is not valid.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Adress is Required!")]
        [DataType(DataType.MultilineText)]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{2,40}\d{1,3}$", ErrorMessage = "Adress is not valid.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Zip code is Required!")]
        [DataType(DataType.PostalCode)]
        // [RegularExpression(@"^\(?([0-4]{3})\)?[-. ]?([0-4]{2})[-. ]?([0-4]{0})$", ErrorMessage = "Entered zip code is not valid.")]
        [Display(Name = "Postal Code")]
        public string Zip { get; set; }
    }
}
