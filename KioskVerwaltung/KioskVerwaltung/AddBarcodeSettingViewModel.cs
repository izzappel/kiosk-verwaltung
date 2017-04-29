using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public class AddBarcodeSettingViewModel: INotifyPropertyChanged
    {
        public BarcodeSetting BarcodeSetting
        {
            get { return barcodeSetting; }
            set { barcodeSetting = value; }
        }
        private BarcodeSetting barcodeSetting;

        public AddBarcodeSettingViewModel()
        {
            barcodeSetting = new BarcodeSetting();
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
