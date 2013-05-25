using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarData.Models
{
    public class AutoTraderSearchOption
    {
        public string BodyStyle { get; set; }
        public bool CarproofOnly { get; set; }
        public string Category { get; set; }
        public string Colour { get; set; }
        public string CompanyForeignID { get; set; }
        public string CompanyID { get; set; }
        public string CompanySourceID { get; set; }
        public string FuelType { get; set; }
        public string Keyword { get; set; }
        public string LCID { get; set; }
        public string Make { get; set; }
        public string MaxHours { get; set; }
        public string MaxLength { get; set; }
        public string MaxOdometer { get; set; }
        public string MaxPrice { get; set; }
        public string MaxYear { get; set; }
        public string MicroSite { get; set; }
        public string MinHours { get; set; }
        public string MinLength { get; set; }
        public string MinOdometer { get; set; }
        public string MinPrice { get; set; }
        public string MinYear { get; set; }
        public string Model { get; set; }
        public bool PriceOnly { get; set; }
        public int Proximity { get; set; }
        public string SearchLocation { get; set; }
        public bool ShowCPO { get; set; }
        public bool ShowDealer { get; set; }
        public bool ShowNew { get; set; }
        public bool ShowPrivate { get; set; }
        public bool ShowUsed { get; set; }
        public string SubType { get; set; }
        public string Transmission { get; set; }
        public string Trim { get; set; }
        public string Type { get; set; }
    }
}
