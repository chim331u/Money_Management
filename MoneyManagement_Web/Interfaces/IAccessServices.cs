using MoneyManagement_Web.Data;

namespace MoneyManagement_Web.Interfaces
{
    public interface IAccessServices
    {
        bool IsAuthorized{get;set;}
        string _apiToken { get; set; }
        event Action? OnChange;
        RegistrationRequest LoggedInfo { get; set; }

        Task<AuthResponse> Login(AuthRequest authRequest);
        Task<string> Register(RegistrationRequest registrationRequest);
        Task<string> AccessTest();

    }
}
