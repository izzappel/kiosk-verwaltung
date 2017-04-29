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
    /// Interaktionslogik für SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private SettingsViewModel viewModel;

        public SettingsView()
        {
            viewModel = new SettingsViewModel();
            DataContext = viewModel;

            InitializeComponent();
        }

        private void EditBarcodeSetting(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            BarcodeSetting barcodeSetting = button.CommandParameter as BarcodeSetting;

            EditBarcodeSettingWindow editBarcodeSettingWindow = new EditBarcodeSettingWindow(barcodeSetting);
            if (editBarcodeSettingWindow.ShowDialog() == true)
            {
                viewModel.EditBarcodeSetting(editBarcodeSettingWindow.BarcodeSetting);
            }
        }

        private void DeleteBarcodeSetting(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            BarcodeSetting barcodeSetting = button.CommandParameter as BarcodeSetting;

            MessageBoxResult result = MessageBox.Show("Wollen Sie diese Konfiguration " + barcodeSetting.Name + " (Barcode=" + barcodeSetting.Barcode + ") wirklich löschen?", "Barcode-Konfiguration löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                viewModel.RemoveBarcodeSetting(barcodeSetting);
            }
        }

        private void AddBarcodeSetting(object sender, RoutedEventArgs e)
        {
            AddBarcodeSettingWindow addBarcodeSettingWindow = new AddBarcodeSettingWindow();
            if (addBarcodeSettingWindow.ShowDialog() == true)
            {
                viewModel.AddBarcodeSetting(addBarcodeSettingWindow.BarcodeSetting);
            }
        }
    }
}
