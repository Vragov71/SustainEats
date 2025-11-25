using SustainEats.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System;
using System.Threading.Tasks;

namespace SustainEats.Shared.Services
{
    public class AuthService : IAuthService
    {
        public event Action OnAuthStateChanged;

        private readonly HttpClient _httpClient;
        private string _username;

        // Base URL will need to be configured differently for Web and MAUI
        private string _baseUrl = "https://localhost:7138"; // Default for local web dev

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string GetApiUrl(string path)
        {
            return $"{_baseUrl}/{path}";
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