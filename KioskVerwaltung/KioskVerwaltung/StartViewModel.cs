using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public class StartViewModel : INotifyPropertyChanged, Observer
    {
        public ObservableCollection<Product> ExpiringProducts
        {
            get { return expiringProducts; }
            set
            {
                expiringProducts = value;
                OnPropertyChanged("ExpiringProducts");
            }
        }
        public ObservableCollection<Product> ProductsShortInStock
        {
            get { return productsShortInStock; }
            set
            {
                productsShortInStock = value;
                OnPropertyChanged("ProductsShortInStock");
            }
        }

        private ObservableCollection<Product> expiringProducts;
        private ObservableCollection<Product> productsShortInStock;

        private TimeSpan expiringTimeSpan = new TimeSpan(7, 0, 0, 0, 0);
        private int MinStockValue = 5;

        private ObservableCollection<Product> products;
        private DataAccess.DataAccess dataAccess;
        private String kioskFilename;
        
        public StartViewModel()
        {
            dataAccess = DataAccess.DataAccess.Instance;
            dataAccess.Attach(this);
            
            kioskFilename = Properties.Settings.Default.Filename;

            expiringProducts = new ObservableCollection<Product>();
            productsShortInStock = new ObservableCollection<Product>();

            Udpate();
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
            UpdateExpiringProducts();
            UpdateProductsShortInStock();
        }

        private void UpdateExpiringProducts()
        {
            expiringProducts.Clear();
            foreach (var product in products)
            {
                if (product.HasExpirationDate)
                {
                    Product expiringProduct = new Product(product.Id, product.Name, product.Barcode, product.HasExpirationDate, product.Price, null);
                    List<Consignment> expiringConsignments = new List<Consignment>();

                    foreach (var consignment in product.Consignments)
                    {
                        if (consignment.ExpirationDate - DateTime.Now <= expiringTimeSpan)
                        {
                            expiringConsignments.Add(consignment);
                        }
                    }

                    if (expiringConsignments.Count > 0)
                    {
                        expiringProduct.Consignments = expiringConsignments;
                        expiringProducts.Add(expiringProduct);
                    }
                }
            }

            OnPropertyChanged("ExpiringProducts");
        }
        private void UpdateProductsShortInStock()
        {
            productsShortInStock.Clear();
            foreach (var product in products)
            {
                if (product.Stock <= MinStockValue)
                {
                    productsShortInStock.Add(product);
                }
            }
            OnPropertyChanged("ProductsShortInStock");
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
