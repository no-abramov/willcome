using System;
using System.Windows.Input;

namespace WillCome.Utils
{
    // Абстрактный класс для представлений (ViewModel), связанных с рабочим пространством приложения
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        #region Constructor

        /// <summary>
        /// Конструктор для WorkspaceViewModel.
        /// </summary>
        protected WorkspaceViewModel() { }

        #endregion

        #region Commands

        private RelayCommand _closeCommand;

        /// <summary>
        /// Команда для закрытия текущего окна или модуля.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(param => this.OnRequestClose());
                }

                return _closeCommand;
            }
        }

        #endregion

        #region RequestClose

        // Событие для запроса закрытия окна
        public event EventHandler RequestClose;

        /// <summary>
        /// Метод для вызова события закрытия окна.
        /// </summary>
        protected void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
