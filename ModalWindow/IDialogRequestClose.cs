using System;

namespace ModalWindow
{
    // Интерфейс для запроса на закрытие диалогового окна из ViewModel
    public interface IDialogRequestClose
    {
        #region Events

        // Событие, инициируемое ViewModel для запроса закрытия окна
        event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        #endregion
    }
}