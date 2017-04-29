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
    /// Interaktionslogik für EditBarcodeSettingWindow.xaml
    /// </summary>
    public partial class EditBarcodeSettingWindow : Window
    {
        public BarcodeSetting BarcodeSetting
        {
            get { return viewModel.BarcodeSetting; }
        }

        private EditBarcodeSettingsViewModel viewModel;

        public EditBarcodeSettingWindow(BarcodeSetting barcodeSetting)
        {
            viewModel = new EditBarcodeSettingsViewModel(barcodeSetting);
            DataContext = viewModel;

            InitializeComponent();
        }

        private void SaveBarcodeSetting(object sender, RoutedEventArgs e)
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
