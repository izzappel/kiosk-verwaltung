using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Printing
{
    public class BarcodeSettingsPageViewModel : INotifyPropertyChanged
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

        private DataAccess.DataAccess dataAccess;

        public BarcodeSettingsPageViewModel()
        {
            dataAccess = DataAccess.DataAccess.Instance;
            BarcodeSettings = new ObservableCollection<BarcodeSetting>(dataAccess.BarcodeSettings);
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
