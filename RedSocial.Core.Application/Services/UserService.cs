using AutoMapper;
using RedSocial.Core.Application.Dtos.Account;
using RedSocial.Core.Application.Dtos.Email;
using RedSocial.Core.Application.Interfaces.Repositories;
using RedSocial.Core.Application.Interfaces.Services;
using RedSocial.Core.Application.ViewModels.Users;
using RedSocial.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IAccountService accountService)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsyncs(LoginViewModel vm)
        {
            AuthenticationRequest Loginrequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AutheticationAsync(Loginrequest);

            return userResponse;
        }

        public async Task<RegisterResponse> RegisterAsyncs(SaveUserViewModel vm, string origin)
        {
            RegisterRequest RegisterRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterBasicUserAsync(RegisterRequest, origin);
        }

        public async Task<string> ConfirmEmailAsyncs(string userId, string token)
        {
            return await _accountService.ConfirmEmailAsync(userId, token);
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string token)
        {
            ForgotPasswordRequest ForgotrRequest = _mapper.Map<ForgotPasswordRequest>(vm);
            return await _accountService.ForgotPasswordAsync(ForgotrRequest, token);
        }
        public async Task<ResetPasswordResponse> ResetPasswordAsyncs(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest ForgotrRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsyncs(ForgotrRequest);
        }

        public async Task SingoutAsyncs()
        {
            await _accountService.SingoutAsyncs();
        }





    }
}
