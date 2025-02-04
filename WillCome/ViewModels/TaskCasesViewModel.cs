using SimpleInjector;
using WillCome.Utils;

namespace WillCome.ViewModels
{
    // ViewModel для управления задачами и делами
    // TODO: (каркас ViewModel — последующее отключение Container из класса)
    public class TaskCasesViewModel : WorkspaceViewModel
    {
        #region Fields

        private static Container _container;

        #endregion

        #region Constructor

        public TaskCasesViewModel(Container valueContainer)
        {
            _container = valueContainer;
        }

        #endregion

        #region Base Class Overrides

        public override string DisplayName => "Дела и задачи";

        public override void UpdateModule()
        {
            // TO-DO: Реализовать логику обновления данных
        }

        #endregion
    }
}