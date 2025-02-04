using SimpleInjector;
using WillCome.Utils;

namespace WillCome.ViewModels
{
    // ViewModel для управления базой клиентов
    // TODO: (каркас ViewModel — последующее отключение Container из класса)
    public class CustomerBaseViewModel : WorkspaceViewModel
    {
        #region Fields

        private static Container _container;

        #endregion

        #region Constructor

        public CustomerBaseViewModel(Container valueContainer)
        {
            _container = valueContainer;
        }

        #endregion

        #region Base Class Overrides

        public override string DisplayName => "Мои клиенты";

        public override void UpdateModule()
        {
            // TO-DO: Реализовать логику обновления данных
        }

        #endregion
    }
}