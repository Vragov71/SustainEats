using Microsoft.AspNetCore.Components;
using SustainEats.Shared.Models;
using SustainEats.Shared.Services;

namespace SustainEats.Web.Services
{
    public class AuthService : IAuthService
    {
        public event Action OnAuthStateChanged;

        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private string _username;

        public AuthService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        private string GetApiUrl(string path)
        {
            return $"{_navigationManager.BaseUri}{path}";
        }

        public async Task<bool> Register(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync(GetApiUrl("api/auth/register"), model);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> Login(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync(GetApiUrl("api/auth/login"), model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                _username = result.Username;
                OnAuthStateChanged?.Invoke();
                return _username;
            }
            return null;
        }

        public async Task Logout()
        {
            await _httpClient.PostAsync(GetApiUrl("api/auth/logout"), null);
            _username = null;
            OnAuthStateChanged?.Invoke();
        }



        public string GetUsername() => _username;
    }

    public class LoginResult
    {
        public string Username { get; set; }
    }
}