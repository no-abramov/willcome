using System;

namespace ModalWindow
{
    // Аргументы события для запроса закрытия модального окна
    public class DialogCloseRequestedEventArgs : EventArgs
    {
        #region Properties

        // Результат диалога: true (ОК), false (Отмена), null (не определён)
        public bool? DialogResult { get; }

        #endregion

        #region Constructor

        // Инициализация события с результатом диалога (true, false или null)
        public DialogCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        #endregion
    }
}