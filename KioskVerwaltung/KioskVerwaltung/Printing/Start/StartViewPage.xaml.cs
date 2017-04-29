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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace KioskVerwaltung.Printing.Start
{
    /// <summary>
    /// Interaktionslogik für StartViewPage.xaml
    /// </summary>
    public partial class StartViewPage : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<Product> ExpiringProducts
        {
            get { return expiringProducts; }
            set
            {
                expiringProducts = value;
                OnPropertyChanged("ExpiringProducts");
            }
        }
        public ObservableCollection<Product> ShortInStockProducts
        {
            get { return shortInStockProducts; }
            set
            {
                shortInStockProducts = value;
                OnPropertyChanged("ShortInStockProducts");
            }
        }
        private ObservableCollection<Product> expiringProducts;
        private ObservableCollection<Product> shortInStockProducts;
        private Size pageSize;


        public StartViewPage(IList<Product> expiringProducts, IList<Product> shortInStockProducts, Size pageSize)
        {
            this.expiringProducts = new ObservableCollection<Product>(expiringProducts);
            this.shortInStockProducts = new ObservableCollection<Product>(shortInStockProducts);
            this.pageSize = pageSize;

            DataContext = this;

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
