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
using KioskVerwaltung.BusinessObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KioskVerwaltung.Printing
{
    /// <summary>
    /// Interaktionslogik für SaleViewPage.xaml
    /// </summary>
    public partial class SaleViewPrintPage : UserControl, INotifyPropertyChanged
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
        public int PageNumber
        {
            get { return pageNr; }
        }
        private int pageNr;

        public SaleViewPrintPage(IList<SaleProduct> saleProducts, Size pageSize, int pageNr)
        {
            saleToday = new KioskVerwaltung.BusinessObjects.Sale();
            saleToday.SaleProducts = new System.Collections.ObjectModel.ObservableCollection<SaleProduct>(saleProducts);
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
