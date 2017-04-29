using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KioskVerwaltung.BusinessObjects;
using System.ComponentModel;

namespace KioskVerwaltung
{
    public class NewProductViewModel : INotifyPropertyChanged
    {
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

        public NewProductViewModel()
        {
            product = new Product();
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
