using System.Collections.ObjectModel;
using WillCome.Models.Deal;

namespace WillCome.Repositories
{
    // Интерфейс для работы с хранилищем сделок
    public interface IDealStorage
    {
        #region Methods

        /// <summary>
        /// Возвращает коллекцию воронок продаж.
        /// </summary>
        ReadOnlyObservableCollection<Funnel> GetFunnels();

        /// <summary>
        /// Загружает данные о воронках из базы данных.
        /// </summary>
        void LoadFunnel();

        /// <summary>
        /// Обновляет данные по определённой воронке.
        /// </summary>
        /// <param name="funnel">Объект воронки для обновления.</param>
        void UpdateFunnel(Funnel funnel);

        /// <summary>
        /// Загружает данные о сделках из базы данных.
        /// </summary>
        void LoadDeals();

        /// <summary>
        /// Обновляет данные по сделкам.
        /// </summary>
        void UpdateDeals();

        /// <summary>
        /// Обновляет позицию сделки, перемещая её на новый этап в воронке.
        /// </summary>
        /// <param name="dealValue">Сделка для обновления.</param>
        /// <param name="stageValue">Новый этап для сделки.</param>
        void UpdatePositionStageDeal(Deal dealValue, Stage stageValue);

        #endregion
    }
}
