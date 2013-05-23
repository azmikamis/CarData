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

                            DecodeThisScraper decodeThisScraper = new DecodeThisScraper();
                            DecodeThisResult decodeThisResult = decodeThisScraper.GetResult(car.Vin);
                            car.DecodeThisYearMakeModel = decodeThisResult.YearMakeModel;

                            cars.Add(car);
                        }
                    }

                } while (line != null);
            }

            return cars;
        }
    }
}
