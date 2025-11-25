using SustainEats.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SustainEats.Shared.Services
{
    public interface IAuthService
    {
        event Action OnAuthStateChanged;
        Task<bool> Register(RegisterModel model);
        Task<string> Login(LoginModel model);
        Task Logout();
        string GetUsername();
    }
}