using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using KioskVerwaltung.BusinessObjects;
using System.Drawing.Printing;
using KioskVerwaltung.Printing;

namespace KioskVerwaltung
{
    public class DayStatisticViewModel: INotifyPropertyChanged, Observer
    {
        public ObservableCollection<Sale> Sales
        {
            get { return sales; }
            set {
                sales = value;
                UpdateDay();
                UpdateTotalSaleProducts();
                OnPropertyChanged("Sales");
            }
        }
        private ObservableCollection<Sale> sales;

        public ObservableCollection<TotalSaleProduct> TotalSaleProducts
        {
            get { return totalSaleProducts; }
            set
            {
                totalSaleProducts = value;
                OnPropertyChanged("TotalSaleProducts");
            }
        }
        private ObservableCollection<TotalSaleProduct> totalSaleProducts;

        public DateTime Day
        {
            get { return day; }
            set 
            {
                day = value;
                OnPropertyChanged("Day");
                UpdateTotalSaleProducts();
            }
        }
        private DateTime day;

        public DateTime FromDay
        {
            get { return GetFormDay(); }
        }
        
        public DateTime ToDay
        {
            get { return GetToDay(); }
        }
       
        public double TotalCreditCard
        {
            get { return GetTotalCreditCard(); }
        }
        public double TotalCash
        {
            get { return GetTotalCash(); }
        }
        public double TotalPrivate
        {
            get { return GetTotalPrivate(); }
		}
		public double TotalForGuest
		{
			get { return GetTotalForGuest(); }
		}
		public double Total
        {
            get { return GetTotal(); }
        }


        public DayStatisticViewModel()
        {
            sales = new ObservableCollection<Sale>();
        }

        public void UpdateSales(ObservableCollection<Sale> sales)
        {
            this.sales = sales;
            OnPropertyChanged("Sales");
            Udpate();
        }

        private void UpdateDay()
        {
            if (sales.Count > 0)
            {
                Sale sale = sales[0];
                day = sale.Date;
            }
            OnPropertyChanged("Day");
        }
        private void UpdateTotalSaleProducts()
        {
            totalSaleProducts = new ObservableCollection<TotalSaleProduct>();
            foreach (var sale in sales)
            {
                if (sale.Date.Equals(Day))
                {
                    foreach (var saleProduct in sale.SaleProducts)
                    {
                        if (totalSaleProducts.FirstOrDefault(t => t.ProductId == saleProduct.ProductId && t.SellPrice == saleProduct.SellPrice) == null)
                        {
                            TotalSaleProduct totalSaleProduct = new TotalSaleProduct(saleProduct);
                            totalSaleProducts.Add(totalSaleProduct);
                        }
                        else
                        {
                            totalSaleProducts.FirstOrDefault(t => t.ProductId == saleProduct.ProductId && t.SellPrice == saleProduct.SellPrice).AddSaleProduct(saleProduct);
                        }
                    }
                }
            }
            OnPropertyChanged("TotalSaleProducts");
            OnPropertyChanged("TotalCreditCard");
            OnPropertyChanged("TotalCash");
            OnPropertyChanged("TotalPrivate");
			OnPropertyChanged("TotalForGuest");
			OnPropertyChanged("Total");
            OnPropertyChanged("ToDay");
            OnPropertyChanged("FromDay");
        }

        private DateTime GetFormDay()
        {
            if (sales.Count > 0)
            {
                Sale sale = sales[0];
                return new DateTime(sale.Date.Year, sale.Date.Month, 1);
            }
            return new DateTime();
        }
        private DateTime GetToDay()
        {
            if (sales.Count > 0)
            {
                Sale sale = sales[0];
                DateTime dateTime = new DateTime(sale.Date.Year, sale.Date.Month, 1);
                dateTime = dateTime.AddMonths(1);
                dateTime = dateTime.AddDays(-1);
                return dateTime;
            }
            return new DateTime();
        }

        private double GetTotalCreditCard()
        {
            double total = 0;
            foreach (var sale in totalSaleProducts)
            {
                total += sale.TotalCreditCard;
            }
            return total;
        }
        private double GetTotalCash()
        {
            double total = 0;
            foreach (var sale in totalSaleProducts)
            {
                total += sale.TotalCash;
            }
            return total;
        }
        private double GetTotalPrivate()
        {
            double total = 0;
            foreach (var sale in totalSaleProducts)
            {
                total += sale.TotalPrivate;
            }
            return total;
		}
		private double GetTotalForGuest()
		{
			double total = 0;
			foreach (var sale in totalSaleProducts)
			{
				total += sale.TotalForGuest;
			}
			return total;
		}
		private double GetTotal()
        {
            double total = 0;
            foreach (var sale in totalSaleProducts)
            {
                total += sale.Total;
            }
            return total;
        }

        public void Udpate()
        {
            UpdateDay();
            UpdateTotalSaleProducts();
            OnPropertyChanged("TotalCreditCard");
            OnPropertyChanged("TotalCash");
            OnPropertyChanged("TotalPrivate");
			OnPropertyChanged("TotalForGuest");
			OnPropertyChanged("Total");
            OnPropertyChanged("ToDay");
            OnPropertyChanged("FromDay");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
