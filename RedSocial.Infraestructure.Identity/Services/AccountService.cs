using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RedSocial.Core.Application.Dtos.Account;
using RedSocial.Core.Application.Enum;
using RedSocial.Core.Application.Interfaces.Services;
using RedSocial.Core.Domain.Entity;
using RedSocial.Core.Domain.Settings;
using RedSocial.Infraestructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel;

namespace RedSocial.Infraestructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _jwtSettings;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IEmailService emailservice, IOptions<JwtSettings> jwtsettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailservice;
            _jwtSettings = jwtsettings.Value;
        }


        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleclaim = new List<Claim>();

            foreach (var role in roles)
            {
                roleclaim.Add(new Claim("roles", role));
            }

            var claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleclaim);

            var SymetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var SinginCredential = new SigningCredentials(SymetricKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken
                (
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claim,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    signingCredentials: SinginCredential
                );
           
               
            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,

            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoProvider = new RNGCryptoServiceProvider();
            var randombyte = new byte[40];
            rngCryptoProvider.GetBytes(randombyte);

            return BitConverter.ToString(randombyte).Replace("-", "");
        }

        public async Task<AuthenticationResponse> AutheticationAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts register with {request.Email}";
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid Credentials for {request.Email}";
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"The email is not confirmed for {request.Email}";
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
            

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.ProfilePictureUrl = user.ProfilePictureUrl;
            response.JwToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshtoken = GenerateRefreshToken();
            response.RefreshToken = refreshtoken.Token;

            var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = roleList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task SingoutAsyncs()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var UserSameName = await _userManager.FindByNameAsync(request.UserName);
            if (UserSameName != null)
            {
                response.HasError = true;
                response.Error = $"The UserName '{request.UserName}' already exists";
                return response;
            }

            var UserSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (UserSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"The email '{request.Email}' is already register";
                return response;
            }

            var user = new ApplicationUser()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.Phone,
                ProfilePictureUrl = request.ProfilePictureUrl
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.BASIC.ToString());
                var verificationurl = await SendVerificationEmailurlAsync(user, origin);
                await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
                {
                    To = user.Email,
                    Body = $"Confirme Your Account visiting this URL {verificationurl}",
                    Subject = "Confirm Registration"
                });
            }
            else
            {
                response.HasError = true;
                response.Error = $"A error has ocurred in the register";
                return response;
            }

            return response;
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };


            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"Account {request.Email} not found";
                return response;
            }

            var verificationurl = await SendForgotPasswordUriAsync(user, origin);
            await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
            {
                To = user.Email,
                Body = $"Reset your password visiting this URL {verificationurl}",
                Subject = "Reset your password"
            });


            return response;
        }

        private async Task<string> SendForgotPasswordUriAsync(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "User/ResetPassword";

            var uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUrl = QueryHelpers.AddQueryString(uri.ToString(), "token", code);


            return verificationUrl;

        }

        private async Task<string> SendVerificationEmailurlAsync(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "User/ConfirmEmail";

            var uri = new Uri(string.Concat($"{origin}/", route));

            var verificationUrl = QueryHelpers.AddQueryString(uri.ToString(), "userId", user.Id);
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "token", code);


            return verificationUrl;

        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"{user.Id} is not register";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for{user.Email}, now you can use de app";
            }
            else
            {
                return $"An error ocurred confirmin the email:{user.Email}";
            }
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsyncs(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"Account {request.Email} not found";
                return response;
            }
            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error ocurred while reset password for:{user.Email}";
            }

            return response;
        }

        public async Task<AuthenticationResponse> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            return new AuthenticationResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                UserName = user.UserName
            };
        }

        public async Task<List<AuthenticationResponse>> GetUsersByIdsAsync(List<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            return users.Select(user => new AuthenticationResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfilePictureUrl = user.ProfilePictureUrl
            }).ToList();
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user.Id;
        }
    }
}
