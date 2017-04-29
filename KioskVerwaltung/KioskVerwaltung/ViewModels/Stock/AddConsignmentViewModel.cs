using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using KioskVerwaltung.BusinessObjects;
using System.Collections.ObjectModel;

namespace KioskVerwaltung
{
    public class AddConsignmentViewModel : INotifyPropertyChanged
    {
        public Consignment Consignment 
        {
            get { return consignment; }
            set
            {
                consignment = value;
                OnPropertyChanged("Consignment");
            }
        }
        private Consignment consignment;

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

        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
                OnPropertyChanged("Product");
            }
        }
        private Product product;

        private DataAccess.DataAccess dataAccess;

        public AddConsignmentViewModel()
        {
            dataAccess = DataAccess.DataAccess.Instance;
            products = new ObservableCollection<Product>(dataAccess.Products);
            product = new Product();
            consignment = new Consignment();
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
