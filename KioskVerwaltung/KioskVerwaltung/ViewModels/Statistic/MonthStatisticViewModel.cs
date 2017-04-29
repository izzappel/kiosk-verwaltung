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
    public class MonthStatisticViewModel: INotifyPropertyChanged, Observer
    {
        public ObservableCollection<Sale> Sales
        {
            get { return sales; }
        }
        private ObservableCollection<Sale> sales;

        public ObservableCollection<TotalSaleProduct> TotalSaleProducts
        {
            get { return totalSaleProducts; }
            set { 
                totalSaleProducts = value;
                OnPropertyChanged("TotalSaleProducts");
            }
        }
        private ObservableCollection<TotalSaleProduct> totalSaleProducts;

        public string Month
        {
            get { return GetMonth(); }
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
        public double Total
        {
            get { return GetTotal(); }
        }

        public MonthStatisticViewModel()
        {
            sales = new ObservableCollection<Sale>();
        }

        public void UpdateSales(ObservableCollection<Sale> sales)
        {
            this.sales = sales;
            OnPropertyChanged("Sales");
            Udpate();
        }

        private void UpdateTotalSaleProducts()
        {
            totalSaleProducts = new ObservableCollection<TotalSaleProduct>();
            foreach (var sale in sales)
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
            OnPropertyChanged("TotalSaleProducts");
        }

        private string GetMonth()
        {
            if (sales.Count > 0)
            {
                Sale sale = sales[0];
                return sale.Date.ToString("MMMM yyyy");
            }
            return string.Empty;
        }

        private double GetTotalCreditCard()
        {
            double total = 0;
            foreach(var sale in totalSaleProducts)
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
            UpdateTotalSaleProducts();
            OnPropertyChanged("Month");
            OnPropertyChanged("TotalCreditCard");
            OnPropertyChanged("TotalCash");
            OnPropertyChanged("TotalPrivate");
            OnPropertyChanged("Total");
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
