namespace ModalWindow
{
    // Интерфейс для работы с модальными окнами (регистрация и отображение)
    public interface IDialogService
    {
        #region Methods

        // Регистрация ViewModel и соответствующего ей окна (View)
        void Register<TViewModel, TView>()
            where TViewModel : IDialogRequestClose
            where TView : IDialog;

        // Отображение модального окна с передачей экземпляра ViewModel
        bool? ShowDialog<TViewModel>(TViewModel viewModel)
            where TViewModel : IDialogRequestClose;

        // Отображение модального окна без передачи экземпляра ViewModel (создаётся новый)
        bool? ShowDialog<TViewModel>()
            where TViewModel : IDialogRequestClose;

        #endregion
    }
}