//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Khaira_Freight.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class quote
    {
        public int id { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string street_address { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string province { get; set; }
        public string pickup_location { get; set; }
        public System.DateTime pickup_date { get; set; }
        public string drop_location { get; set; }
        public System.DateTime drop_date { get; set; }
        public string equipment_type { get; set; }
        public decimal weight { get; set; }
        public string commodity { get; set; }
    }
}
