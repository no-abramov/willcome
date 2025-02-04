using System;
using System.Collections.Generic;
using System.Windows;

namespace ModalWindow
{
    // Сервис для управления отображением модальных окон
    public class DialogService : IDialogService
    {
        #region Fields

        // Родительское окно, относительно которого открываются модальные окна
        private readonly Window owner;

        #endregion

        #region Constructor

        // Инициализация сервиса с указанием родительского окна
        public DialogService(Window owner)
        {
            this.owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }

        #endregion

        #region Properties

        // Словарь для хранения сопоставлений между ViewModel и View (окном)
        public IDictionary<Type, Type> Mappings { get; }

        #endregion

        #region Methods

        // Регистрация ViewModel и соответствующего ей окна (View)
        public void Register<TViewModel, TView>()
            where TViewModel : IDialogRequestClose
            where TView : IDialog
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");
            }

            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        // Отображение модального окна с созданием нового экземпляра ViewModel
        public bool? ShowDialog<TViewModel>()
            where TViewModel : IDialogRequestClose
        {
            if (Mappings.TryGetValue(typeof(TViewModel), out var viewType))
            {
                var viewModel = Activator.CreateInstance<TViewModel>(); // Создание нового экземпляра ViewModel
                return ShowDialog(viewModel);
            }

            throw new InvalidOperationException($"No registered window for {typeof(TViewModel)}");
        }

        // Отображение модального окна с передачей существующего экземпляра ViewModel
        public bool? ShowDialog<TViewModel>(TViewModel viewModel)
            where TViewModel : IDialogRequestClose
        {
            if (!Mappings.TryGetValue(typeof(TViewModel), out var viewType))
            {
                throw new InvalidOperationException($"No view registered for {typeof(TViewModel)}.");
            }

            IDialog dialogApp = Activator.CreateInstance(viewType) as IDialog;

            if (dialogApp == null)
            {
                throw new InvalidOperationException("Unable to create dialog instance.");
            }

            // Обработчик события для закрытия окна по запросу из ViewModel
            EventHandler<DialogCloseRequestedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                viewModel.CloseRequested -= handler;

                if (e.DialogResult.HasValue)
                {
                    // Установка результата диалога
                    dialogApp.DialogResult = e.DialogResult; 
                }
                else
                {
                    // Закрытие окна без результата
                    dialogApp.Close(); 
                }
            };

            // Подписка на событие запроса закрытия окна
            viewModel.CloseRequested += handler;

            dialogApp.DataContext = viewModel;
            dialogApp.Owner = owner;          

            return dialogApp.ShowDialog();
        }

        #endregion
    }
}