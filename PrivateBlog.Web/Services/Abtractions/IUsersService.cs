using Microsoft.AspNetCore.Identity;
using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.Data.Entities;
using UnidadResidencial.Web.DTOs;

namespace UnidadResidencial.Web.Services.Abtractions
{
    public interface IUsersService
    {
        public Task<Response<IdentityResult>> AddUserAsync(User user, string password);
        public Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token);
        public bool CurrentUserIsAuthenticaded();
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        public Task<Response<string>> GenerateConfirmationTokenAsync(User user);
        public Task<User> GetUserByEmailasync(string email);
        public Task<Response<SignInResult>> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
        public Task<Response<AccountUserDTO>> UpdateUserAsync(AccountUserDTO dto);
    }
}
