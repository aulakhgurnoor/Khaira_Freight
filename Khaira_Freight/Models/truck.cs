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
    
    public partial class truck
    {
        public int unit_number { get; set; }
        public string plate_number { get; set; }
        public string registration { get; set; }
        public string vin_number { get; set; }
        public string make { get; set; }
        public int year { get; set; }
        public decimal weight { get; set; }
        public string status { get; set; }
        public string owner_company { get; set; }
        public string driver { get; set; }
        public string last_location { get; set; }
        public string last_trip { get; set; }
    }
}