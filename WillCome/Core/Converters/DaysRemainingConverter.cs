using System;
using System.Globalization;
using System.Windows.Data;

namespace WillCome.Core.Converters
{
    // Конвертер для вычисления количества дней, прошедших с заданной даты
    public class DaysRemainingConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Преобразует дату в строку с описанием, сколько дней прошло с этой даты.
        /// </summary>
        /// <param name="value">Дата, с которой вычисляется разница.</param>
        /// <param name="targetType">Целевой тип (обычно строка).</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Информация о культуре.</param>
        /// <returns>
        /// "Сегодня", если дата — текущий день;
        /// "Вчера", если дата была вчера;
        /// или количество дней в формате "N дней", "N дня" или "N день".
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime createDate)
            {
                DateTime now = DateTime.Now;
                TimeSpan difference = now - createDate;
                int days = (int)Math.Floor(difference.TotalDays);

                if (days == 0)
                {
                    return "Сегодня";
                }
                else if (days == 1)
                {
                    return "Вчера";
                }
                else
                {
                    string suffix = days % 10 == 1 && days % 100 != 11 ? "день" :
                                    (days % 10 >= 2 && days % 10 <= 4 && (days % 100 < 10 || days % 100 >= 20)) ? "дня" : "дней";
                    return $"{days} {suffix}";
                }
            }
            return "Нет данных";
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
