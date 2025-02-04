using WillCome.Utils;

namespace WillCome.Models.MainView
{
    // Модель, представляющая пункт меню в основном интерфейсе приложения
    public class MenuItemModel
    {
        #region Properties

        // Уникальный идентификатор пункта меню
        public int Identifier { get; set; }

        // Команда, которая выполняется при выборе пункта меню
        public RelayCommand Command { get; set; }

        // Краткое имя пункта меню (например, для использования во внутренней логике)
        public string Name { get; set; }

        // Отображаемый заголовок пункта меню
        public string Title { get; set; }

        // Иконка пункта меню (индекс иконки в ресурсах приложения)
        public int Icon { get; set; }

        #endregion
    }
}
