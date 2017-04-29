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
using System.Globalization;
using System.ComponentModel;

namespace KioskVerwaltung.Printing
{
    public partial class MonthViewPrintPage : UserControl, INotifyPropertyChanged
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
        private readonly IList<TotalSaleProduct> saleProducts;
        public int PageNumber
        {
            get { return pageNr; }
        }
        private int pageNr;
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

        public MonthViewPrintPage(string month, IList<TotalSaleProduct> saleProducts, Size pageSize, int pageNr)
        {
            viewModel = new MonthStatisticViewModel();
            viewModel.TotalSaleProducts = new System.Collections.ObjectModel.ObservableCollection<TotalSaleProduct>(saleProducts);
            DataContext = this;

            InitializeComponent();

            this.pageNr = pageNr + 1;
            this.Month = month;
            this.saleProducts = saleProducts;
            this.RenderSize = pageSize;

            PageNumberTextBlock.Text = PageNumber.ToString();
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
