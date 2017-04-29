using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using KioskVerwaltung.BusinessObjects;
using KioskVerwaltung.DataAccess;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Windows.Documents;
using KioskVerwaltung.Printing;

namespace KioskVerwaltung
{
    public class MainViewModel : INotifyPropertyChanged, Observer
    {

        private DataAccess.DataAccess dataAccess;
        private string kioskFilename, saleFilename, settingsFilename, kioskMonthlyBackupFilename;

        public MainViewModel()
        {
            dataAccess = DataAccess.DataAccess.Instance;

            kioskFilename = Properties.Settings.Default.Filename;
            saleFilename = String.Format("{0:00}_{1}.xml", DateTime.Now.Month, DateTime.Now.Year);
            settingsFilename = Properties.Settings.Default.SettingFilename;

            DateTime monthBefore = DateTime.Now;
            monthBefore = monthBefore.AddMonths(-1);
            kioskMonthlyBackupFilename = String.Format("{0:00}_{1}_Vorrat.xps", monthBefore.Month, monthBefore.Year);

            LoadKioskData(kioskFilename);
            LoadSaleData();
            LoadSettingsData();

            //dataAccess.AddBarcodeSetting(new BarcodeSetting(0, "fix", 7.4, "42181828", true));

            dataAccess.Attach(this);
        }

        public void LoadKioskData(string filename)
        {
            this.kioskFilename = filename;
            dataAccess.ReadKioskFile(filename);

            if (!File.Exists(kioskMonthlyBackupFilename))
            {
                var products = new ObservableCollection<Product>(dataAccess.Products);

                XpsDocument doc = new XpsDocument(kioskMonthlyBackupFilename, System.IO.FileAccess.ReadWrite);
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                IDocumentPaginatorSource document = new StockPrintFlowDocument(products);
                writer.Write(document.DocumentPaginator);
                doc.Close();
            }
        }
        public void SaveKioskData(string filename)
        {
            dataAccess.SaveKioskToFile(filename);
        }

        public void LoadSaleData()
        {
            if (File.Exists(saleFilename))
            {
                dataAccess.ReadSaleFile(saleFilename);
            }
            else
            {
                dataAccess.SaveSaleToFile(saleFilename);
            }
        }
        public void SaveSaleData()
        {
            dataAccess.SaveSaleToFile(saleFilename);
        }

        public void LoadSettingsData()
        {
            dataAccess.ReadSettingsFile(settingsFilename);
        }
        public void SaveSettingsData()
        {
            dataAccess.SaveSettingsFile(settingsFilename);
        }

        public void Close()
        {

        }

        public void Udpate()
        {
            SaveKioskData(kioskFilename);
            SaveSaleData();
            SaveSettingsData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
