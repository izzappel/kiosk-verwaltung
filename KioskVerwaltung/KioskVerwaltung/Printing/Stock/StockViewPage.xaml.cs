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

namespace KioskVerwaltung.Printing
{
    /// <summary>
    /// Interaktionslogik für StockViewPage.xaml
    /// </summary>
    public partial class StockViewPage : UserControl
    {
        private StockViewModel viewModel;
        private readonly IList<Product> products;
        private readonly int pageNr;

        public StockViewPage(IList<Product> products, Size pageSize, int pageNr, bool isLast)
        {
            viewModel = new StockViewModel();
            viewModel.Products = new System.Collections.ObjectModel.ObservableCollection<Product>(products);
            DataContext = viewModel;

            InitializeComponent();
            this.products = products;
            this.pageNr = pageNr;
            this.RenderSize = pageSize;
        }
    }
}
