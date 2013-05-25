using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using CarData.Models;
using HtmlAgilityPack;
namespace CarData.Scrapers
{
    public class AutoTraderScraper
    {
        public event Action<AutoTraderResult> ResultScraped;

        HtmlDocument doc = new HtmlDocument();

        private int index;
        private string yearMakeModel;

        public AutoTraderScraper(int index, string yearMakeModel)
        {
            this.index = index;
            this.yearMakeModel = yearMakeModel;
        }

        public void GetResult()
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

            AutoTraderResult result = new AutoTraderResult();
            result.Index = this.index;

            if (yearMakeModel == "")
            {
                result.AveragePrice = 0;
                result.NumberOfListings = "";

                ResultScraped(result);
                return;
            }

            string mainDocUrl = String.Format("http://www.autotrader.ca/a/pv/new-used/all/all/{0}/?prx=100&cty=TURKEY+POINT&prv=Ontario&r=40&loc=N0E1T0&cat1=2&cat2=7%2c11%2c10%2c9&st=1", yearMakeModel.Replace(" ", "%20"));
            HtmlDocument mainDoc = web.Load(mainDocUrl);

            List<AutoTraderCar> cars = new List<AutoTraderCar>();
            HtmlNodeCollection priceKmNodes = mainDoc.DocumentNode.SelectNodes("//div[@class='at_priceKmArea at_marginB']");

            if (priceKmNodes == null)
            {
                result.AveragePrice = 0;
                result.NumberOfListings = "0";

                ResultScraped(result);
                return;
            }

            foreach (HtmlNode priceKmNode in priceKmNodes)
            {
                string priceInnerHtml = priceKmNode.SelectSingleNode("div[@class='at_price at_sprite']").InnerHtml.Trim();
                double price = 0;
                try
                {
                    price = double.Parse(priceInnerHtml, NumberStyles.Currency);
                }
                catch
                {
                    price = double.Parse(priceInnerHtml.Replace("$", "").Replace(",", ""));
                }

                string meter = "";
                try
                {
                    meter = priceKmNode.SelectSingleNode("div[@class='at_km']").InnerHtml.Trim();
                }
                catch
                {
                    meter = "0";
                }
                cars.Add(new AutoTraderCar { Price = price, Km = meter });
            }

            if (cars.Count == 0)
            {
                result.AveragePrice = 0;
                result.NumberOfListings = "0";

                ResultScraped(result);
                return;
            }

            double totalPrice = 0;
            foreach (var autoTraderCar in cars)
            {
                totalPrice += autoTraderCar.Price;
            }
            result.AveragePrice = totalPrice / cars.Count;
            result.NumberOfListings = "";

            ResultScraped(result);
        }
    }
}
