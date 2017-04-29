using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using KioskVerwaltung.BusinessObjects;
using KioskVerwaltung.DataAccess;
using System.Collections.ObjectModel;
using System.IO;

namespace KioskVerwaltung
{
    public class MainViewModel : INotifyPropertyChanged, Observer
    {

        private DataAccess.DataAccess dataAccess;
        private string kioskFilename, saleFilename, settingsFilename;

        public MainViewModel()
        {
            dataAccess = DataAccess.DataAccess.Instance;

            kioskFilename = Properties.Settings.Default.Filename;
            saleFilename = String.Format("{0:00}_{1}.xml", DateTime.Now.Month, DateTime.Now.Year);
            settingsFilename = Properties.Settings.Default.SettingFilename;

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
