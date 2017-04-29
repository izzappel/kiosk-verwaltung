using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KioskVerwaltung.BusinessObjects
{
    public class SaleProduct : INotifyPropertyChanged
    {
        public int Id 
        { 
            get
            {
                return id;
            } 
            set
            {
                id = value;
                OnPropertyChanged("Id");
            } 
        }
        private int id;

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

        public bool IsPaidByCreditCard
        {
            get
            {
                return isPaidByCreditCard;
            }
            set
            {
                isPaidByCreditCard = value;
                OnPropertyChanged("IsPaidByCreditCard");
            }
        }
        private bool isPaidByCreditCard;

        public bool IsPrivate
        {
            get
            {
                return isPrivate;
            }
            set
            {
                isPrivate = value;
                OnPropertyChanged("IsPrivate");
            }
        }
        private bool isPrivate;

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

        public SaleProduct(int id, int productId, string name, double price, bool isPaidByCreditCard, bool isPrivate, string deduction, double sellPrice) 
        {
            this.Id = id;
            this.ProductId = productId;
            this.Name = name;
            this.Price = price;
            this.IsPaidByCreditCard = isPaidByCreditCard;
            this.IsPrivate = isPrivate;
            this.Deduction = deduction;
            this.SellPrice = sellPrice;
        }

        public SaleProduct()
        {
            isPaidByCreditCard = false;
            isPrivate = false;
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
