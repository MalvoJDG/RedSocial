using RedSocial.Core.Application.Dtos.Account;
using RedSocial.Core.Application.ViewModels.Users;

namespace RedSocial.Core.Application.Services
{
    public interface IUserService
    {
        Task<string> ConfirmEmailAsyncs(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string token);
        Task<AuthenticationResponse> LoginAsyncs(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsyncs(SaveUserViewModel vm, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsyncs(ResetPasswordViewModel vm);
        Task SingoutAsyncs();
    }
}