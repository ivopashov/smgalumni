using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> UsersByDivisionAndYearOfGraduation(ClassDivision division, int year);
        IEnumerable<User> UsersByUserName(string username);
        IEnumerable<User> UsersByName(string name);
        IEnumerable<User> UsersByEmail(string email);
        IEnumerable<User> VerifiedUsers();
        IEnumerable<User> UnVerifiedUsers();
    }
}
