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
    /// Interaktionslogik für AddConsignmentWindow.xaml
    /// </summary>
    public partial class AddConsignmentWindow : Window
    {
        public Consignment Consignment
        {
            get { return viewModel.Consignment; }
        }

        public Product Product
        {
            get { return viewModel.Product; }
            set { viewModel.Product = value; }
        }

        AddConsignmentViewModel viewModel;

        public AddConsignmentWindow()
        {
            viewModel = new AddConsignmentViewModel();
            this.DataContext = viewModel;

            InitializeComponent();

            viewModel.PropertyChanged += ProprtyChanged;
            if (!viewModel.Product.HasExpirationDate)
            {
                ExpirationDateLabel.Visibility = System.Windows.Visibility.Collapsed;
                ExpirationDatePicker.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (!viewModel.Product.HasConsignmentPrice)
            {
                PriceLabel.Visibility = System.Windows.Visibility.Collapsed;
                PriceTextBox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void ProprtyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Product"))
            {
                if (!viewModel.Product.HasExpirationDate)
                {
                    ExpirationDateLabel.Visibility = System.Windows.Visibility.Collapsed;
                    ExpirationDatePicker.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    ExpirationDateLabel.Visibility = System.Windows.Visibility.Visible;
                    ExpirationDatePicker.Visibility = System.Windows.Visibility.Visible;
                }
                if (!viewModel.Product.HasConsignmentPrice)
                {
                    PriceLabel.Visibility = System.Windows.Visibility.Collapsed;
                    PriceTextBox.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    PriceLabel.Visibility = System.Windows.Visibility.Visible;
                    PriceTextBox.Visibility = System.Windows.Visibility.Visible;
                }
            }

        }

        private void SaveConsignment(object sender, RoutedEventArgs e)
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
