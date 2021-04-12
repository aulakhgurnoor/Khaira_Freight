using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khaira_Freight.Models
{
    [MetadataType(typeof(Quote_Metadata))]
    public partial class quote
    {
        public List<SelectListItem> equipmentList { get; set; }
        public List<SelectListItem> provinceList { get; set; }
    }

    public class Quote_Metadata
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "Please enter ID")]
        public int id { get; set; }

        [DisplayName("Full Name")]
        [StringLength(50, ErrorMessage = "Full Name cannot be more than 50 characters")]
        [Required(ErrorMessage = "Please enter Full Name")]
        
        public string full_name { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Please enter Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Not a valid Email")]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30 characters")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "Please enter Phone number")]
        [DisplayName("Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [StringLength(15, ErrorMessage = "Phone number cannot be more than 15 characters")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Please enter Street Address")]
        [DisplayName("Street Address")]
        [StringLength(50, ErrorMessage = "Street address cannot be more than 50 characters")]

        public string street_address { get; set; }

        [Required(ErrorMessage = "Please enter City")]
        [DisplayName("City")]
        [StringLength(30, ErrorMessage = "City name cannot be more than 30 characters")]

        public string city { get; set; }

        [Required(ErrorMessage = "Please enter Postal Code")]
        [DisplayName("Postal Code")]
        [RegularExpression("^(?!.*[DFIOQU])[A-VXY][0-9][A-Z] ?[0-9][A-Z][0-9]$", ErrorMessage = "Enter correct format of Postal code")]
        [StringLength(10, ErrorMessage = "Postal Code cannot be more than 10 characters")]
        public string postal_code { get; set; }

        [Required(ErrorMessage = "Please enter Province")]
        [DisplayName("Province")]
        public string province { get; set; }


        [Required(ErrorMessage = "Please enter Pick up Location")]
        [DisplayName("Pick up Location")]
        [StringLength(50, ErrorMessage = "Pick up Location cannot be more than 50 characters")]
        public string pickup_location { get; set; }

        [DisplayName("Pick Up Date")]
        [Required(ErrorMessage = "Please enter PickUp Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [DataType(DataType.Date)]
        public System.DateTime pickup_date { get; set; }

        [Required(ErrorMessage = "Please enter Drop Location")]
        [DisplayName("Drop Location")]
        [StringLength(50, ErrorMessage = "Drop Location cannot be more than 50 characters")]
        public string drop_location { get; set; }

        [DisplayName("Drop Date")]
        [Required(ErrorMessage = "Please enter Drop Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [DataType(DataType.Date)]
        public System.DateTime drop_date { get; set; }

        [DisplayName("Equipment Type")]
        [Required(ErrorMessage = "Please select equipment type")]
        public string equipment_type { get; set; }

        [DisplayName("Weight")]
        [Required(ErrorMessage = "Please enter Weight")]
        public decimal weight { get; set; }

        [DisplayName("Commodity")]
        [Required(ErrorMessage = "Please enter Commodity")]
        [StringLength(50, ErrorMessage = "Commodity cannot be more than 50 characters")]
        public string commodity { get; set; }
    }

}