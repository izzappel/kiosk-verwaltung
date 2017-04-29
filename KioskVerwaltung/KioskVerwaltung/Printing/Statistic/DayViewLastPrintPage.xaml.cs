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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KioskVerwaltung.Printing
{
    /// <summary>
    /// Interaktionslogik für DayViewLastPrintPage.xaml
    /// </summary>
    public partial class DayViewLastPrintPage : UserControl, INotifyPropertyChanged
    {
        public DayStatisticViewModel ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
                OnPropertyChanged("ViewModel");
            }
        }
        private DayStatisticViewModel viewModel;
        public ObservableCollection<TotalSaleProduct> TotalSaleProducts
        {
            get { return saleProducts; }
            set
            {
                saleProducts = value;
                OnPropertyChanged("TotalSaleProducts");
            }
        }
        private ObservableCollection<TotalSaleProduct> saleProducts;
        private readonly int pageNr;

        public DayViewLastPrintPage(DateTime date, IList<TotalSaleProduct> saleProducts, IList<TotalSaleProduct> allSaleProducts, Size pageSize, int pageNr)
        {
            this.saleProducts = new ObservableCollection<TotalSaleProduct>(saleProducts);
            viewModel = new DayStatisticViewModel();
            viewModel.Day = date;
            viewModel.TotalSaleProducts = new System.Collections.ObjectModel.ObservableCollection<TotalSaleProduct>(allSaleProducts);
            
            DataContext = this;

            InitializeComponent();
            this.pageNr = pageNr;
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
