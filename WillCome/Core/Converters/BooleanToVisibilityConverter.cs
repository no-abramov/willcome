using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace WillCome.Core.Converters
{
    // Конвертер для преобразования логического значения в видимость (Visible/Collapsed)
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Преобразует логическое значение в значение видимости (Visibility).
        /// </summary>
        /// <param name="value">Булево значение, определяющее видимость.</param>
        /// <param name="targetType">Целевой тип (обычно Visibility).</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Информация о культуре.</param>
        /// <returns>Visible, если значение true, иначе Collapsed.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Преобразует значение видимости обратно в булево значение.
        /// </summary>
        /// <param name="value">Значение Visibility для преобразования.</param>
        /// <param name="targetType">Тип целевого значения (обычно bool).</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Информация о культуре.</param>
        /// <returns>True, если значение Visibility.Visible, иначе false.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }

        #endregion
    }
}
