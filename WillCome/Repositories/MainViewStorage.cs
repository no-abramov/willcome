using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using WillCome.Core.Database;
using WillCome.Core.Services;
using WillCome.Models.MainView;
using WillCome.Utils;

namespace WillCome.Repositories
{
    // Хранилище данных для основного представления (главного меню)
    public class MainViewStorage : IMainViewStorage
    {
        #region Fields

        // Подключение к базе данных
        private readonly IDBConnection _dbConnection;
        // Данные текущего пользователя
        private readonly IUserAccessData _userAccessData;
        // Основная модель представления
        private readonly CoreMainView _coreMainView;

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация хранилища данных для главного меню.
        /// </summary>
        public MainViewStorage(IDBConnection dbConnection, IUserAccessData userAccessData)
        {
            _dbConnection = dbConnection;
            _userAccessData = userAccessData;
            _coreMainView = new CoreMainView();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Загрузка пунктов меню из базы данных.
        /// </summary>
        private void DownloadMenuItem()
        {
            if (_dbConnection.OpenConnect())
            {
                _coreMainView.RemoveMenuItemData();

                var command = _dbConnection.UniCommand(
                    "SELECT am.am_identifier, am.am_name, am.am_title, am.am_icon " +
                    "FROM access_module am " +
                    "JOIN access_module_rights amr ON am.am_identifier = amr.amr_identifier_module " +
                    "JOIN user_rights ur ON ur.ur_mode = amr.amr_rights_mode " +
                    "JOIN customers c ON c.c_identifier = ur.ur_identifier_customer " +
                    "WHERE c.c_mail = @mail"
                );

                command.Parameters.Add(new MySqlParameter("@mail", MySqlDbType.VarChar)
                {
                    Value = _userAccessData.Login
                });

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var itemModel = new MenuItemModel
                    {
                        Identifier = int.Parse(reader["am_identifier"].ToString()),
                        Name = reader["am_name"].ToString(),
                        Title = reader["am_title"].ToString(),
                        Icon = int.Parse(reader["am_icon"].ToString()),
                        Command = GetModuleCommand(reader["am_name"].ToString())
                    };

                    _coreMainView.AppendMenuItemData(itemModel);
                }

                reader.Close();
                _dbConnection.CloseConnect();
            }
        }

        /// <summary>
        /// Обновление данных модулей (при необходимости).
        /// </summary>
        private void UpdateModules()
        {
            if (_dbConnection.IsCloseConnect())
                DownloadMenuItem();
        }

        /// <summary>
        /// Определение команды для модуля по его имени.
        /// </summary>
        private RelayCommand GetModuleCommand(string moduleName)
        {
            return moduleName switch
            {
                "ModuleDashboard" => CommandMenuItemDashboard,
                "ModuleDeal" => CommandMenuItemDeal,
                "ModuleTaskCases" => CommandMenuItemTaskCases,
                "ModuleCustomerBase" => CommandMenuItemCustomerBase,
                _ => null
            };
        }

        #endregion

        #region Public Methods

        public RelayCommand CommandMenuItemDashboard { get; set; }
        public RelayCommand CommandMenuItemDeal { get; set; }
        public RelayCommand CommandMenuItemTaskCases { get; set; }
        public RelayCommand CommandMenuItemCustomerBase { get; set; }

        public ReadOnlyObservableCollection<MenuItemModel> GetMenuItemData()
        {
            return _coreMainView.MenuItemDataPublicValues;
        }

        public void DataMenuItem()
        {
            DownloadMenuItem();
        }

        public void DataUpdateModules()
        {
            UpdateModules();
        }

        #endregion
    }
}
