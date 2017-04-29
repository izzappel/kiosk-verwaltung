using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public delegate void UpdateSales(ObservableCollection<Sale> sales);

    public class StatisticViewModel : INotifyPropertyChanged, Observer
    {
        public event UpdateSales SalesUpdated;

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

            saleFilename = String.Format("{0:00}_{1}.xml", DateTime.Now.Month, DateTime.Now.Year);
            LoadSaleData(saleFilename);
        }

        public void LoadSaleData(string saleFilename)
        {
            this.saleFilename = saleFilename;
            sales = new ObservableCollection<Sale>(dataAccess.ReadSalesFromFile(this.saleFilename));
            OnPropertyChanged("Sales");

            if (SalesUpdated != null)
            {
                SalesUpdated(sales);
            }
        }

        public void Udpate()
        {
            LoadSaleData(saleFilename);
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
