using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace WillCome.Utils
{
    // Базовый класс для всех ViewModel в приложении, реализующий основные интерфейсы INotifyPropertyChanged и INotifyDataErrorInfo
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo, IDisposable
    {
        #region Constructor

        /// <summary>
        /// Конструктор базового класса ViewModel.
        /// </summary>
        protected ViewModelBase() { }

        #endregion

        #region Display

        // Отображаемое имя для ViewModel (может использоваться для заголовков окон и т.д.)
        public virtual string DisplayName { get; protected set; }

        // Уникальный идентификатор для ViewModel (для внутренней логики)
        public virtual string DisplayIdentifier { get; protected set; }

        #endregion

        #region Update

        /// <summary>
        /// Метод для обновления данных модуля. Может быть переопределён в наследниках.
        /// </summary>
        public virtual void UpdateModule() { }

        #endregion

        #region Debugging Aides

        // Указывает, выбрасывать ли исключение при недопустимом имени свойства
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        /// <summary>
        /// Проверяет корректность имени свойства во время отладки.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string message = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(message);
                else
                    Debug.Fail(message);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Уведомляет подписчиков об изменении свойства.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyDataErrorInfo Members

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // Поднимает событие изменения ошибки для свойства
        protected void RaiseErrorsChanged(string propertyName) =>
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        public bool HasErrors => _validationErrors.Count > 0;

        // Указывает, прошла ли модель валидацию (нет ошибок)
        public bool IsValidation => !HasErrors;

        private readonly Dictionary<string, string> _validationErrors = new Dictionary<string, string>();

        /// <summary>
        /// Возвращает ошибки для указанного свойства.
        /// </summary>
        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == null)
                return null;

            return _validationErrors.TryGetValue(propertyName, out string error)
                ? new string[] { error }
                : null;
        }

        /// <summary>
        /// Добавляет ошибку валидации для свойства.
        /// </summary>
        public void AppendValidation(string propertyName, string error)
        {
            if (_validationErrors.ContainsKey(propertyName))
                _validationErrors.Remove(propertyName);

            _validationErrors.Add(propertyName, error);
            RaiseErrorsChanged(propertyName);
        }

        /// <summary>
        /// Удаляет ошибку валидации для свойства.
        /// </summary>
        public void RemoveValidation(string propertyName)
        {
            if (!_validationErrors.ContainsKey(propertyName))
                return;

            _validationErrors.Remove(propertyName);
            RaiseErrorsChanged(propertyName);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Логика очистки ресурсов. Может быть переопределена в наследниках.
        /// </summary>
        protected virtual void OnDispose() { }

        /// <summary>
        /// Освобождает ресурсы.
        /// </summary>
        public void Dispose() => this.OnDispose();

        #endregion
    }
}
