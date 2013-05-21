using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                            string[] separator = new string[] { ">" };
                            string[] whiteSpaceDelimiter = new string[] { " " };
                            string[] array3 = line.Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            int num = array3.Length;
                            if (num == 3)
                            {
                                string[] array4 = array3[0].Trim().Split(whiteSpaceDelimiter, StringSplitOptions.RemoveEmptyEntries);
                                string text2 = array4[0];
                                string text3 = array4[1];
                                string text4 = array4[2];
                                string text5 = WebUtility.HtmlDecode(array3[1].Substring(0, array3[1].Length - 3));
                                string[] array5 = array3[2].Trim().Split(whiteSpaceDelimiter, StringSplitOptions.RemoveEmptyEntries);
                                string text6 = array5[0];
                                string text7 = array5[1];
                                string text8 = array5[2];
                                string text9 = array5[array5.Length - 3];
                                for (int j = 3; j < array5.Length; j++)
                                {
                                    if (array5[j].Length > 12)
                                    {
                                        text9 = array5[j];
                                    }
                                }
                            }
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
