using SmgAlumni.EF.Models;
using System;

namespace SmgAlumni.ServiceLayer.Interfaces
{
    public interface IAccountService
    {
        bool ValidatePassword(User user, string password);
        void ChangePassword(string username, string password);
        bool AddResetPassRequest(User user, Guid guid);
        void ResetPasswordBasedOnToken(User user, Guid token, string password);
        bool IsPasswordResetTokenValid(User user, Guid token);
    }
}
