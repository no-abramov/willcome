using System.Collections.ObjectModel;

namespace WillCome.Models.MainView
{
    // Класс для управления данными основного меню приложения
    public class CoreMainView
    {
        #region Fields

        // Приватная коллекция пунктов меню, доступная только для внутренних операций
        private readonly ObservableCollection<MenuItemModel> MenuItemDataValues = new ObservableCollection<MenuItemModel>();

        // Публичная коллекция пунктов меню, доступная только для чтения
        public readonly ReadOnlyObservableCollection<MenuItemModel> MenuItemDataPublicValues;

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация CoreMainView и настройка публичной коллекции пунктов меню.
        /// </summary>
        public CoreMainView()
        {
            MenuItemDataPublicValues = new ReadOnlyObservableCollection<MenuItemModel>(MenuItemDataValues);
        }

        #endregion

        #region Public Methods (Menu Item Management)

        /// <summary>
        /// Добавляет новый пункт меню в коллекцию.
        /// </summary>
        /// <param name="valueMenuItemData">Модель пункта меню для добавления.</param>
        public void AppendMenuItemData(MenuItemModel valueMenuItemData)
        {
            if (valueMenuItemData != null)
            {
                MenuItemDataValues.Add(valueMenuItemData);
            }
        }

        /// <summary>
        /// Удаляет указанный пункт меню из коллекции.
        /// Если параметр не указан, очищает всю коллекцию пунктов меню.
        /// </summary>
        /// <param name="valueMenuItemData">Модель пункта меню для удаления. Если null — очистка всех пунктов меню.</param>
        public void RemoveMenuItemData(MenuItemModel valueMenuItemData = null)
        {
            if (valueMenuItemData != null)
                MenuItemDataValues.Remove(valueMenuItemData);
            else
                MenuItemDataValues.Clear();
        }

        #endregion
    }
}
