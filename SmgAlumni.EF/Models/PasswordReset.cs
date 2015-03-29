using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.Models
{
    public class PasswordReset : IEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Used { get; set; }
        public virtual User User { get; set; }
        public Guid Guid { get; set; }
    }
}
