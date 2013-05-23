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
    public class CarGuruScraper
    {
        HtmlDocument doc = new HtmlDocument();

        public CarGuruScraper()
        {
            
        }

        public CarGuruResult GetResult(string vin)
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
            string mainDocUrl = String.Format("http://www.cargurus.com/Cars/instantMarketValueFromVIN.action?startUrl=%2Findex.html&carDescription.vin={0}", vin);
            HtmlDocument mainDoc = web.Load(mainDocUrl);

            var makerIdNode = mainDoc.DocumentNode.SelectSingleNode("//select[@name='ign-makerId-selectedEntity']/option[@selected='selected'][last()]");
            var modelIdNode = mainDoc.DocumentNode.SelectSingleNode("//select[@name='ign-modelId-selectedEntity']/option[@selected='selected'][last()]");
            var carIdNode = mainDoc.DocumentNode.SelectSingleNode("//select[@name='ign-carId-selectedEntity']/option[@selected='selected'][last()]");
            var trimIdNode = mainDoc.DocumentNode.SelectSingleNode("//select[@name='ign-trimId-selectedEntity']/option[@selected='selected'][last()]");

            HtmlNode entityIdNode = trimIdNode ?? carIdNode ?? modelIdNode ?? makerIdNode;
            var entityId = entityIdNode.Attributes["value"].Value;

            var priceViewUrl = String.Format("http://www.cargurus.com/Cars/priceCalculatorReportAjaxResearchPriceView.action?carDescription.autoEntityId={0}", entityId);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(priceViewUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();
            
            HtmlDocument priceViewDoc = new HtmlDocument();
            priceViewDoc.LoadHtml(@"<html>" + responseString + "</html>");

            CarGuruResult result = new CarGuruResult();
            
            result.InstantMarketValue = priceViewDoc.DocumentNode.SelectSingleNode("//span[@class='instantMarketValue']").InnerHtml;
            
            string numberOfListings = priceViewDoc.DocumentNode.SelectSingleNode("//span[@class='sectionExplanation']").InnerHtml;
            Regex pattern = new Regex(@"^Based on\s*(?<numOfListings>.*)\s*listings");
            Match match = pattern.Match(numberOfListings);
            result.NumberOfListings = match.Groups["numOfListings"].Value.Trim();

            return result;
        }
    }
}
