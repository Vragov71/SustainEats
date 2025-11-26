using SustainEats.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SustainEats.Shared.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthStateProvider _authStateProvider;

        // Using HTTP for server-to-self calls to avoid SSL issues in development.
        private string _baseUrl = "http://localhost:5097";
        private IAuthService _authServiceImplementation;

        public AuthService(HttpClient httpClient, CustomAuthStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        private string GetApiUrl(string path)
        {
            return $"{_baseUrl.TrimEnd('/')}/{path.TrimStart('/')}";
        }

        public event Action? OnAuthStateChanged
        {
            add => _authServiceImplementation.OnAuthStateChanged += value;
            remove => _authServiceImplementation.OnAuthStateChanged -= value;
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
                _authStateProvider.MarkUserAsAuthenticated(result.Username);
                return result.Username;
            }
            return null;
        }

        public async Task Logout()
        {
            // The API call is not strictly necessary for the client-side state, but good practice
            await _httpClient.PostAsync(GetApiUrl("api/auth/logout"), null);
            _authStateProvider.MarkUserAsLoggedOut();
        }

        // This method is no longer the source of truth, but can be kept for convenience
        public string GetUsername()
        {
            var authState = _authStateProvider.GetAuthenticationStateAsync().Result;
            return authState.User.Identity?.Name;
        }
    }

    public class LoginResult
    {
        public string Username { get; set; }
    }
}
