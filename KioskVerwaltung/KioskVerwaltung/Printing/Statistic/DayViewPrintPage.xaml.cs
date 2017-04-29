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

namespace KioskVerwaltung.Printing
{
    /// <summary>
    /// Interaktionslogik für DayViewPage.xaml
    /// </summary>
    public partial class DayViewPrintPage : UserControl
    {
        private DayStatisticViewModel viewModel;
        private readonly IList<TotalSaleProduct> saleProducts;
        private readonly int pageNr;

        public DayViewPrintPage(DateTime date, IList<TotalSaleProduct> saleProducts, Size pageSize, int pageNr)
        {
            viewModel = new DayStatisticViewModel();
            viewModel.Day = date;
            viewModel.TotalSaleProducts = new System.Collections.ObjectModel.ObservableCollection<TotalSaleProduct>(saleProducts);
            DataContext = viewModel;

            InitializeComponent();
            this.saleProducts = saleProducts;
            this.pageNr = pageNr;
            this.RenderSize = pageSize;
        }
    }
}
