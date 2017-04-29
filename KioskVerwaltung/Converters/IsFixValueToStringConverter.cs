using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Converters
{
    public class IsFixValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BarcodeSetting barcodeSetting = value as BarcodeSetting;
            if (barcodeSetting != null) 
            {
                if (barcodeSetting.IsFixPrice)
                {
                    return string.Format("{0:0.00} CHF", barcodeSetting.Value);
                }

                return string.Format("{0}%", barcodeSetting.Value * 100);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
