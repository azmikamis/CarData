using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CarData.Models;
using HtmlAgilityPack;
namespace CarData.Scrapers
{
    public class OveSearchResult1Scraper
    {
        public event Action<Dictionary<string, string>> CarInfoScraped;

        HtmlDocument doc = new HtmlDocument();
        
        public OveSearchResult1Scraper(string html)
        {
            doc.LoadHtml(html);
        }

        public void GetCars()
        {
            var carNodes = doc.DocumentNode.SelectNodes("//tr[@class='search_vehicle_basic_row search_name_cell']");

            if (carNodes != null)
            {
                foreach (var carNode in carNodes)
                {
                    Dictionary<string, string> carProperties = new Dictionary<string, string>();

                    carProperties.Add("Lane", "");
                    carProperties.Add("Run", "");
                    carProperties.Add("Year", ScrapeYear(carNode));
                    carProperties.Add("MakeModel", ScrapeMakeModel(carNode));
                    carProperties.Add("EngineTransmission", "");
                    carProperties.Add("Odometer", ScrapeOdometer(carNode));
                    carProperties.Add("Color1", ScrapeColor1(carNode));
                    carProperties.Add("Color2", "");
                    carProperties.Add("Vin", ScrapeVin(carNode));
                    carProperties.Add("Bin", "");
                    carProperties.Add("Bid", "");

                    DecodeThisScraper decodeThisScraper = new DecodeThisScraper();
                    DecodeThisResult decodeThisResult = decodeThisScraper.GetResult(carProperties["Vin"]);
                    carProperties.Add("DecodeThisYearMakeModel", decodeThisResult.YearMakeModel);

                    CarInfoScraped(carProperties);
                }
            }
        }

        private string ScrapeYear(HtmlNode node)
        {
            HtmlNode yearMakeModelNode = node.SelectSingleNode("td[1]/a");
            return yearMakeModelNode.InnerHtml.Split(new char[] { ' ' }, 2)[0];
        }

        private string ScrapeMakeModel(HtmlNode node)
        {
            HtmlNode yearMakeModelNode = node.SelectSingleNode("td[1]/a");
            return yearMakeModelNode.InnerHtml.Split(new char[] { ' ' }, 2)[1];
        }

        private string ScrapeOdometer(HtmlNode node)
        {
            HtmlNode odometerNode = node.SelectSingleNode("td[3]");
            return odometerNode.InnerHtml.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        private string ScrapeColor1(HtmlNode node)
        {
            HtmlNode colorNode = node.SelectSingleNode("td[2]");
            return colorNode.InnerHtml.Split(new string[] { "<br>" }, StringSplitOptions.None)[0];
        }

        private string ScrapeColor2(HtmlNode node)
        {
            HtmlNode colorNode = node.SelectSingleNode("td[2]");
            return colorNode.InnerHtml.Split(new string[] { "<br>" }, StringSplitOptions.None)[1];
        }

        private string ScrapeVin(HtmlNode node)
        {
            HtmlNode vinNode = node.SelectSingleNode("td[1]");
            Regex pattern = new Regex(@"<a.*?>(.*?)</a><br>(?<vin>.*)");
            Match match = pattern.Match(vinNode.InnerHtml);
            return match.Groups["vin"].Value.Trim();
        }

        private string ScrapeBin(HtmlNode node)
        {
            HtmlNode binNode = node.SelectSingleNode("td/table[@class='search_location_cell']/tbody/tr[2]/td/table/tbody/tr/td");
            return binNode == null ? "" : binNode.InnerHtml;
        }

        private string ScrapeBid(HtmlNode node)
        {
            HtmlNode bidNode = node.SelectSingleNode("td/table[@class='search_location_cell']/tbody/tr[4]/td/table/tbody/tr/td");
            return bidNode == null ? "" : bidNode.InnerHtml;
        }
    }
}
