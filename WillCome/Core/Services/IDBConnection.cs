using MySql.Data.MySqlClient;

namespace WillCome.Core.Database
{
    // Интерфейс для работы с подключением к базе данных MySQL
    public interface IDBConnection
    {
        #region Properties

        // Подключение к базе данных
        MySqlConnection Connect { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Создаёт подключение к базе данных с использованием логина и пароля.
        /// </summary>
        /// <param name="loginString">Имя пользователя базы данных.</param>
        /// <param name="passwordString">Пароль пользователя базы данных.</param>
        /// <returns>True, если подключение успешно; иначе false.</returns>
        bool CreateConnect(string loginString, string passwordString);

        /// <summary>
        /// Открывает подключение к базе данных.
        /// </summary>
        /// <returns>True, если подключение успешно открыто; иначе false.</returns>
        bool OpenConnect();

        /// <summary>
        /// Закрывает подключение к базе данных.
        /// </summary>
        void CloseConnect();

        /// <summary>
        /// Проверяет, закрыто ли подключение к базе данных.
        /// </summary>
        /// <returns>True, если подключение закрыто; иначе false.</returns>
        bool IsCloseConnect();

        /// <summary>
        /// Создаёт универсальную SQL-команду для выполнения запросов.
        /// </summary>
        /// <param name="valueCommand">Текст SQL-запроса.</param>
        /// <returns>Объект MySqlCommand для выполнения запросов.</returns>
        MySqlCommand UniCommand(string valueCommand);

        #endregion
    }
}
