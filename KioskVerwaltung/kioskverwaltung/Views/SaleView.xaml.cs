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
using KioskVerwaltung.BusinessObjects;
using KioskVerwaltung.Printing;
using System.Printing;
using System.Windows.Xps;

namespace KioskVerwaltung
{
    /// <summary>
    /// Interaktionslogik für SaleView.xaml
    /// </summary>
    public partial class SaleView : UserControl
    {
        SaleViewModel viewModel;
        private string inputText = string.Empty;

        public SaleView()
        {
            viewModel = new SaleViewModel();
            DataContext = viewModel;

            InitializeComponent();
        }

        private void PayByCreditCardCash(object sender, RoutedEventArgs e)
        {
            viewModel.SetIsBasketPaidByCreditCard(!viewModel.IsBasketPaidByCreditCard);
            InvokersListBox.Focus();
        }
        private void PrivatePublicSale(object sender, RoutedEventArgs e)
        {
            viewModel.SetIsBasketPrivate(!viewModel.IsBasketPrivate);
            InvokersListBox.Focus();
        }
        private void RemoveSaleProduct(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            BasketItem basketItem = button.CommandParameter as BasketItem;

            viewModel.RemoveSaleProduct(basketItem);
            InvokersListBox.Focus();
        }
        private void ClearBasket(object sender, RoutedEventArgs e)
        {
            viewModel.ClearBasket();
            InvokersListBox.Focus();
        }
        private void PrintDailyAccounting(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PrintQueue printQueue = printDialog.PrintQueue;
                XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
                IDocumentPaginatorSource document = new SalePrintFlowDocument(viewModel.SaleToday.SaleProducts);
                xpsDocumentWriter.Write(document.DocumentPaginator);
            }

            InvokersListBox.Focus();
        }


        private void AddProductToBasket(object sender, RoutedEventArgs e)
        {
            viewModel.ScanProduct(inputText);
            inputText = string.Empty;
            BarcodeTextBox.Text = inputText;
        }


        private void Scan(object sender, TextCompositionEventArgs e)
        {
            if (!e.Text.Equals(" "))
            {
                if (e.Text.Equals("\b") && inputText.Length > 0) //backspace
                {
                    inputText = inputText.Substring(0, inputText.Length - 1);
                }
                else
                {
                    inputText += e.Text;
                }
            }
            else
            {
                viewModel.ScanProduct(inputText);
                inputText = string.Empty;
            }
            BarcodeTextBox.Text = inputText;
        }
        private void ScanPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                viewModel.ClearBasket();
                inputText = string.Empty;
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                viewModel.SetIsBasketPaidByCreditCard(!viewModel.IsBasketPaidByCreditCard);
                inputText = string.Empty;
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                viewModel.SetIsBasketPrivate(!viewModel.IsBasketPrivate);
                inputText = string.Empty;
                e.Handled = true;
            }

#if DEBUG
            //only for debug
            if (e.Key == Key.D)
            {
                viewModel.ScanProduct(inputText);
                inputText = string.Empty;
                e.Handled = true;
            }
#endif
            BarcodeTextBox.Text = inputText;
            InvokersListBox.Focus();
        }
        public void CleanForClosing()
        {
            viewModel.CleanForClosing();
        }
        private void ManualPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }


        private void SetPrivateSale(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            SaleProduct saleProduct = menuItem.CommandParameter as SaleProduct;

            viewModel.SetIsPrivateToSaleProduct(true, saleProduct);
        }
        private void SetPublicSale(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            SaleProduct saleProduct = menuItem.CommandParameter as SaleProduct;

            viewModel.SetIsPrivateToSaleProduct(false, saleProduct);
        }
        private void SetCreditCardSale(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            SaleProduct saleProduct = menuItem.CommandParameter as SaleProduct;

            viewModel.SetIsPaidByCreditCardToSaleProduct(true, saleProduct);
        }
        private void SetCashSale(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            SaleProduct saleProduct = menuItem.CommandParameter as SaleProduct;

            viewModel.SetIsPaidByCreditCardToSaleProduct(false, saleProduct);
        }


        private void DeleteSaleProduct(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            SaleProduct saleProduct = menuItem.CommandParameter as SaleProduct;

            MessageBoxResult result = MessageBox.Show("Wollen Sie das Produkt \"" + saleProduct.Name + "\" wirklich aus dem Verkauf entfernen?", "Produkt entfernen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                viewModel.DeleteSaleProduct(saleProduct);
                MessageBox.Show("Produkt \"" + saleProduct.Name + "\" erfolgreich entfernt. Bitte passen Sie die Mengen in Ihrem Vorrat an.", "Produkt entfernen", MessageBoxButton.OK);
            }
        }
    }
}
