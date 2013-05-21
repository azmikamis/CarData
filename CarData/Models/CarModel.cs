using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CarData.Models
{
    public class CarModel : DependencyObject
    {
        public CarModel()
        {

        }

        public Car Car
        {
            set
            {
                this.Year = value.Year;
                this.MakeModel = value.MakeModel;
                this.Odometer = value.Odometer;
                this.Color1 = value.Color1;
                this.Color2 = value.Color2;
                this.Vin = value.Vin;
                this.Bin = value.Bin;
                this.Bid = value.Bid;
                this.AutoTraderPrice = value.AutoTraderPrice;
                this.NumberOfVehicles = value.NumberOfVehicles;
                this.TopThree = value.TopThree;
                this.Profit = value.Profit;
            }

        }

        public string Year
        {
            get { return (string)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string MakeModel
        {
            get { return (string)GetValue(MakeModelProperty); }
            set { SetValue(MakeModelProperty, value); }
        }

        public static readonly DependencyProperty MakeModelProperty =
            DependencyProperty.Register("MakeModel", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string Odometer
        {
            get { return (string)GetValue(OdometerProperty); }
            set { SetValue(OdometerProperty, value); }
        }

        public static readonly DependencyProperty OdometerProperty =
            DependencyProperty.Register("Odometer", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string Color1
        {
            get { return (string)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register("Color1", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string Color2
        {
            get { return (string)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }

        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register("Color2", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string Vin
        {
            get { return (string)GetValue(VinProperty); }
            set { SetValue(VinProperty, value); }
        }

        public static readonly DependencyProperty VinProperty =
            DependencyProperty.Register("Vin", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string Bin
        {
            get { return (string)GetValue(BinProperty); }
            set { SetValue(BinProperty, value); }
        }

        public static readonly DependencyProperty BinProperty =
            DependencyProperty.Register("Bin", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string Bid
        {
            get { return (string)GetValue(BidProperty); }
            set { SetValue(BidProperty, value); }
        }

        public static readonly DependencyProperty BidProperty =
            DependencyProperty.Register("Bid", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string AutoTraderPrice
        {
            get { return (string)GetValue(AutoTraderPriceProperty); }
            set { SetValue(AutoTraderPriceProperty, value); }
        }

        public static readonly DependencyProperty AutoTraderPriceProperty =
            DependencyProperty.Register("AutoTraderPrice", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string NumberOfVehicles
        {
            get { return (string)GetValue(NumberOfVehiclesProperty); }
            set { SetValue(NumberOfVehiclesProperty, value); }
        }

        public static readonly DependencyProperty NumberOfVehiclesProperty =
            DependencyProperty.Register("NumberOfVehicles", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string TopThree
        {
            get { return (string)GetValue(TopThreeProperty); }
            set { SetValue(TopThreeProperty, value); }
        }

        public static readonly DependencyProperty TopThreeProperty =
            DependencyProperty.Register("TopThree", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));

        public string Profit
        {
            get { return (string)GetValue(ProfitProperty); }
            set { SetValue(ProfitProperty, value); }
        }

        public static readonly DependencyProperty ProfitProperty =
            DependencyProperty.Register("Profit", typeof(string), typeof(CarModel), new UIPropertyMetadata(""));
    }
}
