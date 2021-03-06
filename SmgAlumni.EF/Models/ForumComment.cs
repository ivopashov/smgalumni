﻿using System;

namespace SmgAlumni.EF.Models
{
    public class ForumComment : IEntity
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ForumAnswerId { get; set; }
        public virtual ForumAnswer ForumAnswer { get; set; }
    }
}
