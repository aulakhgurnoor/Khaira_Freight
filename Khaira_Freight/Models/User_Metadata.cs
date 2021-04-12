using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khaira_Freight.Models
{
    [MetadataType(typeof(User_Metadata))]
    public partial class UserTable
    {
        public List<SelectListItem> roleList { get; set; }

        public List<SelectListItem> empList { get; set; }
    }
    public class User_Metadata
    {
        [Required(ErrorMessage = "Please enter User ID")]
        [DisplayName("User ID")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter Employee ID")]
        [DisplayName("Employee ID")]
      
        public int EmpId { get; set; }

        [Required(ErrorMessage = "Please enter Username")]
        [DisplayName("Username")]
        [StringLength(20, ErrorMessage = "Username cannot be more than 20 characters")]
      
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [DisplayName("Password")]
        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,20}", ErrorMessage = "Password must contain atleast 1 lowercase aplhabet,1 uppercase alphabet,1 numeric character,1 special character(!@#$%^&*) and must be atleast 8 characters or longer")]
      
        [StringLength(20, ErrorMessage = "Password cannot be more than 20 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter Employee First Name")]
        [DisplayName("First Name")]
        [StringLength(50, ErrorMessage = "Employee First Name cannot be more than 50 characters")]
        public string EmpName { get; set; }

        [DisplayName("Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [DisplayName("Last Login Date")]
        public Nullable<System.DateTime> LastLoginDate { get; set; }

        [DisplayName("Role ID")]
        [Required(ErrorMessage = "Please enter Role ID")]
        public Nullable<int> RoleId { get; set; }

        public virtual RoleTable RoleTable { get; set; }

        public bool RememberMe { get; set; }
    }
}