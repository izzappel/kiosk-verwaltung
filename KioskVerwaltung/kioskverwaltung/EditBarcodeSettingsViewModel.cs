using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public class EditBarcodeSettingsViewModel : INotifyPropertyChanged
    {
        public BarcodeSetting BarcodeSetting
        {
            get { return barcodeSetting; }
            set { barcodeSetting = value; }
        }
        private BarcodeSetting barcodeSetting;

        public EditBarcodeSettingsViewModel(BarcodeSetting barcodeSetting)
        {
            this.barcodeSetting = barcodeSetting;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
