using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Theme.Core.Converts
{
    public class ValidationErrorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null && values.Length > 0 && values[0] is ValidationError error)
            {
                return error.ErrorContent?.ToString();
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
