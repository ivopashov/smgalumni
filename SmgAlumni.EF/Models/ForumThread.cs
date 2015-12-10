using System;
using System.Collections.Generic;

namespace SmgAlumni.EF.Models
{
    public class ForumThread : IEntity
    {
        public ForumThread()
        {
            Answers = new List<ForumAnswer>();
            Tags = new List<Tag>();
        }

        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<ForumAnswer> Answers { get; set; }
        public virtual List<Tag> Tags { get; set; }
    }
}
