using TodoApi.Models;

namespace TodoApi.Services.Interface;

public interface IUserService
{
    Task<UserManagerResponse> RegisterUserAsync(RegisterModel model);
}