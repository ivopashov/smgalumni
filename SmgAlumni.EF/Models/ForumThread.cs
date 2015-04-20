using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.Models
{
    public class ForumThread :IEntity
    {
        public ForumThread()
        {
            Answers = new List<ForumAnswer>();
        }

        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<ForumAnswer> Answers{ get; set; }
    }
}
