using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KioskVerwaltung.BusinessObjects
{
    public class BarcodeSetting : INotifyPropertyChanged
    {
        public int Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }
        private int id;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        private string name;
        public double Value { get { return value; } set { this.value = value; OnPropertyChanged("Value"); } }
        private double value;
        public string Barcode { get { return barcode; } set { barcode = value; OnPropertyChanged("Barcode"); } }
        private string barcode;

        public bool IsFixPrice { get; set; }

        public BarcodeSetting()
        {
        }

        public BarcodeSetting(int id, string name, double value, string barcode, bool isFixPrice)
        {
            this.Id = id;
            this.Name = name;
            this.Value = value;
            this.Barcode = barcode;
            this.IsFixPrice = isFixPrice;
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
