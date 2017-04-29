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

namespace KioskVerwaltung
{
    /// <summary>
    /// Interaktionslogik für StockView.xaml
    /// </summary>
    public partial class StockView : UserControl
    {
        private StockViewModel viewModel;

        public StockView()
        {
            viewModel = new StockViewModel();
            DataContext = viewModel;

            InitializeComponent();
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            NewProductWindow newProductWindow = new NewProductWindow();
            if (newProductWindow.ShowDialog() == true)
            {
                viewModel.AddProduct(newProductWindow.Product);
            }
        }
        private void EditProduct(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            Product product = button.CommandParameter as Product;

            EditProductWindow editProductWindow = new EditProductWindow(product);
            if (editProductWindow.ShowDialog() == true)
            {
                viewModel.EditProduct(editProductWindow.Product);
            }
        }
        private void EditProductFromList(object sender, MouseButtonEventArgs e)
        {
            ListBox list = e.Source as ListBox;
            Product product = list.SelectedItem as Product;

            if (product != null)
            {
                EditProductWindow editProductWindow = new EditProductWindow(product);
                if (editProductWindow.ShowDialog() == true)
                {
                    viewModel.EditProduct(editProductWindow.Product);
                }
            }
        }
        private void RemoveProduct(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            Product product = button.CommandParameter as Product;

            MessageBoxResult result = MessageBox.Show("Wollen Sie das Produkt " + product.Name + " (Barcode=" + product.Barcode + ") wirklich löschen?", "Produkt löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                viewModel.RemoveProduct(product);
            }
        }
        private void AddConsignment(object sender, RoutedEventArgs e)
        {
            AddConsignmentWindow addConsignmentWindow = new AddConsignmentWindow();

            Button button = e.Source as Button;
            if (button.CommandParameter != null)
            {
                Product product = button.CommandParameter as Product;
                addConsignmentWindow.Product = product;
            }


            if (addConsignmentWindow.ShowDialog() == true)
            {
                viewModel.AddConsigment(addConsignmentWindow.Product, addConsignmentWindow.Consignment);
            }
        }
        private void EditConsignment(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            Consignment consignmnet = button.CommandParameter as Consignment;

            Product product = viewModel.GetProductFromConsignment(consignmnet);
            EditConsignmentWindow editConsignmentWindow = new EditConsignmentWindow(product, consignmnet);
            if (editConsignmentWindow.ShowDialog() == true)
            {
                viewModel.EditConsignment(editConsignmentWindow.Product, editConsignmentWindow.Consignment);
            }
        }
        private void RemoveConsignment(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            Consignment consignmnet = button.CommandParameter as Consignment;

            MessageBoxResult result = MessageBox.Show("Wollen Sie die Lieferung (Menge=" + consignmnet.NumberOfContent + ") wirklich löschen?", "Lierferung löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                viewModel.RemoveConsignment(consignmnet);
            }
        }
    }
}
