using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public class StatisticViewModel : INotifyPropertyChanged, Observer
    {
        public ObservableCollection<Sale> Sales
        {
            get { return sales; }
        }
        private ObservableCollection<Sale> sales;

        private string saleFilename;
        private DataAccess.DataAccess dataAccess;

        public StatisticViewModel()
        {
            dataAccess = DataAccess.DataAccess.Instance;
            dataAccess.Attach(this);

            sales = new ObservableCollection<Sale>(dataAccess.Sales);
        }

        

        public void Udpate()
        {
            sales = new ObservableCollection<Sale>(dataAccess.Sales);
            OnPropertyChanged("Sales");
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
