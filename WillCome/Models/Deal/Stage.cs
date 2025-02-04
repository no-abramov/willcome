using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace WillCome.Models.Deal
{
    // Модель, представляющая этап воронки продаж
    public class Stage
    {
        #region Properties

        // Уникальный идентификатор этапа
        public long Identifier { get; set; }

        // Идентификатор воронки, к которой относится этап
        public long FunnelIdentifier { get; set; }

        // Позиция этапа в воронке
        public int Position { get; set; }

        // Цвет этапа (индекс для палитры)
        public int Color { get; set; }

        // Название этапа
        public string Name { get; set; }

        // Дата создания этапа
        public DateTime CreatedAt { get; set; }

        // Идентификатор пользователя, создавшего этап
        public long CreatedBy { get; set; }

        // Коллекция сделок, связанных с этапом
        public ObservableCollection<Deal> Deals { get; set; }

        /// <summary>
        /// Строковое представление количества сделок и их общей суммы.
        /// Например: "3 сделки на 100 000 ₽"
        /// </summary>
        public string DealsSummary => $"{Deals.Count} {GetDealsWord(Deals.Count)} на {Deals.Sum(d => d.Budget):C}";

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация нового этапа с пустым списком сделок.
        /// </summary>
        public Stage()
        {
            Deals = new ObservableCollection<Deal>();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Метод для корректного склонения слова "сделка" в зависимости от количества.
        /// </summary>
        private string GetDealsWord(int count)
        {
            if (count % 100 >= 11 && count % 100 <= 19)
                return "сделок";

            switch (count % 10)
            {
                case 1: return "сделка";
                case (2 or 3 or 4): return "сделки";
                default: return "сделок";
            }
        }

        #endregion
    }
}
