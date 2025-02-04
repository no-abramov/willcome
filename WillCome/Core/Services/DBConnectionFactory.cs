using System;
using MySql.Data.MySqlClient;

namespace WillCome.Core.Database
{
    // Фабрика для создания и управления подключением к базе данных MySQL
    internal class DBConnectionFactory : IDBConnection
    {
        #region Constructor

        // Инициализация фабрики подключения
        public DBConnectionFactory()
        {
            _connect = new MySqlConnection();
        }

        #endregion

        #region Private Fields

        // Экземпляр подключения к базе данных
        private MySqlConnection _connect = null;

        #endregion

        #region Public Properties

        public MySqlConnection Connect
        {
            get { return _connect; }
            set { _connect = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Закрывает текущее подключение к базе данных.
        /// </summary>
        public void CloseConnect()
        {
            if (Connect != null)
                Connect.Close();
        }

        /// <summary>
        /// Открывает подключение к базе данных.
        /// </summary>
        /// <returns>True, если подключение открыто успешно; иначе false.</returns>
        public bool OpenConnect()
        {
            try
            {
                Connect.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        throw new Exception("Ошибка подключения: Cannot contact MySQL Server");
                    case 1045:
                        throw new Exception("Ошибка подключения: Invalid username/password, please try again");
                    case 1042:
                        throw new Exception("Ошибка подключения: Unable to resolve DNS");
                }
                return false;
            }
        }

        /// <summary>
        /// Проверяет, закрыто ли подключение к базе данных.
        /// </summary>
        /// <returns>True, если подключение закрыто; иначе false.</returns>
        public bool IsCloseConnect()
        {
            return Connect.State == System.Data.ConnectionState.Closed;
        }

        /// <summary>
        /// Создаёт подключение к базе данных с использованием логина и пароля.
        /// </summary>
        /// <param name="loginString">Имя пользователя базы данных.</param>
        /// <param name="passwordString">Пароль пользователя базы данных.</param>
        /// <returns>True, если подключение успешно; иначе false.</returns>
        public bool CreateConnect(string loginString, string passwordString)
        {
            string[] items = { "176.124.219.13", "WillCome_trial", loginString, passwordString };

            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
            {
                Server = items[0],
                Database = items[1],
                UserID = loginString,
                Password = passwordString,
                CharacterSet = "utf8",
                SslMode = MySqlSslMode.Preferred,
                ConnectionTimeout = 10000
            };

            Connect.ConnectionString = csb.ToString();

            try
            {
                Connect.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Ошибка подключения: {ex.Message}");
            }
            finally
            {
                if (Connect != null)
                    Connect.Close();
            }
        }

        /// <summary>
        /// Создаёт SQL-команду для выполнения запросов к базе данных.
        /// </summary>
        /// <param name="valueString">Текст SQL-запроса.</param>
        /// <returns>Объект MySqlCommand для выполнения запросов.</returns>
        public MySqlCommand UniCommand(string valueString)
        {
            if (valueString == null)
                return null;

            return new MySqlCommand(valueString, _connect);
        }

        #endregion
    }
}
