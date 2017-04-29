using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public class SettingsViewModel : Observer, INotifyPropertyChanged
    {
        public ObservableCollection<BarcodeSetting> BarcodeSettings
        {
            get { return barcodeSettings; }
            set
            {
                barcodeSettings = value;
                OnPropertyChanged("BarcodeSettings");
            }
        }
        private ObservableCollection<BarcodeSetting> barcodeSettings;


        public string ClearBasketBarcode
        {
            get { return clearBasketBarcode; }
            set
            {
                clearBasketBarcode = dataAccess.GetBarcode(value);
                Properties.Settings.Default.ClearBasketBarcode = clearBasketBarcode;
                Properties.Settings.Default.Save();
                OnPropertyChanged("ClearBasketBarcode");
            }
        }
        private string clearBasketBarcode;

        private DataAccess.DataAccess dataAccess;

        public SettingsViewModel()
        {
            clearBasketBarcode = Properties.Settings.Default.ClearBasketBarcode;

            dataAccess = DataAccess.DataAccess.Instance;
            dataAccess.Attach(this);

            Udpate();
        }

        public void EditBarcodeSetting(BarcodeSetting barcodeSetting)
        {
            dataAccess.EditBarcodeSetting(barcodeSetting);
        }

        public void RemoveBarcodeSetting(BarcodeSetting barcodeSetting)
        {
            dataAccess.RemoveBarcodeSettings(barcodeSetting.Id);
        }

        public void AddBarcodeSetting(BarcodeSetting barcodeSetting)
        {
            dataAccess.AddBarcodeSetting(barcodeSetting);
        }

        public void Udpate()
        {
            barcodeSettings = new ObservableCollection<BarcodeSetting>(dataAccess.BarcodeSettings);
            OnPropertyChanged("BarcodeSettings");
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
