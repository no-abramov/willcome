namespace WillCome.Core.Services
{
    // Интерфейс для хранения данных о текущем пользователе
    public interface IUserAccessData
    {
        #region Properties

        // Уникальный идентификатор пользователя
        int Identifier { get; set; }

        // Логин пользователя
        string Login { get; set; }

        // Режим работы пользователя (например, "Admin" или "User")
        string Mode { get; set; }

        #endregion
    }
}
