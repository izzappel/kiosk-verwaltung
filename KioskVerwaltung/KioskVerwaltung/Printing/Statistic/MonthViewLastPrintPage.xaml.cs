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
    /// Interaktionslogik für MonthViewLastPrintPage.xaml
    /// </summary>
    public partial class MonthViewLastPrintPage : UserControl, INotifyPropertyChanged
    {
        public MonthStatisticViewModel ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
                OnPropertyChanged("ViewModel");
            }
        }
        private MonthStatisticViewModel viewModel;
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
        public string Month
        {
            get { return month; }
            set
            {
                month = value;
                OnPropertyChanged("Month");
            }
        }
        private string month;
        private readonly int pageNr;

        public MonthViewLastPrintPage(string month, IList<TotalSaleProduct> saleProducts, IList<TotalSaleProduct> allSaleProducts, Size pageSize, int pageNr)
        {
            this.saleProducts = new ObservableCollection<TotalSaleProduct>(saleProducts);
            viewModel = new MonthStatisticViewModel();
            viewModel.TotalSaleProducts = new System.Collections.ObjectModel.ObservableCollection<TotalSaleProduct>(allSaleProducts);
            DataContext = this;

            InitializeComponent();
            this.pageNr = pageNr + 1;
            this.Month = month;
            this.RenderSize = pageSize;

            PageNumberTextBlock.Text = this.pageNr.ToString();
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
