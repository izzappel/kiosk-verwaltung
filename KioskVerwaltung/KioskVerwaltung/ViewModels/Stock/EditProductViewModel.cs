using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public class EditProductViewModel : INotifyPropertyChanged
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

        public EditProductViewModel(Product product)
        {
            this.product = product;
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
