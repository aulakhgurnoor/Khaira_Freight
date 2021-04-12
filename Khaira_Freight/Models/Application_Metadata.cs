using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Khaira_Freight.Models
{

    [MetadataType(typeof(Application_Metadata))]
    public partial class driver_application
    {

    }
    public class Application_Metadata
    {
        [DisplayName("File ID")]
        public int Id { get; set; }

        [DisplayName("File Name")]
        public string Name { get; set; }

        [DisplayName("File Type")]
        public string ContentType { get; set; }

        [DisplayName("Data")]
        public byte[] Data { get; set; }
    }
}