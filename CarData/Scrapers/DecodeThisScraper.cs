using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using CarData.Models;
using HtmlAgilityPack;
namespace CarData.Scrapers
{
    public class DecodeThisScraper
    {
        HtmlDocument doc = new HtmlDocument();

        public DecodeThisScraper()
        {
            
        }

        public DecodeThisResult GetResult(string vin)
        {
            if (HtmlNode.ElementsFlags.ContainsKey("option"))
            {
                HtmlNode.ElementsFlags["option"] = HtmlElementFlag.Closed;
            }
            else
            {
                HtmlNode.ElementsFlags.Add("option", HtmlElementFlag.Closed);
            }

            HtmlWeb web = new HtmlWeb();
            string url = String.Format("http://www.decodethis.com/VIN-Decoded/vin/{0}", vin);
            HtmlDocument doc = web.Load(url);

            var carDataNode = doc.DocumentNode.SelectSingleNode("//table[@class='cardata']");

            DecodeThisResult result = new DecodeThisResult();

            if (carDataNode != null)
            {
                result.YearMakeModel = carDataNode.SelectSingleNode("tr[2]/td[2]").InnerHtml + " " +
                                       carDataNode.SelectSingleNode("tr/td[@itemprop='manufacturer']").InnerHtml + " " +
                                       carDataNode.SelectSingleNode("tr/td[@itemprop='model']").InnerHtml;
            }
            else
            {
                result.YearMakeModel = doc.DocumentNode.SelectSingleNode("//span[@id='dnn_ctr500_VehicleIdPrime_lblName']").InnerHtml;
            }

            return result;
        }
    }
}
