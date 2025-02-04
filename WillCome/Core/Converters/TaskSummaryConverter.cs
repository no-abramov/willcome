using System;
using System.Globalization;
using System.Windows.Data;

namespace WillCome.Core.Converters
{
    // Конвертер для отображения количества задач с правильным склонением
    public class TaskSummaryConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Преобразует количество задач в строку с правильным склонением на русском языке.
        /// </summary>
        /// <param name="value">Количество задач (целое число).</param>
        /// <param name="targetType">Тип целевого значения (обычно строка).</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Информация о культуре.</param>
        /// <returns>Строка, описывающая количество задач с правильным склонением.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int taskCount)
            {
                string suffix = taskCount % 10 == 1 && taskCount % 100 != 11 ? "задача" :
                                (taskCount % 10 >= 2 && taskCount % 10 <= 4 && (taskCount % 100 < 10 || taskCount % 100 >= 20)) ? "задачи" : "задач";

                return taskCount == 0 ? "Нет задач" : $"{taskCount} {suffix}";
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
