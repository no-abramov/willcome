using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Theme.Core.Converts.Utils
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter where T : class, IValueConverter, new()
    {
        #region Fields

        private static T converter;

        #endregion

        #region Overrides

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new T());
        }

        #endregion

        #region IValueConverter

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion

        #region Helper Methods

        public object Convert(object value)
        {
            return Convert(value, null, null, null);
        }

        public object ConvertBack(object value)
        {
            return ConvertBack(value, null, null, null);
        }

        #endregion
    }
}
