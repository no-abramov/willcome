using System;
using System.Collections.ObjectModel;

namespace WillCome.Models.Deal
{
    // Модель, представляющая воронку продаж
    public class Funnel
    {
        #region Properties

        // Уникальный идентификатор воронки
        public long Identifier { get; set; }

        // Позиция воронки (для сортировки)
        public int Position { get; set; }

        // Название воронки
        public string Name { get; set; }

        // Описание воронки
        public string Description { get; set; }

        // Дата создания воронки
        public DateTime CreatedAt { get; set; }

        // Идентификатор пользователя, который создал воронку
        public long CreatedBy { get; set; }

        // Коллекция этапов, связанных с этой воронкой
        public ObservableCollection<Stage> Stages { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация новой воронки с пустым списком этапов.
        /// </summary>
        public Funnel()
        {
            Stages = new ObservableCollection<Stage>();
        }

        #endregion
    }
}
