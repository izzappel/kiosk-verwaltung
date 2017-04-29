using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Printing
{
    /// <summary>
    /// Interaktionslogik für SaleViewLastPrintPage.xaml
    /// </summary>
    public partial class SaleViewLastPrintPage : UserControl, INotifyPropertyChanged
    {       
        public KioskVerwaltung.BusinessObjects.Sale SaleToday
        {
            get { return saleToday; }
            set
            {
                saleToday = value;
                OnPropertyChanged("SaleToday");
            }
        }
        private KioskVerwaltung.BusinessObjects.Sale saleToday;

        public KioskVerwaltung.BusinessObjects.Sale SaleTodayTotal
        {
            get { return saleTodayTotal; }
            set
            {
                saleTodayTotal = value;
                OnPropertyChanged("SaleTodayTotal");
            }
        }
        private KioskVerwaltung.BusinessObjects.Sale saleTodayTotal;

        public int PageNumber
        {
            get { return pageNr; }
        }
        private int pageNr;

        public SaleViewLastPrintPage(IList<SaleProduct> saleProducts, IList<SaleProduct> saleProductsTotal, Size pageSize, int pageNr)
        {
            saleToday = new Sale();
            saleToday.SaleProducts = new System.Collections.ObjectModel.ObservableCollection<SaleProduct>(saleProducts);
            saleTodayTotal = new Sale();
            saleTodayTotal.SaleProducts = new System.Collections.ObjectModel.ObservableCollection<SaleProduct>(saleProductsTotal);
            DataContext = this;
            
            this.pageNr = pageNr + 1;

            InitializeComponent();
            
            this.RenderSize = pageSize;
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
