using WillCome.Core.Enums;

namespace WillCome.Core.ApiClients
{
    // Класс для обработки ответа от API
    public class ApiResponse
    {
        #region Properties

        // Указывает, успешен ли запрос
        public bool Success { get; set; }

        // Ответ сервера, содержащий информацию о результате запроса
        public ResultRequest Answer { get; set; }

        #endregion

        #region Methods

        // Конвертация строки в перечисление NotificationStatusEnum
        public static NotificationStatusEnum getStringToEnumStatus(string _statusString)
        {
            switch (_statusString.ToLower())
            {
                case "mistake": return NotificationStatusEnum.Mistake;
                case "message": return NotificationStatusEnum.Message;
                case "success": return NotificationStatusEnum.Success;
                default: return NotificationStatusEnum.Message;
            }
        }

        #endregion
    }
}