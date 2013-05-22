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
using CarData.Scrapers;
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
            set { if (source != value) source = value; }
        }

        public ICommand GoToCommand { get; set; }
        public ICommand GetManheimCarsCommand { get; set; }
        public ICommand GetOveCarsCommand { get; set; }
        public ICommand ScrapeAutotraderCommand { get; set; }
        public ICommand ScrapeCarguruCommand { get; set; }

        public ObservableCollection<CarModel> Cars { get; set; }

        public MainWindowViewModel(WebBrowser mainBrowser)
        {
            this.mainBrowser = mainBrowser;

            this.Cars = new ObservableCollection<CarModel>();

            this.GoToCommand = new GoToCommand(this);
            this.GetManheimCarsCommand = new GetManheimCarsCommand(this);
            this.GetOveCarsCommand = new GetOveCarsCommand(this);
            this.ScrapeAutotraderCommand = new ScrapeAutotraderCommand(this);
            this.ScrapeCarguruCommand = new ScrapeCarguruCommand(this);
        }

        public void GoTo()
        {
            this.mainBrowser.Navigate(Source);
        }

        public void GetManheimCars()
        {
            mshtml.IHTMLDocument3 doc3 = (mshtml.IHTMLDocument3)this.mainBrowser.Document;
            string html = doc3.documentElement.outerHTML;

            ManheimSearchResultScraper manheimSearchResultScraper = new ManheimSearchResultScraper(html);
            foreach (var car in manheimSearchResultScraper.GetCars())
                this.Cars.Add(car);
        }

        public void GetOveCars()
        {
            mshtml.IHTMLDocument3 doc3 = (mshtml.IHTMLDocument3)this.mainBrowser.Document;
            string html = doc3.documentElement.outerHTML;

            OveSearchResultScraper oveSearchResultScraper = new OveSearchResultScraper(html);
            foreach (var car in oveSearchResultScraper.GetCars())
                this.Cars.Add(car);
        }

        public void ScrapeAutotrader()
        {

        }

        public void ScrapeCarguru()
        {

        }
    }
}
