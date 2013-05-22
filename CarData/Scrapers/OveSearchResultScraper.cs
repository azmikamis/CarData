using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var carNodes = doc.DocumentNode.SelectNodes("//tr[@class='search_vehicle_row']");

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
                    car.Bin = ScrapeBin(carNode);
                    car.Bid = ScrapeBid(carNode);

                    cars.Add(car);
                }
            }

            return cars;
        }

        private string ScrapeYear(HtmlNode node)
        {
            HtmlNode yearMakeModelNode = node.SelectSingleNode("td/table[@class='search_name_cell']/tbody/tr[1]/td/a");
            return yearMakeModelNode.InnerHtml.Split(new char[] { ' ' }, 2)[0];
        }

        private string ScrapeMakeModel(HtmlNode node)
        {
            HtmlNode yearMakeModelNode = node.SelectSingleNode("td/table[@class='search_name_cell']/tbody/tr[1]/td/a");
            return yearMakeModelNode.InnerHtml.Split(new char[] { ' ' }, 2)[1];
        }

        private string ScrapeOdometer(HtmlNode node)
        {
            HtmlNode odometerNode = node.SelectSingleNode("td/table[@class='search_name_cell']/tbody/tr[3]/td/text()");
            return odometerNode.InnerHtml.Replace("&nbsp;", "");
        }

        private string ScrapeColor1(HtmlNode node)
        {
            HtmlNode color1Node = node.SelectSingleNode("td[@class='search_thumb_cell']/table/tbody/tr[2]/td[1]");
            return color1Node.InnerHtml;
        }

        private string ScrapeColor2(HtmlNode node)
        {
            HtmlNode color2Node = node.SelectSingleNode("td[@class='search_thumb_cell']/table/tbody/tr[2]/td[3]");
            return color2Node.InnerHtml;
        }

        private string ScrapeVin(HtmlNode node)
        {
            HtmlNode vinNode = node.SelectSingleNode("td/table[@class='search_name_cell']/tbody/tr[2]/td");
            return vinNode.InnerHtml;
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
