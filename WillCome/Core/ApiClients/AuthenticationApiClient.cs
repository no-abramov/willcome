using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WillCome.Core.ApiClients;

public class AuthenticationApiClient
{
    #region Fields

    // HTTP-клиент для выполнения запросов к серверу
    private readonly HttpClient _httpClient;

    #endregion

    #region Constructor

    // Инициализация клиента с переданным экземпляром HttpClient
    public AuthenticationApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    #endregion

    #region Public Async Tasks

    #region Public Async Task LoginAsync

    /// <summary>
    /// Выполняет асинхронную авторизацию пользователя.
    /// Отправляет POST-запрос с данными пользователя на сервер для проверки учётных данных.
    /// </summary>
    /// <param name="_username">Имя пользователя для входа в систему.</param>
    /// <param name="_password">Пароль, связанный с учётной записью пользователя.</param>
    /// <returns>Объект <see cref="ApiResponse"/>, содержащий информацию о результате авторизации.</returns>
    /// <exception cref="Exception">Генерируется в случае ошибки при выполнении запроса к серверу.</exception>
    public async Task<ApiResponse> LoginAsync(string _username, string _password)
    {
        var url = "http://176.124.219.13/api/loginV2.php"; // URL для авторизации

        // Формирование данных для отправки (JSON)
        var loginData = new
        {
            username = _username,
            password = _password
        };

        var jsonContent = JsonConvert.SerializeObject(loginData);
        Console.WriteLine("— — — SerializeObject (Login): \n" + jsonContent);

        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(url, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("— — — ServerResponse (Login): \n" + jsonResponse);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
            }
            else
            {
                throw new Exception($"Ошибка при выполнении запроса: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Не удалось выполнить запрос: " + ex.Message);
        }
    }

    #endregion

    #region Public Async Task RegistrationAsync

    /// <summary>
    /// Выполняет асинхронную регистрацию компании и пользователя.
    /// Отправляет POST-запрос с данными о компании и пользователе на сервер для создания новой учётной записи.
    /// </summary>
    /// <param name="_companyInn">ИНН компании для регистрации.</param>
    /// <param name="_firstName">Имя пользователя для регистрации.</param>
    /// <param name="_lastName">Фамилия пользователя для регистрации.</param>
    /// <param name="_userEmail">Электронная почта пользователя для связи и входа в систему.</param>
    /// <param name="_userPassword">Пароль, который будет использоваться для авторизации пользователя.</param>
    /// <returns>Объект <see cref="ApiResponse"/>, содержащий информацию о результате регистрации.</returns>
    /// <exception cref="Exception">Генерируется в случае ошибки при выполнении запроса к серверу.</exception>
    public async Task<ApiResponse> RegistrationAsync(string _companyInn, string _firstName, string _lastName, string _userEmail, string _userPassword)
    {
        var url = "http://176.124.219.13/api/registrationV2.php"; // URL для регистрации

        // Формирование данных для отправки (JSON)
        var registrationData = new
        {
            inn = _companyInn,
            firstname = _firstName,
            lastname = _lastName,
            email = _userEmail,
            password = _userPassword
        };

        var jsonContent = JsonConvert.SerializeObject(registrationData);
        Console.WriteLine("— — — SerializeObject (Registration): \n" + jsonContent);

        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(url, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
            }
            else
            {
                throw new Exception($"Ошибка при выполнении запроса: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Не удалось выполнить запрос: " + ex.Message);
        }
    }

    #endregion

    #endregion
}