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
            Invoker invoker = button.CommandParameter as Invoker;

            viewModel.RemoveSaleProduct(invoker);
            InvokersListBox.Focus();
        }
        private void ClearBasket(object sender, RoutedEventArgs e)
        {
            viewModel.ClearBasket();
            InvokersListBox.Focus();
        }
        private void PrintDailyAccounting(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.PrintDialog dialog = new System.Windows.Forms.PrintDialog();
            dialog.Document = viewModel.CreateDailyAccounting();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dialog.Document.Print();
            }
            InvokersListBox.Focus();
        }


        private void Scan(object sender, TextCompositionEventArgs e)
        {
            if (!e.Text.Equals(" "))
            {
                inputText += e.Text;
            }
            else
            {
                viewModel.ScanProduct(inputText);
                inputText = string.Empty;
            }
        }

        private void ScanPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                viewModel.ScanProduct(inputText);
                inputText = string.Empty;
                e.Handled = true;
            }
        }

    }
}
