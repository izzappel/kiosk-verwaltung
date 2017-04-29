using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public class StockViewModel :  Observer, INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set 
            { 
                products = value;
                OnPropertyChanged("Products");
            }
        }

        private ObservableCollection<Product> products;
        private DataAccess.DataAccess dataAccess;

        public StockViewModel()
        {
            dataAccess = DataAccess.DataAccess.Instance;
            dataAccess.Attach(this);

            Udpate();
        }

        public void AddProduct(Product product)
        {
            dataAccess.AddProduct(product);
        }
        public void EditProduct(Product product)
        {
            dataAccess.EditProduct(product);
        }
        public void RemoveProduct(Product product)
        {
            dataAccess.RemoveProduct(product.Id);
        }

        public void AddConsigment(Product product, Consignment consignment)
        {
            dataAccess.AddConsigment(product.Id, consignment);
        }
        public void EditConsignment(Product product, Consignment consignment)
        {
            dataAccess.EditConsignment(product.Id, consignment);
        }
        public void RemoveConsignment(Consignment consignment)
        {
            Product product = GetProductFromConsignment(consignment);
            dataAccess.RemoveConsignment(product.Id, consignment.Id);
        }

        public Product GetProductFromConsignment(Consignment consignment)
        {
            foreach (var product in products)
            {
                if (product.Consignments.Contains(consignment)) return product;
            }
            return null;
        }

        public void Udpate()
        {
            products = new ObservableCollection<Product>(dataAccess.Products);
            OnPropertyChanged("Products");
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
