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
        private void EditBarcodeSettingFromList(object sender, MouseButtonEventArgs e)
        {
            ListBox list = e.Source as ListBox;
            BarcodeSetting barcodeSetting = list.SelectedItem as BarcodeSetting;

            if (barcodeSetting != null)
            {
                BarcodeSetting editableBarcodeSetting = new BarcodeSetting(barcodeSetting.Id, barcodeSetting.Name, barcodeSetting.Value, barcodeSetting.Barcode, barcodeSetting.IsFixPrice);
                EditBarcodeSettingWindow editBarcodeSettingWindow = new EditBarcodeSettingWindow(editableBarcodeSetting);
                if (editBarcodeSettingWindow.ShowDialog() == true)
                {
                    viewModel.EditBarcodeSetting(editBarcodeSettingWindow.BarcodeSetting);
                }
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

        private void PrintBarcodeSettings(object sender, RoutedEventArgs e)
        {
            IList<BarcodeSetting> barcodes = new List<BarcodeSetting>(viewModel.BarcodeSettings);
            barcodes.Insert(0, new BarcodeSetting(0, "Warenkorb leeren", 0, viewModel.ClearBasketBarcode, false));


            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PrintQueue printQueue = printDialog.PrintQueue;
                XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
                IDocumentPaginatorSource document = new SettingsPrintFlowDocument(barcodes);
                xpsDocumentWriter.Write(document.DocumentPaginator);
            }
        }

        private void GenerateBarcode(object sender, RoutedEventArgs e)
        {
            BarcodeControl.GenerateBarcode();
        //    string data = "";
        //    int count = 6;
        //    Random random = new Random();
        //    for (int i = 0; i < count; i++)
        //    {
        //        data += random.Next(0, 9).ToString();
        //    }
        //    /////////////////////////////////////
        //    // Encode The Data
        //    /////////////////////////////////////
        //    BarcodeControl.BarcodeType = BarcodeEnum.Code39;
        //    BarcodeControl.Data = data;
        //    BarcodeControl.HasCheckDigit = true;
        // 
            
            BarcodeControl.Encode();
        }
    }
}
