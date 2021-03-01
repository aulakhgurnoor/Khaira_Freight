using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Khaira_Freight.Models
{
    [MetadataType(typeof(Truck_Metadata))]
    public partial class truck
    {
       // public List<SelectListItem> departmentList { get; set; }

        
    }
    public class Truck_Metadata
    {
        [Required(ErrorMessage = "Please enter Unit Number")]
        [DisplayName("Unit Number")]
        public int unit_number { get; set; }

        [Required(ErrorMessage = "Please enter Plate Number")]
        [DisplayName("Plate Number")]
        [StringLength(10, ErrorMessage = "Plate Number cannot be more than 10 characters")]
        public string plate_number { get; set; }

        [Required(ErrorMessage = "Please enter Registeration Info")]
        [DisplayName("Registeration")]
        [StringLength(10, ErrorMessage = "Registeration Info cannot be more than 10 characters")]
        public string registration { get; set; }

        [Required(ErrorMessage = "Please enter VIN Number")]
        [DisplayName("VIN Number")]
        [StringLength(25, ErrorMessage = "VIN Number cannot be more than 25 characters")]
        public string vin_number { get; set; }

        [Required(ErrorMessage = "Please enter Make")]
        [DisplayName("Make")]
        [StringLength(20, ErrorMessage = "Make cannot be more than 20 characters")]
        public string make { get; set; }

        [Required(ErrorMessage = "Please enter Year")]
        [DisplayName("Year")]
        public int year { get; set; }

        [Required(ErrorMessage = "Please enter Weight")]
        [DisplayName("Weight")]
        public decimal weight { get; set; }

        [Required(ErrorMessage = "Please enter Status")]
        [DisplayName("Status")]
        [StringLength(20, ErrorMessage = "Status cannot be more than 20 characters")]
        public string status { get; set; }

    
        [DisplayName("Owner Company")]
        [StringLength(100, ErrorMessage = "Owner Company cannot be more than 100 characters")]
        public string owner_company { get; set; }

        [DisplayName("Driver Name")]
        [StringLength(50, ErrorMessage = "Driver Name cannot be more than 50 characters")]
        public string driver { get; set; }

        [DisplayName("Last Location")]
        [StringLength(50, ErrorMessage = "Last Location cannot be more than 50 characters")]
        public string last_location { get; set; }

        [DisplayName("Last Trip")]
        [StringLength(50, ErrorMessage = "Last Trip cannot be more than 50 characters")]
        public string last_trip { get; set; }
    }
}