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

namespace KioskVerwaltung.Printing
{
    /// <summary>
    /// Interaktionslogik für StockItem.xaml
    /// </summary>
    public partial class StockItem : UserControl
    {
        public Product Product
        {
            get { return (Product)this.GetValue(ProductProperty); }
            set { this.SetValue(ProductProperty, value); }
        }

        public static readonly DependencyProperty ProductProperty = DependencyProperty.Register(
            "Product", typeof(Product), typeof(StockItem), new PropertyMetadata(null));

        public StockItem()
        {
            InitializeComponent();
            
        }
    }
}
