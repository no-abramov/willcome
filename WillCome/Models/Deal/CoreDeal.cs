using System;
using System.Collections.ObjectModel;

namespace WillCome.Models.Deal
{
    // Класс для управления данными по воронкам и сделкам
    public class CoreDeal
    {
        #region Fields

        // Приватная коллекция воронок, доступная только для внутреннего использования
        private readonly ObservableCollection<Funnel> FunnelDataValues = new ObservableCollection<Funnel>();

        // Публичная коллекция воронок, доступная только для чтения
        public readonly ReadOnlyObservableCollection<Funnel> FunnelDataPublicValues;

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация CoreDeal и настройка публичной коллекции воронок.
        /// </summary>
        public CoreDeal()
        {
            FunnelDataPublicValues = new ReadOnlyObservableCollection<Funnel>(FunnelDataValues);
        }

        #endregion

        #region Public Methods (Funnel Management)

        /// <summary>
        /// Добавляет новую воронку в коллекцию.
        /// </summary>
        /// <param name="valueFunnelModel">Модель воронки для добавления.</param>
        public void AppendFunnelData(Funnel valueFunnelModel)
        {
            if (valueFunnelModel != null)
            {
                FunnelDataValues.Add(valueFunnelModel);
            }
        }

        /// <summary>
        /// Удаляет воронку из коллекции или очищает все воронки, если параметр не указан.
        /// </summary>
        /// <param name="valueFunnelModel">Модель воронки для удаления. Если null — очистка всех данных.</param>
        public void RemoveFunnelData(Funnel valueFunnelModel = null)
        {
            if (valueFunnelModel == null)
            {
                FunnelDataValues.Clear();
            }
            else
            {
                FunnelDataValues.Remove(valueFunnelModel);
            }
        }

        #endregion
    }
}
