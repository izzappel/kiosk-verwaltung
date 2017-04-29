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
using Microsoft.Win32;
using System.Security;
using System.Xml;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    /// <summary>
    /// Interaktionslogik für StartView.xaml
    /// </summary>
    public partial class StartView : UserControl
    {
        public StartViewModel viewModel;

        public StartView()
        {
            viewModel = new StartViewModel();
            DataContext = viewModel;

            InitializeComponent();
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
