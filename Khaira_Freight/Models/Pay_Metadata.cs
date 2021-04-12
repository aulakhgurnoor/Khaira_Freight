using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Khaira_Freight.Models
{
    [MetadataType(typeof(Pay_Metadata))]
    public partial class pay_statement
    {

    }

    public class Pay_Metadata
    {
        [DisplayName("File ID")]
        public int Id { get; set; }

        [DisplayName("File Name")]
        public string Name { get; set; }

        [DisplayName("File Type")]
        public string ContentType { get; set; }

        [DisplayName("Data")]
        public byte[] Data { get; set; }

        [DisplayName("Employee ID")]
        public int emp_id { get; set; }

        [DisplayName("Department")]
        public string department { get; set; }
    }
}