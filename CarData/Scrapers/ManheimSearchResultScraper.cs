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
    public class ManheimSearchResultScraper
    {
        HtmlDocument doc = new HtmlDocument();

        public ManheimSearchResultScraper(string html)
        {
            doc.LoadHtml(html);
        }

        public IEnumerable<CarModel> GetCars()
        {
            IList<CarModel> cars = new List<CarModel>();

            using (StringReader reader = new StringReader(doc.DocumentNode.SelectSingleNode("//pre").InnerHtml))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        if (line.Contains("pre_inv_ps-vdp"))
                        {
                            CarModel car = new CarModel();
                            
                            Match match = Regex.Match(line, @"<a.*?>(.*?)</a>", RegexOptions.IgnoreCase);
                            string matchValue = match.Value;
                            string makeModel = Regex.Replace(matchValue, @"</?a.*?>", "");
                            line = line.Replace(matchValue, "");

                            string[] carPropertyArray = line.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                            car.Lane = carPropertyArray[0];
                            car.Run = carPropertyArray[1];
                            car.Year = carPropertyArray[2];
                            car.MakeModel = makeModel;
                            car.EngineTransmission = carPropertyArray[3];
                            car.Odometer = carPropertyArray[4];
                            car.Color1 = carPropertyArray[5];
                            car.Color2 = "";
                            car.Vin = carPropertyArray[6];
                            car.Bin = "";
                            car.Bid = "";

                            cars.Add(car);
                        }
                    }

                } while (line != null);
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
