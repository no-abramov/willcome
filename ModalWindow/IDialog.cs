using System.Windows;

namespace ModalWindow
{
    // Интерфейс для модальных окон
    public interface IDialog
    {
        #region Properties

        // Контекст данных, связывающий окно с ViewModel
        object DataContext { get; set; }

        // Результат диалога: true (ОК), false (Отмена) или null (не определён)
        bool? DialogResult { get; set; }

        // Ссылка на родительское окно, которому принадлежит диалог
        Window Owner { get; set; }

        #endregion

        #region Methods

        // Закрывает модальное окно
        void Close();

        // Отображает окно как модальное и ожидает его закрытия
        bool? ShowDialog();

        #endregion
    }
}