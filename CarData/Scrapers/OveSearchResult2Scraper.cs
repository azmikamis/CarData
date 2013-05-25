using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CarData.Models;
using HtmlAgilityPack;
namespace CarData.Scrapers
{
    public class OveSearchResult2Scraper
    {
        public event Action<List<OveCarBinBid>> CarBinBidsScraped;

        HtmlDocument doc = new HtmlDocument();
        
        public OveSearchResult2Scraper(string html)
        {
            doc.LoadHtml(html);
        }

        public void GetCars()
        {
            var carNodes = doc.DocumentNode.SelectNodes("//tr[@class='search_vehicle_row']");

            List<OveCarBinBid> oveCarBinBids = new List<OveCarBinBid>();

            if (carNodes != null)
            {
                foreach (var carNode in carNodes)
                {
                    string bin = ScrapeBin(carNode).Replace("USD", "").Trim();
                    string bid = ScrapeBid(carNode).Replace("USD", "").Trim();

                    try
                    {
                        oveCarBinBids.Add(new OveCarBinBid
                        {
                            Bin = double.Parse(bin, NumberStyles.Currency).ToString(),
                            Bid = double.Parse(bid, NumberStyles.Currency).ToString()
                        });
                    }
                    catch
                    {
                        oveCarBinBids.Add(new OveCarBinBid
                        {
                            Bin = bin.Replace("$", "").Replace(",", ""),
                            Bid = bid.Replace("$", "").Replace(",", "")
                        });
                    }
                }                  

                CarBinBidsScraped(oveCarBinBids);
            }
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
