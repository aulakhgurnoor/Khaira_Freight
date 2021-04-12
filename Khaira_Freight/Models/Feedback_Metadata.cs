using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Khaira_Freight.Models
{
    [MetadataType(typeof(Feedback_Metadata))]
    public partial class feedback
    {
  
    }
    public class Feedback_Metadata
    {

        [Required]
        [DisplayName("Feedback Id")]
        public int id { get; set; }

        [DisplayName("Full Name")]
        [StringLength(50, ErrorMessage = "Name cannot be more than 50 characters")]
        public string full_name { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30 characters")]
        public string email { get; set; }

        [DisplayName("Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [StringLength(15, ErrorMessage = "Phone number cannot be more than 15 characters")]
        public string phone { get; set; }

        [DisplayName("Subject")]
        [StringLength(50, ErrorMessage = "Subject cannot be more than 50 characters")]
        public string message_subject { get; set; }

        [Required(ErrorMessage = "Please enter your message")]
        [DisplayName("Message")]
        [StringLength(200, ErrorMessage = "Message cannot be more than 200 characters")]
        public string message_body { get; set; }

        [DisplayName("Added On")]
        public Nullable<System.DateTime> added_on { get; set; }
    }
}