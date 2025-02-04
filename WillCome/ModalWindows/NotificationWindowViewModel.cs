using ModalWindow;
using System;
using WillCome.Core.Enums;
using WillCome.Utils;

namespace WillCome.ModalWindows
{
    // ViewModel для модального окна уведомлений
    internal class NotificationWindowViewModel : WorkspaceViewModel, IDialogRequestClose
    {
        #region Events

        // Событие для запроса закрытия модального окна
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        #endregion

        #region Fields

        // Команда для подтверждения (закрытия уведомления)
        private RelayCommand _acceptCommand;
        // Статус уведомления (успех, ошибка и т.д.)
        private NotificationStatusEnum _notificationStatus;
        // Заголовок уведомления
        private string _title;
        // Описание уведомления
        private string _description;
        // Текст кнопки подтверждения
        private string _contentButton;
        // Дополнительные данные для поддержки
        private string _supportData;

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация уведомления с заданными параметрами.
        /// </summary>
        /// <param name="_valueNotificationStatus">Статус уведомления (успех, ошибка и т.д.).</param>
        /// <param name="_valueTitle">Заголовок уведомления.</param>
        /// <param name="_valueDescription">Основной текст уведомления.</param>
        /// <param name="_valueContentButton">Текст на кнопке для закрытия уведомления.</param>
        /// <param name="_valueSupportData">Дополнительные данные (необязательно).</param>
        public NotificationWindowViewModel(NotificationStatusEnum _valueNotificationStatus, string _valueTitle, string _valueDescription, string _valueContentButton, string _valueSupportData = null)
        {
            NotificationStatus = _valueNotificationStatus;
            Title = _valueTitle;
            Description = _valueDescription;
            ContentButton = _valueContentButton;
            SupportData = _valueSupportData;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Команда для закрытия окна уведомления.
        /// </summary>
        public RelayCommand AcceptCommand
        {
            get
            {
                return _acceptCommand ?? (_acceptCommand = new RelayCommand(p =>
                {
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                }, null));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Статус уведомления (например, успех, ошибка или информационное сообщение).
        /// </summary>
        public NotificationStatusEnum NotificationStatus
        {
            get => _notificationStatus;
            set
            {
                if (_notificationStatus != value)
                {
                    _notificationStatus = value;
                    OnPropertyChanged(nameof(NotificationStatus));
                }
            }
        }

        /// <summary>
        /// Заголовок уведомления.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        /// <summary>
        /// Основной текст уведомления.
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        /// <summary>
        /// Текст кнопки для закрытия уведомления.
        /// </summary>
        public string ContentButton
        {
            get => _contentButton;
            set
            {
                if (_contentButton != value)
                {
                    _contentButton = value;
                    OnPropertyChanged(nameof(ContentButton));
                }
            }
        }

        /// <summary>
        /// Дополнительные данные для поддержки или пояснений (необязательное поле).
        /// </summary>
        public string SupportData
        {
            get => _supportData;
            set
            {
                if (_supportData != value)
                {
                    _supportData = value;
                    OnPropertyChanged(nameof(SupportData));
                }
            }
        }

        #endregion
    }
}
