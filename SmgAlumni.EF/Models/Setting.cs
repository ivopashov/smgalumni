using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.Models
{
    public class Setting : IEntity
    {
        public int Id { get; set; }
        public string SettingKey { get; set; }
        public string SettingName { get; set; }
    }
}
