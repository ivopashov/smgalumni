using Microsoft.Owin.Security;
using SmgAlumni.EF.Models;
using System.Security.Claims;

namespace SmgAlumni.ServiceLayer.Interfaces
{
    public interface IUserService
    {
        AuthenticationTicket CreateAuthenticationTicket(ClaimsIdentity identity);
        ClaimsIdentity CreateIdentity(string username, string authenticationType);
        void CreateUser(User user, bool receiveNewsletter);
        void UpdateUserNewsLetterNotification(bool subscribe, string username);
    }
}