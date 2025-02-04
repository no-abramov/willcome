using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WillCome.Core.Converters
{
    // Конвертер для преобразования целого числа в иконку из ресурсов приложения
    public class IntToIconConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Преобразует целое число в иконку, используя ресурсы приложения.
        /// </summary>
        /// <param name="value">Идентификатор иконки (целое число).</param>
        /// <param name="targetType">Целевой тип (обычно DrawingImage).</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Информация о культуре.</param>
        /// <returns>DrawingImage из ресурсов приложения, соответствующий переданному идентификатору.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int iconId)
            {
                switch (iconId)
                {
                    case 0:
                        return App.Current.FindResource("di_ic_menu_dashboard_xaml") as DrawingImage;
                    case 1:
                        return App.Current.FindResource("di_ic_menu_deal_xaml") as DrawingImage;
                    case 2:
                        return App.Current.FindResource("di_ic_menu_taskcases_xaml") as DrawingImage;
                    case 3:
                        return App.Current.FindResource("di_ic_menu_base_xaml") as DrawingImage;
                    default:
                        return App.Current.FindResource("di_ic_menu_dashboard_xaml") as DrawingImage;
                }
            }
            return null;
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
