using RedSocial.Core.Application.Dtos.Account;

namespace RedSocial.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AutheticationAsync(AuthenticationRequest request);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsyncs(ResetPasswordRequest request);
        Task<AuthenticationResponse> GetUserByIdAsync(string userId);
        Task<List<AuthenticationResponse>> GetUsersByIdsAsync(List<string> userIds);
        Task<string> GetUserIdByUsernameAsync(string username);
        Task SingoutAsyncs();
    }
}