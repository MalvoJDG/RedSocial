using Microsoft.AspNetCore.Http;
using RedSocial.Core.Application.ViewModels.Users;
using RedSocial.Core.Application.Helpers;
using RedSocial.Core.Application.Dtos.Account;

namespace RedSocial.Middelwares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            AuthenticationResponse userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("users");

            if (userViewModel == null)
            {
                return false;
            }
            return true;
        }
    }
}
