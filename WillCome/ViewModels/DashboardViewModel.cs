using SimpleInjector;
using WillCome.Utils;

namespace WillCome.ViewModels
{
    // ViewModel для представления "Рабочий стол"
    // TODO: (каркас ViewModel — последующее отключение Container из класса)
    public class DashboardViewModel : WorkspaceViewModel
    {
        #region Fields

        // Контейнер зависимостей для управления службами и сервисами
        private static Container _container = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация DashboardViewModel с контейнером зависимостей.
        /// </summary>
        /// <param name="valueContainer">Контейнер зависимостей SimpleInjector.</param>
        public DashboardViewModel(Container valueContainer)
        {
            _container = valueContainer;
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// Отображаемое имя вкладки или модуля.
        /// </summary>
        public override string DisplayName => "Рабочий стол";

        /// <summary>
        /// Метод для обновления данных модуля (например, при активации вкладки).
        /// </summary>
        public override void UpdateModule()
        {
            // TO-DO: Реализовать логику обновления данных
        }

        #endregion
    }
}
