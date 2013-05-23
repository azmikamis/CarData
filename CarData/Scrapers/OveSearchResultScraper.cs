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
    public class OveSearchResultScraper
    {
        HtmlDocument doc = new HtmlDocument();
        
        public OveSearchResultScraper(string html)
        {
            doc.LoadHtml(html);
        }

        public IEnumerable<CarModel> GetCars()
        {
            IList<CarModel> cars = new List<CarModel>();

            var carNodes = doc.DocumentNode.SelectNodes("//tr[@class='search_vehicle_basic_row search_name_cell']");

            if (carNodes != null)
            {
                foreach (var carNode in carNodes)
                {
                    CarModel car = new CarModel();

                    car.Lane = "";
                    car.Run = "";
                    car.Year = ScrapeYear(carNode);
                    car.MakeModel = ScrapeMakeModel(carNode);
                    car.EngineTransmission = "";
                    car.Odometer = ScrapeOdometer(carNode);
                    car.Color1 = ScrapeColor1(carNode);
                    car.Color2 = ScrapeColor2(carNode);
                    car.Vin = ScrapeVin(carNode);
                    car.Bin = "";// ScrapeBin(carNode);
                    car.Bid = "";// ScrapeBid(carNode);

                    DecodeThisScraper decodeThisScraper = new DecodeThisScraper();
                    DecodeThisResult decodeThisResult = decodeThisScraper.GetResult(car.Vin);
                    car.DecodeThisYearMakeModel = decodeThisResult.YearMakeModel;

                    cars.Add(car);
                }
            }

            return cars;
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
