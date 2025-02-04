using System;
using System.Collections.ObjectModel;
using ModalWindow;
using WillCome.Utils;

namespace WillCome.ModalWindows
{
    // ViewModel для модального окна добавления этапа в воронку продаж
    public class AddStageInFunnelWindowViewModel : WorkspaceViewModel, IDialogRequestClose
    {
        #region Events

        // Событие для запроса закрытия модального окна
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        #endregion

        #region Fields

        // Коллекция индексов цветов для выбора пользователем
        public ObservableCollection<int> Colors { get; set; }

        // Название этапа воронки
        private string _funnelName = string.Empty;
        // Цвет этапа воронки
        private int _colorFunnelStage;
        // Выбранный цвет по умолчанию
        private int _selectedColor = 0;               

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация ViewModel для окна добавления этапа.
        /// </summary>
        public AddStageInFunnelWindowViewModel()
        {
            // Инициализация доступных цветов (индексы для привязки к палитре)
            Colors = new ObservableCollection<int>
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8,
                9, 10, 11, 12, 13, 14, 15, 16, 17
            };

            // Цвет по умолчанию
            ColorFunnelStage = 2;
            // Пустое имя по умолчанию
            FunnelName = null;   
        }

        #endregion

        #region Commands

        /// <summary>
        /// Команда для подтверждения добавления этапа в воронку.
        /// Закрывает окно с положительным результатом (DialogResult = true).
        /// </summary>
        public RelayCommand AcceptCommand => new RelayCommand(param =>
        {
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        });

        /// <summary>
        /// Команда для отмены добавления этапа.
        /// Закрывает окно с отрицательным результатом (DialogResult = false).
        /// </summary>
        public RelayCommand CancelCommand => new RelayCommand(param =>
        {
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false));
        });

        #endregion

        #region Properties

        /// <summary>
        /// Название этапа воронки.
        /// </summary>
        public string FunnelName
        {
            get => _funnelName;
            set
            {
                if (_funnelName != value)
                {
                    _funnelName = string.IsNullOrWhiteSpace(value) ? null : value;
                    OnPropertyChanged(nameof(FunnelName));
                }
            }
        }

        /// <summary>
        /// Цвет для этапа воронки (индекс из коллекции Colors).
        /// </summary>
        public int ColorFunnelStage
        {
            get => _colorFunnelStage;
            set
            {
                if (_colorFunnelStage != value)
                {
                    _colorFunnelStage = value;
                    OnPropertyChanged(nameof(ColorFunnelStage));
                }
            }
        }

        /// <summary>
        /// Выбранный цвет для этапа воронки.
        /// </summary>
        public int SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged(nameof(SelectedColor));
                }
            }
        }

        #endregion
    }
}
