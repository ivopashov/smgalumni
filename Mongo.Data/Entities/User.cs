using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Data.Entities
{
    public class User : Entity
    {
        public User()
        {
            Roles = new List<string>();
            PasswordResets = new List<PasswordReset>();
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public List<string> Roles { get; set; }
        public List<PasswordReset> PasswordResets { get; set; }
    }

    public class PasswordReset
    {
        public Guid Guid { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUsed { get; set; }
        public bool Used { get; set; }
    }
}
