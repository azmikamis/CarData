using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public event Action<Dictionary<string, string>> CarInfoScraped;

        HtmlDocument doc = new HtmlDocument();

        public ManheimSearchResultScraper(string html)
        {
            doc.LoadHtml(html);
        }

        public void Start()
        {
            
        }

        public void GetCars()
        {
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
                            Dictionary<string, string> carProperties = new Dictionary<string, string>();

                            Match match = Regex.Match(line, @"<a.*?>(.*?)</a>", RegexOptions.IgnoreCase);
                            string matchValue = match.Value;
                            string makeModel = Regex.Replace(matchValue, @"</?a.*?>", "");
                            line = line.Replace(matchValue, "");

                            string[] carPropertyArray = line.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                            carProperties.Add("Lane", carPropertyArray[0]);
                            carProperties.Add("Run", carPropertyArray[1]);
                            carProperties.Add("Year", carPropertyArray[2]);
                            carProperties.Add("MakeModel", makeModel);
                            carProperties.Add("EngineTransmission", carPropertyArray[3]);
                            carProperties.Add("Odometer", carPropertyArray[4]);
                            carProperties.Add("Color1", carPropertyArray[5]);
                            carProperties.Add("Color2", "");
                            carProperties.Add("Vin", carPropertyArray[6]);
                            carProperties.Add("Bin", "");
                            carProperties.Add("Bid", "");

                            DecodeThisScraper decodeThisScraper = new DecodeThisScraper();
                            DecodeThisResult decodeThisResult = decodeThisScraper.GetResult(carProperties["Vin"]);
                            carProperties.Add("DecodeThisYearMakeModel", decodeThisResult.YearMakeModel);

                            CarInfoScraped(carProperties);
                        }
                    }

                } while (line != null);
            }
        }
    }
}
