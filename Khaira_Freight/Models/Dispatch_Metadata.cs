using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khaira_Freight.Models
{
    [MetadataType(typeof(Dispatch_Metadata))]
    public partial class dispatch
    {
        public List<SelectListItem> provinceList { get; set; }

        public List<SelectListItem> genderList { get; set; }
    }
    public class Dispatch_Metadata
    {
        [Required(ErrorMessage = "Please enter the Dispatcher ID")]
        [DisplayName("Dispatcher")]
        public int dispatch_id { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        [DisplayName("First Name")]
        [StringLength(40, ErrorMessage = "First name cannot be more than 40 characters")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [DisplayName("Last Name")]
        [StringLength(40, ErrorMessage = "Last name cannot be more than 40 characters")]
        public string last_name { get; set; }

        [Required(ErrorMessage = "Please enter Street Address")]
        [DisplayName("Street Address")]
        [StringLength(50, ErrorMessage = "Street address cannot be more than 50 characters")]
        public string address { get; set; }

        [Required(ErrorMessage = "Please enter Postal Code")]
        [DisplayName("Postal Code")]
        [RegularExpression("^(?!.*[DFIOQU])[A-VXY][0-9][A-Z] ?[0-9][A-Z][0-9]$", ErrorMessage = "Enter correct format of Postal code")]
        // [StringLength(10, ErrorMessage = "Postal code cannot be more than 10 characters")]
        public string postal_code { get; set; }

        [Required(ErrorMessage = "Please enter City")]
        [DisplayName("City")]
        [StringLength(25, ErrorMessage = "City name cannot be more than 25 characters")]
        public string city { get; set; }

        [Required(ErrorMessage = "Please enter Province")]
        [DisplayName("Province")]
        public string province { get; set; }

        [Required(ErrorMessage = "Please enter Phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        //[StringLength(15, ErrorMessage = "Phone number cannot be more than 15 characters")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Please enter Date of Birth")]
        [DisplayName("Date of Birth")]
        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public System.DateTime dob { get; set; }

        [Required(ErrorMessage = "Please enter Nationality")]
        [DisplayName("Nationality")]
        [StringLength(25, ErrorMessage = "Nationality cannot be more than 25 characters")]
        public string nationality { get; set; }

        [Required(ErrorMessage = "Please enter Gender")]
        [DisplayName("Gender")]
        public string gender { get; set; }
    }
}