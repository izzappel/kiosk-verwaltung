using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using KioskVerwaltung.BusinessObjects;
using System.Windows.Media;

namespace KioskVerwaltung.Converters
{
    public class SaleProductToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SaleProduct product = values[0] as SaleProduct;
            if (product != null)
            {
                if (product.IsPrivate) { return new SolidColorBrush(Colors.Green); }
                if (product.IsPaidByCreditCard) { return new SolidColorBrush(Colors.Blue); }
            }
            return new SolidColorBrush(Colors.Black); 
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
