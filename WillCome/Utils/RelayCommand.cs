using System;
using System.Windows.Input;

namespace WillCome.Utils
{
    // Реализация команды для привязки действий к элементам управления (например, кнопкам)
    public class RelayCommand : ICommand
    {
        #region Fields

        // Действие, которое выполняется при вызове команды
        private readonly Action<object> execute;

        // Функция для проверки, может ли команда быть выполнена
        private readonly Func<object, bool> canExecute;

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация команды с действием и условием выполнения.
        /// </summary>
        /// <param name="execute">Действие для выполнения.</param>
        /// <param name="canExecute">Условие, определяющее возможность выполнения команды.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        #endregion

        #region Events

        /// <summary>
        /// Событие, вызываемое при изменении условий выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Проверяет, может ли команда быть выполнена с заданным параметром.
        /// </summary>
        /// <param name="parameter">Параметр для проверки выполнения команды.</param>
        /// <returns>True, если команда может быть выполнена; иначе false.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Выполняет действие, связанное с командой.
        /// </summary>
        /// <param name="parameter">Параметр, передаваемый в действие.</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion
    }
}
