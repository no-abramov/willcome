namespace WillCome.Core.Services
{
    // Класс для хранения данных о текущем пользователе
    public class UserAccessData : IUserAccessData
    {
        #region Private Fields

        private static int _identifier;
        private static string _login;
        private static string _mode;

        #endregion

        #region Public Properties

        public int Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public string Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        #endregion
    }
}
