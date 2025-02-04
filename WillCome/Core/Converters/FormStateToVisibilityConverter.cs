using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using WillCome.Core.Enums;

namespace WillCome.Core.Converters
{
    // Конвертер для преобразования состояния формы в значение видимости (Visible/Collapsed)
    public class FormStateToVisibilityConverter : IValueConverter
    {
        #region Properties

        // Целевое состояние формы, которое должно быть видно
        public FormAccountState TargetState { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Преобразует состояние формы в видимость.
        /// </summary>
        /// <param name="value">Текущее состояние формы (FormAccountState).</param>
        /// <param name="targetType">Тип целевого значения (обычно Visibility).</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Информация о культуре.</param>
        /// <returns>Visible, если состояние совпадает с TargetState, иначе Collapsed.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FormAccountState formState)
            {
                return formState == TargetState ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
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
