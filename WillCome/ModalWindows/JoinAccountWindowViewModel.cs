using System;
using System.Windows;
using System.Text.RegularExpressions;
using ModalWindow;

using WillCome.Utils;
using WillCome.Core.Enums;
using WillCome.Core.ApiClients;
using WillCome.ViewModels;
using WillCome.Core.Database;
using WillCome.Core.Services;

namespace WillCome.ModalWindows
{
    // ViewModel для окна авторизации и регистрации пользователя
    internal class JoinAccountWindowViewModel : WorkspaceViewModel, IDialogRequestClose
    {
        #region Events

        // Событие для запроса закрытия окна (используется сервисом диалогов)
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        #endregion

        #region Fields

        // Клиент для выполнения запросов авторизации и регистрации
        private readonly AuthenticationApiClient _authClient;
        // Интерфейс для работы с базой данных
        private readonly IDBConnection _dbConnection;
        // Сервис для отображения модальных окон
        private readonly IDialogService _dialogService;
        // Данные об авторизованном пользователе
        private readonly IUserAccessData _userAccessData;
        // Основная ViewModel главного окна приложения
        private readonly MainWindowViewModel _mainWindowViewModel;

        // Текущее состояние формы (вход/регистрация)
        private FormAccountState _currentFormState;
        // Флаг загрузки (для отображения прогресс-индикатора)
        private bool _isLoading = true;                             

        // Приватные данные пользователя для авторизации и регистрации
        private string _userLogin = "nikita@mail.ru";
        private string _userPassword = "Skill22_";
        private string _customerCompanyInn;
        private string _customerFirstName;
        private string _customerLastName;
        private string _customerEmail;
        private string _customerPassword;

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализация ViewModel с зависимостями для авторизации и регистрации.
        /// </summary>
        public JoinAccountWindowViewModel(AuthenticationApiClient authClient, IDBConnection dbConnection, IDialogService dialogService, IUserAccessData userAccessData, MainWindowViewModel mainWindowViewModel)
        {
            _authClient = authClient;
            _dbConnection = dbConnection;
            _dialogService = dialogService;
            _userAccessData = userAccessData;
            _mainWindowViewModel = mainWindowViewModel;

            _currentFormState = FormAccountState.Login;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Команда для выполнения авторизации пользователя.
        /// Проверяет данные, выполняет запрос и обрабатывает ответ.
        /// </summary>
        public RelayCommand JoinCommand => new RelayCommand(async p =>
        {
            IsLoading = false; // Показать индикатор загрузки

            var loginResult = await _authClient.LoginAsync(UserLogin, UserPassword); // Отправка запроса на сервер

            if (!loginResult.Success)
            {
                ShowNotification(loginResult); // Показать уведомление об ошибке, если авторизация не удалась
            }
            else
            {
                if (_dbConnection.CreateConnect(UserLogin, UserPassword))
                {
                    UpdateUserData(); // Обновление данных пользователя из базы данных
                    Application.Current.MainWindow = new MainWindow { DataContext = _mainWindowViewModel };
                    Application.Current.MainWindow.Show();
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)); // Закрытие окна авторизации
                }
                else
                {
                    _dialogService.ShowDialog(new NotificationWindowViewModel(NotificationStatusEnum.Mistake, "Ошибка подключения", "Не удалось подключиться. Попробуйте снова.", "Закрыть"));
                }
            }

            IsLoading = true; // Скрыть индикатор загрузки
        }, p => IsValidation); // Команда активна только при успешной валидации

        /// <summary>
        /// Команда для выполнения регистрации нового пользователя и компании.
        /// </summary>
        public RelayCommand RegisterCommand => new RelayCommand(async p =>
        {
            var registrationResult = await _authClient.RegistrationAsync(CustomerCompanyInn, CustomerFirstName, CustomerLastName, CustomerEmail, CustomerPassword);

            if (registrationResult == null || !registrationResult.Success)
            {
                ShowNotification(registrationResult); // Показать уведомление об ошибке регистрации
            }
            else
            {
                ClearRegistrationFields(); // Очистить поля формы регистрации
                CurrentFormState = FormAccountState.Login; // Переключить на форму входа
            }
        }, p => IsValidation);

        /// <summary>
        /// Переключение на форму авторизации.
        /// </summary>
        public RelayCommand SwitchToLoginCommand => new RelayCommand(p => CurrentFormState = FormAccountState.Login);

        /// <summary>
        /// Переключение на форму регистрации.
        /// </summary>
        public RelayCommand SwitchToRegisterCommand => new RelayCommand(p => CurrentFormState = FormAccountState.Register);

        #endregion

        #region Properties

        /// <summary>
        /// Текущее состояние формы (авторизация или регистрация).
        /// </summary>
        public FormAccountState CurrentFormState
        {
            get => _currentFormState;
            set
            {
                if (_currentFormState != value)
                {
                    _currentFormState = value;
                    OnPropertyChanged(nameof(CurrentFormState));
                }
            }
        }

        /// <summary>
        /// Флаг для отображения индикатора загрузки.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        // Свойства для ввода данных пользователем
        public string UserLogin
        {
            get => _userLogin;
            set
            {
                if (_userLogin != value)
                {
                    _userLogin = value;
                    OnPropertyChanged(nameof(UserLogin));
                    ValidateEmail();
                }
            }
        }

        public string UserPassword
        {
            get => _userPassword;
            set
            {
                if (_userPassword != value)
                {
                    _userPassword = value;
                    OnPropertyChanged(nameof(UserPassword));
                    ValidatePassword();
                }
            }
        }

        public string CustomerCompanyInn
        {
            get => _customerCompanyInn;
            set
            {
                if (_customerCompanyInn != value)
                {
                    _customerCompanyInn = value;
                    OnPropertyChanged(nameof(CustomerCompanyInn));
                }
            }
        }

        public string CustomerFirstName
        {
            get => _customerFirstName;
            set
            {
                if (_customerFirstName != value)
                {
                    _customerFirstName = value;
                    OnPropertyChanged(nameof(CustomerFirstName));
                    ValidateFirstName();
                }
            }
        }

        public string CustomerLastName
        {
            get => _customerLastName;
            set
            {
                if (_customerLastName != value)
                {
                    _customerLastName = value;
                    OnPropertyChanged(nameof(CustomerLastName));
                    ValidateLastName();
                }
            }
        }

        public string CustomerEmail
        {
            get => _customerEmail;
            set
            {
                if (_customerEmail != value)
                {
                    _customerEmail = value;
                    OnPropertyChanged(nameof(CustomerEmail));
                }
            }
        }

        public string CustomerPassword
        {
            get => _customerPassword;
            set
            {
                if (_customerPassword != value)
                {
                    _customerPassword = value;
                    OnPropertyChanged(nameof(CustomerPassword));
                    ValidatePassword();
                }
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Проверка корректности введённого E-mail.
        /// </summary>
        private void ValidateEmail()
        {
            string emailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

            if (UserLogin.Length > 0 && !Regex.IsMatch(UserLogin, emailPattern))
                AppendValidation(nameof(UserLogin), $"Укажите корректный E-mail (например: example@mail.ru)");
            else
                RemoveValidation(nameof(UserLogin));
        }

        /// <summary>
        /// Проверка сложности пароля.
        /// </summary>
        private void ValidatePassword()
        {
            if (UserPassword.Length <= 2)
                AppendValidation(nameof(UserPassword), $"Пароль должен содержать не менее 3 символов.");
            else
                RemoveValidation(nameof(UserPassword));
        }

        /// <summary>
        /// Проверка корректности имени.
        /// </summary>
        private void ValidateFirstName()
        {
            if (!Regex.IsMatch(CustomerFirstName, @"^([А-Я]{0,1}[а-яё]{1,23}|[A-Z]{0,1}[a-z]{1,23})$"))
                AppendValidation(nameof(CustomerFirstName), $"Введите корректное имя.");
            else
                RemoveValidation(nameof(CustomerFirstName));
        }

        /// <summary>
        /// Проверка корректности фамилии.
        /// </summary>
        private void ValidateLastName()
        {
            if (!Regex.IsMatch(CustomerLastName, @"^([А-Я]{0,1}[а-яё]{1,23}|[A-Z]{0,1}[a-z]{1,23})$"))
                AppendValidation(nameof(CustomerLastName), $"Введите корректную фамилию.");
            else
                RemoveValidation(nameof(CustomerLastName));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Отображение окна уведомлений с сообщением об ошибке или успешной операции.
        /// </summary>
        private void ShowNotification(ApiResponse response)
        {
            var status = ApiResponse.getStringToEnumStatus(response.Answer.Type);
            _dialogService.ShowDialog(new NotificationWindowViewModel(status, response.Answer.Title, response.Answer.Description, "Закрыть", status == NotificationStatusEnum.Mistake ? response.Answer.SupportData : null));
        }

        /// <summary>
        /// Очистка полей формы регистрации.
        /// </summary>
        private void ClearRegistrationFields()
        {
            CustomerCompanyInn = CustomerFirstName = CustomerLastName = CustomerEmail = CustomerPassword = null;
        }

        /// <summary>
        /// Обновление данных пользователя после успешной авторизации.
        /// </summary>
        private void UpdateUserData()
        {
            _dbConnection.OpenConnect();
            var command = _dbConnection.UniCommand("SELECT c.c_identifier, ur.ur_mode FROM customers c JOIN user_rights ur ON ur.ur_identifier_customer = c.c_identifier WHERE c.c_mail = @email");
            command.Parameters.AddWithValue("@email", UserLogin);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    _userAccessData.Identifier = reader.GetInt32("c_identifier");
                    _userAccessData.Mode = reader.GetString("ur_mode");
                    _userAccessData.Login = UserLogin;
                }
            }

            _dbConnection.CloseConnect();
        }

        #endregion
    }
}
