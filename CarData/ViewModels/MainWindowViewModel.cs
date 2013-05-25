using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarData.Commands;
using CarData.Models;
using CarData.Scrapers;
using HtmlAgilityPack;
using WinFormsSaveFileDialog = System.Windows.Forms.SaveFileDialog;
using WinFormsDialogResult = System.Windows.Forms.DialogResult;
namespace CarData.ViewModels
{
    public class MainWindowViewModel : DependencyObject
    {
        private readonly Dispatcher currentDispatcher;
        private WebBrowser mainBrowser;

        private string source;

        public string Source
        {
            get { return source; }
            set { if (source != value) source = value; }
        }

        public ICommand GoToManheimCommand { get; set; }
        public ICommand GoToOveCommand { get; set; }
        public ICommand GoToCommand { get; set; }
        public ICommand GetManheimCarsCommand { get; set; }
        public ICommand GetOveCars1Command { get; set; }
        public ICommand GetOveCars2Command { get; set; }
        public ICommand ScrapeAutotraderCommand { get; set; }
        public ICommand ScrapeCarguruCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand ClearListCommand { get; set; }

        public ObservableCollection<CarModel> Cars { get; set; }

        public MainWindowViewModel(WebBrowser mainBrowser)
        {
            this.currentDispatcher = Dispatcher.CurrentDispatcher;
            this.mainBrowser = mainBrowser;

            this.Cars = new ObservableCollection<CarModel>();

            this.GoToManheimCommand = new GoToManheimCommand(this);
            this.GoToOveCommand = new GoToOveCommand(this);
            this.GoToCommand = new GoToCommand(this);
            this.GetManheimCarsCommand = new GetManheimCarsCommand(this);
            this.GetOveCars1Command = new GetOveCars1Command(this);
            this.GetOveCars2Command = new GetOveCars2Command(this);
            this.ScrapeAutotraderCommand = new ScrapeAutotraderCommand(this);
            this.ScrapeCarguruCommand = new ScrapeCarguruCommand(this);
            this.ExportCommand = new ExportCommand(this);
            this.ClearListCommand = new ClearListCommand(this);
        }

        public void GoTo()
        {
            this.mainBrowser.Navigate(Source);
        }

        public void GoToManheim()
        {
            Source = "http://www.manheim.com/";
            this.mainBrowser.Navigate(Source);
        }

        public void GoToOve()
        {
            Source = "http://www.exporttrader.com";
            this.mainBrowser.Navigate(Source);
        }

        public void GetManheimCars()
        {
            mshtml.IHTMLDocument3 doc3 = (mshtml.IHTMLDocument3)this.mainBrowser.Document;
            string html = doc3.documentElement.outerHTML;

            ManheimSearchResultScraper manheimSearchResultScraper = new ManheimSearchResultScraper(html);
            manheimSearchResultScraper.CarInfoScraped += new Action<Dictionary<string, string>>(ManheimCarInfoScraped);

            Thread scraperThread = new Thread(new ThreadStart(manheimSearchResultScraper.GetCars)) { IsBackground = true, Priority = ThreadPriority.Normal };
            scraperThread.Start();
        }

        void ManheimCarInfoScraped(Dictionary<string, string> carProperties)
        {
            currentDispatcher.BeginInvoke((Action)(delegate
            {
                CarModel car = new CarModel();

                car.Lane = carProperties["Lane"];
                car.Run = carProperties["Run"];
                car.Year = carProperties["Year"];
                car.MakeModel = carProperties["MakeModel"];
                car.EngineTransmission = carProperties["EngineTransmission"];
                car.Odometer = carProperties["Odometer"];
                car.Color1 = carProperties["Color1"];
                car.Color2 = carProperties["Color2"];
                car.Vin = carProperties["Vin"];
                car.Bin = carProperties["Bin"];
                car.Bid = carProperties["Bid"];
                car.DecodeThisYearMakeModel = carProperties["DecodeThisYearMakeModel"];

                Cars.Add(car);
            }));
        }

        public void GetOveCars1()
        {
            mshtml.IHTMLDocument3 doc3 = (mshtml.IHTMLDocument3)this.mainBrowser.Document;
            string html = doc3.documentElement.outerHTML;

            OveSearchResult1Scraper oveSearchResultScraper = new OveSearchResult1Scraper(html);
            oveSearchResultScraper.CarInfoScraped += new Action<Dictionary<string, string>>(OveCarInfo1Scraped);

            Thread scraperThread = new Thread(new ThreadStart(oveSearchResultScraper.GetCars)) { IsBackground = true, Priority = ThreadPriority.Normal };
            scraperThread.Start();
        }

        public void GetOveCars2()
        {
            mshtml.IHTMLDocument3 doc3 = (mshtml.IHTMLDocument3)this.mainBrowser.Document;
            string html = doc3.documentElement.outerHTML;

            OveSearchResult2Scraper oveSearchResultScraper = new OveSearchResult2Scraper(html);
            oveSearchResultScraper.CarBinBidsScraped += new Action<List<OveCarBinBid>>(OveCarBinBidsScraped);

            Thread scraperThread = new Thread(new ThreadStart(oveSearchResultScraper.GetCars)) { IsBackground = true, Priority = ThreadPriority.Normal };
            scraperThread.Start();
        }

        void OveCarBinBidsScraped(List<OveCarBinBid> obj)
        {
            currentDispatcher.BeginInvoke((Action)(delegate
            {
                for (int i = 0; i < Cars.Count; i++)
                {
                    Cars[i].Bin = obj[i].Bin;
                    Cars[i].Bid = obj[i].Bid;
                }
            }));
        }

        void OveCarInfo1Scraped(Dictionary<string, string> carProperties)
        {
            currentDispatcher.BeginInvoke((Action)(delegate
            {
                CarModel car = new CarModel();

                car.Lane = carProperties["Lane"];
                car.Run = carProperties["Run"];
                car.Year = carProperties["Year"];
                car.MakeModel = carProperties["MakeModel"];
                car.EngineTransmission = carProperties["EngineTransmission"];
                car.Odometer = carProperties["Odometer"];
                car.Color1 = carProperties["Color1"];
                car.Color2 = carProperties["Color2"];
                car.Vin = carProperties["Vin"];
                car.Bin = carProperties["Bin"];
                car.Bid = carProperties["Bid"];
                car.DecodeThisYearMakeModel = carProperties["DecodeThisYearMakeModel"];

                Cars.Add(car);
            }));
        }

        void OveCarInfo2Scraped(Dictionary<string, string> carProperties)
        {
            currentDispatcher.BeginInvoke((Action)(delegate
            {
                CarModel car = new CarModel();

                car.Lane = carProperties["Lane"];
                car.Run = carProperties["Run"];
                car.Year = carProperties["Year"];
                car.MakeModel = carProperties["MakeModel"];
                car.EngineTransmission = carProperties["EngineTransmission"];
                car.Odometer = carProperties["Odometer"];
                car.Color1 = carProperties["Color1"];
                car.Color2 = carProperties["Color2"];
                car.Vin = carProperties["Vin"];
                car.Bin = carProperties["Bin"];
                car.Bid = carProperties["Bid"];
                car.DecodeThisYearMakeModel = carProperties["DecodeThisYearMakeModel"];

                Cars.Add(car);
            }));
        }

        public void ScrapeAutotrader()
        {
            int index = 0;
            foreach (var car in Cars)
            {
                AutoTraderScraper scraper = new AutoTraderScraper(index, car.DecodeThisYearMakeModel);
                scraper.ResultScraped += new Action<AutoTraderResult>(AutoTraderResultScraped);
                index++;

                Thread scraperThread = new Thread(new ThreadStart(scraper.GetResult)) { IsBackground = true, Priority = ThreadPriority.Normal };
                scraperThread.Start();
            }
        }

        void AutoTraderResultScraped(AutoTraderResult result)
        {
            currentDispatcher.BeginInvoke((Action)(delegate
            {
                Cars[result.Index].AutoTraderPrice = result.AveragePrice.ToString();
                Cars[result.Index].AutoTraderNumberOfVehicles = result.NumberOfListings;
            }));
        }

        public void ScrapeCarguru()
        {
            int index = 0;
            foreach (var car in Cars)
            {
                CarGuruScraper scraper = new CarGuruScraper(index, car.Vin);
                scraper.ResultScraped += new Action<CarGuruResult>(CarGuruResultScraped);
                index++;

                Thread scraperThread = new Thread(new ThreadStart(scraper.GetResult)) { IsBackground = true, Priority = ThreadPriority.Normal };
                scraperThread.Start();
            }
        }

        void CarGuruResultScraped(CarGuruResult result)
        {
            currentDispatcher.BeginInvoke((Action)(delegate
            {
                Cars[result.Index].CarGuruPrice = result.InstantMarketValue;
                Cars[result.Index].CarGuruNumberOfVehicles = result.NumberOfListings;
            }));
        }

        public void ClearList()
        {
            this.Cars.Clear();
        }

        public void Export()
        {
            WinFormsSaveFileDialog saveFileDialog = new WinFormsSaveFileDialog();
            saveFileDialog.Filter = "CSV files|*.csv";
            if (saveFileDialog.ShowDialog() == WinFormsDialogResult.OK)
            {
                StringBuilder result = new StringBuilder();
                foreach (var car in this.Cars)
                {
                    result.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                        car.Lane,
                        car.Run,
                        car.Year,
                        car.MakeModel,
                        car.EngineTransmission,
                        car.Odometer,
                        car.Color1,
                        car.Color2,
                        car.Vin,
                        car.Bin,
                        car.Bid,
                        car.AutoTraderPrice,
                        car.AutoTraderNumberOfVehicles,
                        car.TopThree,
                        car.Profit,
                        car.CarGuruPrice,
                        car.CarGuruNumberOfVehicles);
                    result.AppendLine();
                }
                System.IO.File.WriteAllText(saveFileDialog.FileName, result.ToString());
            }
        }
    }
}
