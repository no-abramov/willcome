namespace WillCome.Core.ApiClients
{
    // Класс для хранения результата запроса к API
    public class ResultRequest
    {
        #region Properties

        // Тип сообщения (например, ошибка, уведомление и т.д.)
        public string Type { get; set; }

        // Заголовок сообщения
        public string Title { get; set; }

        // Описание сообщения
        public string Description { get; set; }

        // Дополнительные данные, связанные с запросом
        public string SupportData { get; set; }

        #endregion
    }
}