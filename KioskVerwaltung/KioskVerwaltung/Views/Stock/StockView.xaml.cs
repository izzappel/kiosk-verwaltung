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
using KioskVerwaltung.Printing;
using System.Printing;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

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

            Product editableProduct = new Product(product.Id, product.Name, product.Barcode, product.HasExpirationDate, product.HasConsignmentPrice, product.Price, product.Consignments);

            EditProductWindow editProductWindow = new EditProductWindow(editableProduct);
            if (editProductWindow.ShowDialog() == true)
            {
                viewModel.EditProduct(editProductWindow.Product);
            }
        }

        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var product = ((ListBoxItem)sender).Content as Product;
            if (product != null)
            {
                Product editableProduct = new Product(product.Id, product.Name, product.Barcode, product.HasExpirationDate, product.HasConsignmentPrice, product.Price, product.Consignments);

                EditProductWindow editProductWindow = new EditProductWindow(editableProduct);
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
            Consignment consignment = button.CommandParameter as Consignment;

            Product product = viewModel.GetProductFromConsignment(consignment);

            Consignment editableConsignment = new Consignment(consignment.Id, consignment.NumberOfContent, consignment.ExpirationDate, consignment.Price);
            EditConsignmentWindow editConsignmentWindow = new EditConsignmentWindow(product, editableConsignment);
            if (editConsignmentWindow.ShowDialog() == true)
            {
                viewModel.EditConsignment(editConsignmentWindow.Product, editConsignmentWindow.Consignment);
            }
        }
        private void RemoveConsignment(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            Consignment consignment = button.CommandParameter as Consignment;

            MessageBoxResult result = MessageBox.Show("Wollen Sie die Lieferung (Menge=" + consignment.NumberOfContent + ") wirklich löschen?", "Lierferung löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                viewModel.RemoveConsignment(consignment);
            }
        }
        private void FreeConsignment(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            Consignment consignment = button.CommandParameter as Consignment;

            MessageBoxResult result = MessageBox.Show("Wollen Sie die Lieferung (Menge=" + consignment.NumberOfContent + ") wirklich gratis rausgeben?", "Lierferung gratis rausgeben", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                viewModel.FreeConsignment(consignment);
            }
        }

        private void PrintStock(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PrintQueue printQueue = printDialog.PrintQueue;
                XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
                IDocumentPaginatorSource document = new StockPrintFlowDocument(viewModel.Products);
                xpsDocumentWriter.Write(document.DocumentPaginator);
            }
        }

        private void PrintSelectedStock(object sender, RoutedEventArgs e)
        {
            System.Collections.IList items = (System.Collections.IList)ProductListBox.SelectedItems;
            var collection = items.Cast<Product>();


            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PrintQueue printQueue = printDialog.PrintQueue;
                XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
                IDocumentPaginatorSource document = new StockPrintFlowDocument(collection.ToList<Product>());
                xpsDocumentWriter.Write(document.DocumentPaginator);
            }
        }

        private void SelectDeselectAll(object sender, RoutedEventArgs e)
        {
            System.Collections.IList items = (System.Collections.IList)ProductListBox.SelectedItems;
            if (items.Count < ProductListBox.Items.Count)
            {
                ProductListBox.SelectAll();
            }
            else
            {
                ProductListBox.SelectedItems.Clear();
            }
            ProductListBox.Focus();
        }
    }
}
