using SmgAlumni.EF.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.Models
{
    public class User
    {
        //basic identification
        public int Id { get; set; }
        public string UserName { get; set; }
        public int YearOfGraduation { get; set; }
        public ClassDivision Division { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        //optional
        public Country DwellingCountry { get; set; }
        public string UniversityGraduated { get; set; }
        public string Profession { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        //for admin usage
        public bool Verified { get; set; }
        public DateTime DateJoined { get; set; }

        //image
        public byte[] AvatarImage { get; set; }

        //role
        public virtual Role Role { get; set; }
    }
}
