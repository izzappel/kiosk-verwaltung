using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public class TotalSaleProduct
    {
        public int ProductId
        {
            get
            {
                return productId;
            }
            set
            {
                productId = value;
                OnPropertyChanged("ProductId");
            }
        }
        private int productId;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private string name;

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }
        private int count;

        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }
        private double price;

        public string Deduction
        {
            get
            {
                return deduction;
            }
            set
            {
                deduction = value;
                OnPropertyChanged("Deduction");
            }
        }
        private string deduction;

        public double SellPrice
        {
            get
            {
                return sellPrice;
            }
            set
            {
                sellPrice = value;
                OnPropertyChanged("SellPrice");
            }
        }
        private double sellPrice;

        public double TotalCash
        {
            get
            {
                return totalCash;
            }
            set
            {
                totalCash = value;
                OnPropertyChanged("TotalCash");
            }
        }
        private double totalCash;

        public double TotalCreditCard
        {
            get
            {
                return totalCreditCard;
            }
            set
            {
                totalCreditCard = value;
                OnPropertyChanged("TotalCreditCard");
            }
        }
        private double totalCreditCard;

        public double TotalPrivate
        {
            get
            {
                return totalPrivate;
            }
            set
            {
                totalPrivate = value;
                OnPropertyChanged("TotalPrivate");
            }
        }
        private double totalPrivate;

		public double TotalForGuest
		{
			get
			{
				return totalForGuest;
			}
			set
			{
				totalForGuest = value;
				OnPropertyChanged("TotalForGuest");
			}
		}
		private double totalForGuest;

		public double Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }
        private double total;

        public TotalSaleProduct(int productId, string name, int count, double price, string deduction, double sellPrice, double totalCash, double totalCreditCard, double totalPrivate, double totalForGuest, double total)
        {
            this.ProductId = productId;
            this.Name = name;
            this.Count = count;
            this.Price = price;
            this.Deduction = deduction;
            this.SellPrice = sellPrice;
            this.TotalCash = totalCash;
            this.TotalCreditCard = totalCreditCard;
            this.TotalPrivate = totalPrivate;
			this.TotalForGuest = totalForGuest;
            this.Total = total;
        }

        public TotalSaleProduct(SaleProduct saleProduct)
        {
            this.ProductId = saleProduct.ProductId;
            this.Name = saleProduct.Name;
            this.Count = 1;
            this.Price = saleProduct.Price;
            this.Deduction = saleProduct.Deduction;
            this.SellPrice = saleProduct.SellPrice;
            this.TotalCash = ((!saleProduct.IsPaidByCreditCard && !saleProduct.IsPrivate && !saleProduct.IsForGuest) ? saleProduct.SellPrice : 0d);
            this.TotalCreditCard = ((saleProduct.IsPaidByCreditCard) ? saleProduct.SellPrice : 0d);
            this.TotalPrivate = ((saleProduct.IsPrivate) ? saleProduct.SellPrice : 0d);
			this.TotalForGuest = ((saleProduct.IsForGuest) ? saleProduct.SellPrice : 0d);
			this.Total = ((!saleProduct.IsPrivate && !saleProduct.IsForGuest) ? saleProduct.SellPrice : 0d);
        }

        public TotalSaleProduct()
        {

        }

        public void AddSaleProduct(SaleProduct saleProduct)
        {
            if (ProductId == saleProduct.ProductId && SellPrice == saleProduct.SellPrice)
            {
                Count++;
                TotalCash += ((!saleProduct.IsPaidByCreditCard && !saleProduct.IsPrivate) ? saleProduct.SellPrice : 0d);
                TotalCreditCard += ((saleProduct.IsPaidByCreditCard && !saleProduct.IsPrivate) ? saleProduct.SellPrice : 0d);
                TotalPrivate += ((saleProduct.IsPrivate) ? saleProduct.SellPrice : 0d);
                Total += ((!saleProduct.IsPrivate) ? saleProduct.SellPrice : 0d);
            }
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
