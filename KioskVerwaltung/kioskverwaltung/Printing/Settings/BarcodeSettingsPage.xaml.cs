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

namespace KioskVerwaltung.Printing
{
    /// <summary>
    /// Interaktionslogik für BarcodeSettingsPage.xaml
    /// </summary>
    public partial class BarcodeSettingsPage : UserControl
    {
        private BarcodeSettingsPageViewModel viewModel;
        private readonly IList<BarcodeSetting> barcodeSettings;
        private readonly int pageNr;

        public BarcodeSettingsPage(IList<BarcodeSetting> barcodeSettings, Size pageSize, int pageNr, bool isLast)
        {
            viewModel = new BarcodeSettingsPageViewModel();
            viewModel.BarcodeSettings = new System.Collections.ObjectModel.ObservableCollection<BarcodeSetting>(barcodeSettings);
            DataContext = viewModel;

            InitializeComponent();
            this.barcodeSettings = barcodeSettings;
            this.pageNr = pageNr;
            this.RenderSize = pageSize;
        }
    }
}
