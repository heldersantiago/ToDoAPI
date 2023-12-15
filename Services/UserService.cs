using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using TodoApi.Models;
using TodoApi.Services.Interface;

namespace TodoApi.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<UserManagerResponse> RegisterUserAsync(RegisterModel model)
    {
        try
        {
            if (model == null)
            {
                throw new NullReferenceException("Register model is null");
            }
            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Passwords doesnt matches",
                    IsSuccess = false,
                };

            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                var confirmedEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                var encodeEmailToken = Encoding.UTF8.GetBytes(confirmedEmailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodeEmailToken);

            }

            return new UserManagerResponse
            {
                Message = "User created",
                IsSuccess = true
            };

        }
        catch (Exception)
        {
            throw;
        }
    }
}