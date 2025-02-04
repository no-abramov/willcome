using System;
using WillCome.Core.Enums;

namespace WillCome.Models.Deal
{
    // Модель, представляющая сделку в системе
    public class Deal
    {
        #region Properties

        // Уникальный идентификатор сделки
        public long DealIdentifier { get; set; }

        // Название сделки
        public string DealName { get; set; }

        // Бюджет сделки
        public decimal Budget { get; set; }

        // Дата создания сделки
        public DateTime CreateDate { get; set; }

        // Идентификатор воронки, к которой относится сделка
        public long Funnel { get; set; }

        // Этап воронки, на котором находится сделка
        public string Stage { get; set; }

        // Идентификатор ответственного за сделку
        public long ResponsibleIdentifier { get; set; }

        // Имя и фамилия ответственного сотрудника
        public string ResponsibleFirstName { get; set; }
        public string ResponsibleLastName { get; set; }

        // Полное имя ответственного сотрудника
        public string ResponsibleFullName => $"{ResponsibleFirstName} {ResponsibleLastName}";

        // Тип контактного лица (индивидуальный предприниматель или юридическое лицо)
        public EntitiesTypeEnum ContactType { get; set; }

        // Идентификатор контактного лица
        public long? ContactIdentifier { get; set; }

        // Имя контактного лица
        public string ContactName { get; set; }

        // Номер телефона контактного лица
        public string ContactPhone { get; set; }

        // Количество связанных задач
        public int TaskSummary { get; set; }

        #endregion
    }
}
