using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using Theme.Core.Converts.Utils;

namespace Theme.Core.Converts
{
    public class BooleanToVisibilityConverter : ConverterMarkupExtension<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            var isVisible = true;
            if (value is bool)
                isVisible = (bool)value;

            var stringParam = parameter?.ToString() ?? string.Empty;
            if (stringParam.IndexOf("inverse", StringComparison.OrdinalIgnoreCase) >= 0)
                isVisible = isVisible == false;

            return isVisible ? Visibility.Visible : stringParam.IndexOf("hidden", StringComparison.OrdinalIgnoreCase) >= 0 ? Visibility.Hidden : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
