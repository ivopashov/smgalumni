using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.Models
{
    public class ForumAnswer : IEntity
    {
        public ForumAnswer()
        {
            Comments = new List<ForumComment>();
        }

        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Body { get; set; }
        public virtual User User { get; set; }
        public int Likes { get; set; }
        public virtual List<ForumComment> Comments { get; set; }
    }
}
