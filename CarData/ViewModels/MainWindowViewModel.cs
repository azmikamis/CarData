using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarData.Commands;
using CarData.Models;
using HtmlAgilityPack;
namespace CarData.ViewModels
{
    public class MainWindowViewModel : DependencyObject
    {
        private WebBrowser mainBrowser;

        private string source;

        public string Source
        {
            get { return source; }
            set
            {
                if (source != value)
                {
                    source = value;
                }
            }
        }

        public ICommand GoToCommand
        {
            get;
            set;
        }

        public ICommand GetCarsCommand
        {
            get;
            set;
        }

        public ObservableCollection<CarModel> Cars 
        { 
            get;
            set; 
        }
        public MainWindowViewModel(WebBrowser mainBrowser)
        {
            this.mainBrowser = mainBrowser;

            this.Cars = new ObservableCollection<CarModel>();

            this.GoToCommand = new GoToCommand(this);
            this.GetCarsCommand = new GetCarsCommand(this);
        }

        public void GoTo()
        {
            this.mainBrowser.Navigate(Source);
        }

        public void GetCars()
        {
            mshtml.IHTMLDocument3 doc3 = (mshtml.IHTMLDocument3)this.mainBrowser.Document;
            string html = doc3.documentElement.outerHTML;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var cars = doc.DocumentNode.SelectSingleNode("//table[@class='search_list_container']");
            foreach (var carNode in cars.SelectNodes("//tr[@class='search_vehicle_row']"))
            {
                CarModel car = new CarModel();

                string yearMakeModel = carNode.SelectSingleNode("td/table[@class='search_name_cell']/tbody/tr[1]/td/a").InnerHtml;
                string[] yearMakeModelSplit = yearMakeModel.Split(new char[] { ' ' }, 2);
                car.Year = yearMakeModelSplit[0];
                car.MakeModel = yearMakeModelSplit[1];

                string odometer = carNode.SelectSingleNode("td/table[@class='search_name_cell']/tbody/tr[3]/td/text()").InnerHtml.Replace("&nbsp;", "");
                car.Odometer = odometer;

                string color1 = carNode.SelectSingleNode("td[@class='search_thumb_cell']/table/tbody/tr[2]/td[1]").InnerHtml;
                car.Color1 = color1;

                string color2 = carNode.SelectSingleNode("td[@class='search_thumb_cell']/table/tbody/tr[2]/td[3]").InnerHtml;
                car.Color2 = color2;

                string vin = carNode.SelectSingleNode("td/table[@class='search_name_cell']/tbody/tr[2]/td").InnerHtml;
                car.Vin = vin;

                HtmlNode binNode = carNode.SelectSingleNode("td/table[@class='search_location_cell']/tbody/tr[2]/td/table/tbody/tr/td");
                string bin = binNode == null ? "" : binNode.InnerHtml;
                car.Bin = bin;

                HtmlNode bidNode = carNode.SelectSingleNode("td/table[@class='search_location_cell']/tbody/tr[4]/td/table/tbody/tr/td");
                string bid = bidNode == null ? "" : bidNode.InnerHtml;
                car.Bid = bid;

                this.Cars.Add(car);
            }
        }
    }
}
