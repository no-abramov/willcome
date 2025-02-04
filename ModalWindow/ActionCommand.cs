using System;
using System.Windows.Input;

namespace ModalWindow
{
    // Реализация команды для привязки действий к элементам управления (например, кнопкам)
    public class ActionCommand : ICommand
    {
        #region Fields

        // Действие, которое выполняется при вызове команды
        private readonly Action<object> action;

        // Предикат для проверки, может ли команда быть выполнена
        private readonly Predicate<object> predicate;

        #endregion

        #region Constructors

        // Конструктор для создания команды с действием без условия выполнения
        public ActionCommand(Action<object> action) : this(action, null)
        {
            // Вызов перегруженного конструктора с предикатом = null
        }

        // Конструктор для создания команды с действием и условием выполнения
        public ActionCommand(Action<object> action, Predicate<object> predicate)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), @"Необходимо указать действие (Action<T>).");
            }

            this.action = action;
            this.predicate = predicate;
        }

        #endregion

        #region Events

        // Событие, которое вызывается при изменении условий выполнения команды
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region Methods

        // Проверяет, может ли команда быть выполнена с заданным параметром
        public bool CanExecute(object parameter)
        {
            if (parameter == null && predicate != null)
            {
                return false; // Если предикат есть, но параметр null — выполнение запрещено
            }

            return predicate == null || predicate(parameter);
        }

        // Выполняет действие, связанное с командой
        public void Execute(object parameter)
        {
            if (parameter == null && action != null)
            {
                throw new ArgumentNullException(nameof(parameter), "Параметр не может быть null");
            }

            if (action != null)
            {
                action(parameter);
            }
        }

        #endregion
    }
}