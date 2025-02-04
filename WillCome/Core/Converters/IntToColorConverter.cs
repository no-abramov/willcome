using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WillCome.Core.Converters
{
    // Конвертер для преобразования целого числа в цвет
    public class IntToColorConverter : IValueConverter
    {
        #region Fields

        // Набор предопределённых цветов
        private static readonly Brush[] ColorBrushes =
        {
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D6A7")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EE6767")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81C784")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8AC5CE")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4FC3F7")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F879BE")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B39DDB")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81D4FA")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9575CD")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F48FB1")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5A7A1")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F06292")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB74D")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8A65")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD54F")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5A5A6")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C2C3C5")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D4DADE")),
        };

        #endregion

        #region Methods

        /// <summary>
        /// Преобразует целое число в цветовую кисть.
        /// </summary>
        /// <param name="value">Целое число, используемое как индекс массива цветов.</param>
        /// <param name="targetType">Целевой тип (обычно Brush).</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Информация о культуре.</param>
        /// <returns>Brush, соответствующий числу, либо чёрный цвет, если индекс невалиден.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && intValue >= 0 && intValue < ColorBrushes.Length)
            {
                return ColorBrushes[intValue];
            }
            return Brushes.Black;
        }

        /// <summary>
        /// Обратное преобразование не реализовано.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
