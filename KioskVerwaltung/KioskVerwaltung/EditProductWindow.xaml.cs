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
using System.Windows.Shapes;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    /// <summary>
    /// Interaktionslogik für EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        EditProductViewModel viewModel;

        public Product Product
        {
            get { return viewModel.Product; }
            set { viewModel.Product = value; }
        }

        public EditProductWindow(Product product)
        {
            viewModel = new EditProductViewModel(product);
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void SaveProduct(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
