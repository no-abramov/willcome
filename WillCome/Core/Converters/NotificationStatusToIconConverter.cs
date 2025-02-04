using System.Globalization;
using System;
using System.Windows.Data;

namespace WillCome.Core.Converters
{
    // Конвертер для преобразования статуса уведомления в иконку
    public class NotificationStatusToIconConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Преобразует статус уведомления в соответствующую иконку из ресурсов приложения.
        /// </summary>
        /// <param name="value">Статус уведомления (NotificationStatusEnum).</param>
        /// <param name="targetType">Целевой тип (например, DrawingImage).</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Информация о культуре.</param>
        /// <returns>Иконка, соответствующая статусу уведомления.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WillCome.Core.Enums.NotificationStatusEnum status)
            {
                switch (status)
                {
                    case WillCome.Core.Enums.NotificationStatusEnum.Success:
                        return System.Windows.Application.Current.FindResource("di_ic_success_xaml");
                    case WillCome.Core.Enums.NotificationStatusEnum.Mistake:
                        return System.Windows.Application.Current.FindResource("di_ic_mistake_xaml");
                    case WillCome.Core.Enums.NotificationStatusEnum.Message:
                    default:
                        return System.Windows.Application.Current.FindResource("di_ic_message_xaml");
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
