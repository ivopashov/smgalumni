using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int Likes { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ForumThreadId { get; set; }
        public virtual ForumThread ForumThread{ get; set; }

        public virtual List<ForumComment> Comments { get; set; }
    }
}
