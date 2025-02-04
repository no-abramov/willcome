using System.Collections.ObjectModel;
using WillCome.Models.MainView;
using WillCome.Utils;

namespace WillCome.Repositories
{
    // Интерфейс для работы с хранилищем данных основного представления (главного меню)
    public interface IMainViewStorage
    {
        #region Methods

        /// <summary>
        /// Получает данные пунктов меню.
        /// </summary>
        ReadOnlyObservableCollection<MenuItemModel> GetMenuItemData();

        /// <summary>
        /// Загружает данные пунктов меню из базы данных.
        /// </summary>
        void DataMenuItem();

        /// <summary>
        /// Обновляет данные модулей приложения.
        /// </summary>
        void DataUpdateModules();

        #endregion

        #region Commands

        /// <summary>
        /// Команда для открытия Дашборда.
        /// </summary>
        RelayCommand CommandMenuItemDashboard { get; set; }

        /// <summary>
        /// Команда для открытия раздела "Сделки".
        /// </summary>
        RelayCommand CommandMenuItemDeal { get; set; }

        /// <summary>
        /// Команда для открытия раздела "Задачи".
        /// </summary>
        RelayCommand CommandMenuItemTaskCases { get; set; }

        /// <summary>
        /// Команда для открытия раздела "База клиентов".
        /// </summary>
        RelayCommand CommandMenuItemCustomerBase { get; set; }

        #endregion
    }
}
