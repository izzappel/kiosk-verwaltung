using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KioskVerwaltung.BusinessObjects
{
    public class Sale : INotifyPropertyChanged
    {
        public int Id 
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        public IList<SaleProduct> SaleProducts
        {
            get { return saleProducts; }
            set
            {
                saleProducts = value;
                OnPropertyChanged("SaleProducts");
            }
        }

        public double TotalCash
        {
            get { return GetTotalCash(); }
        }
        public double TotalCreditCard
        {
            get { return GetTotalCreditCard(); }
        }
        public double TotalPrivate
        {
            get { return GetTotalPrivate(); }
        }
        public double Total
        {
            get { return GetTotal(); }
        }

        private int id;
        private DateTime date;
        private IList<SaleProduct> saleProducts;

        public Sale(int id, DateTime date, IList<SaleProduct> saleProducts)
        {
            this.Id = id;
            this.Date = date;
            this.SaleProducts = saleProducts;
        }

        public Sale(DateTime date)
        {
            this.Date = date;
            this.SaleProducts = new  ObservableCollection<SaleProduct>();
        }
        public Sale()
        {
            this.SaleProducts = new ObservableCollection<SaleProduct>();
        }

        private double GetTotalCash()
        {
            double total = 0;
            foreach (var saleProduct in saleProducts)
            {
                if (!saleProduct.IsPaidByCreditCard && !saleProduct.IsPrivate) total += saleProduct.SellPrice;
            }
            return total;
        }
        private double GetTotalCreditCard()
        {
            double total = 0;
            foreach (var saleProduct in saleProducts)
            {
                if (saleProduct.IsPaidByCreditCard && !saleProduct.IsPrivate) total += saleProduct.SellPrice;
            }
            return total;
        }
        private double GetTotalPrivate()
        {
            double total = 0;
            foreach (var saleProduct in saleProducts)
            {
                if (saleProduct.IsPrivate) total += saleProduct.SellPrice;
            }
            return total;
        }
        private double GetTotal()
        {
            double total = 0;
            foreach (var saleProduct in saleProducts)
            {
                if (!saleProduct.IsPrivate) total += saleProduct.SellPrice;
            }
            return total;
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
