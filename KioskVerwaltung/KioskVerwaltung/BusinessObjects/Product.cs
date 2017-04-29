using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KioskVerwaltung.BusinessObjects
{
    public class Product : INotifyPropertyChanged
    {
        public int Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }
        private int id;
        public string Name  { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        private string name;
        public string Barcode { get { return barcode; } set { barcode = value; OnPropertyChanged("Barcode"); } }
        private string barcode;
        public bool HasExpirationDate { get { return hasExpirationDate; } set { hasExpirationDate = value; OnPropertyChanged("HasExpirationDate"); } }
        private bool hasExpirationDate;
        public System.Windows.Visibility ExpirationDateVisibility
        {
            get
            {
                if (HasExpirationDate)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }
        }
        public bool HasConsignmentPrice { get { return hasConsignmentPrice; } set { hasConsignmentPrice = value; OnPropertyChanged("HasConsignmentPrice"); } }
        private bool hasConsignmentPrice;
        public System.Windows.Visibility ConsignmentPriceVisibility 
        {
            get
            {
                if (HasConsignmentPrice)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }
        }
        public double Price { get { return price; } set { price = value; OnPropertyChanged("Price"); } }
        private double price;

        public int Stock 
        {
            get { return CalculateStock(); }
        }

        public double TotalPrice
        {
            get { return CalculateTotalPrice(); }
        }

        public IList<Consignment> Consignments { get; set; }

        public Product()
        {
            Consignments = new List<Consignment>();
        }

        public Product(int id, string name, string barcode, bool hasExpirationDate, bool hasConsignmentPrice, double price, IList<Consignment> consignments)
        {
            this.Id = id;
            this.Name = name;
            this.Barcode = barcode;
            this.HasExpirationDate = hasExpirationDate;
            this.HasConsignmentPrice = hasConsignmentPrice;
            this.Price = price;
            this.Consignments = consignments;
        }

        private int CalculateStock()
        {
            int stock = 0;
            foreach (var consignment in Consignments)
            {
                stock += consignment.NumberOfContent;
            }
            return stock;
        }

        private double CalculateTotalPrice()
        {
            if (Stock <= 0) return 0;
            double totalPrice = Stock * price;
            if (hasConsignmentPrice)
            {
                totalPrice = 0;
                foreach (var consignment in Consignments)
                {
                    totalPrice += consignment.NumberOfContent * consignment.Price;
                }
            }
            return totalPrice;
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
