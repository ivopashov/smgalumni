using SmgAlumni.EF.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.Models
{
    public class Role : IEntity
    {
        public int Id { get; set; }
        public RoleType Name { get; set; }
    }
}
