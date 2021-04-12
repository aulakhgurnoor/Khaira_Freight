using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khaira_Freight.Models
{
    [MetadataType(typeof(Planner_Metadata))]
    public partial class planner
    {
        public List<SelectListItem> statusList { get; set; }

        public List<SelectListItem> activityList { get; set; }

        public List<SelectListItem> provinceList { get; set; }

      


    }
    public class Planner_Metadata
    {
        [DisplayName("Event ID")]
        public int event_id { get; set; }

        [DisplayName("Status")]
        [Required(ErrorMessage = "Please enter Status")]
        public string status { get; set; }

        [DisplayName("Trip")]
        [Required(ErrorMessage = "Please enter Trip")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int trip { get; set; }

        [DisplayName("Activity")]
        [Required(ErrorMessage = "Please enter Activity")]
        public string activity { get; set; }

        [DisplayName("Driver ID")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public Nullable<int> driver_id { get; set; }

        [DisplayName("Driver Name")]
        [StringLength(50, ErrorMessage = "Driver name cannot be more than 50 characters")]
        public string driver_name { get; set; }

        [DisplayName("City")]
        [StringLength(25, ErrorMessage = "City cannot be more than 25 characters")]
        [Required(ErrorMessage = "Please enter City")]
        public string city { get; set; }

        [DisplayName("Province")]
        [Required(ErrorMessage = "Please enter Province")]
        public string province { get; set; }

        [DisplayName("Truck#")]
        public Nullable<int> truck { get; set; }

        [DisplayName("Trailer#")]
        [StringLength(20, ErrorMessage = "Trailer# cannot be more than 20 characters")]
        public string trailer { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please enter Start Date")]
        public System.DateTime start_date { get; set; }

        [DisplayName("Due Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> due_date { get; set; }
    }
}