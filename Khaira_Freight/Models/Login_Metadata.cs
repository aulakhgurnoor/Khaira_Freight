using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khaira_Freight.Models
{
    [MetadataType(typeof(Login_Metadata))]
    public partial class employee_login
    {
        public List<SelectListItem> departmentList { get; set; }

        public List<SelectListItem> DriverId_List { get; set; }
    }
    public class Login_Metadata
    {
        [Required(ErrorMessage = "Please enter Employee ID")]
        [DisplayName("Employee ID")]
        public int emp_id { get; set; }

        [Required(ErrorMessage = "Please enter Department")]
        [DisplayName("Department")]
        public string department { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        [DisplayName("First Name")]
        [StringLength(25, ErrorMessage = "First Name cannot be more than 25 characters")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [DisplayName("Last Name")]
        [StringLength(25, ErrorMessage = "Last name cannot be more than 25 characters")]
        public string last_name { get; set; }

        [Required(ErrorMessage = "Please enter Username")]
        [DisplayName("Username")]
        [StringLength(25, ErrorMessage = "Username cannot be more than 25 characters")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [DisplayName("Password")]
       // [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Password cannot be more than 25 characters")]
        public string password { get; set; }
    }
}