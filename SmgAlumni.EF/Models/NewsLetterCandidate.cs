using SmgAlumni.EF.Models.enums;
using System;

namespace SmgAlumni.EF.Models
{
    public class NewsLetterCandidate : IEntity
    {
        public int Id
        {
            get; set;
        }

        public string HtmlBody { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual User CreatedBy { get; set; }
        public bool Sent { get; set; }
        public DateTimeOffset SentOn { get; set; }
        public NewsLetterItemType Type { get; set; }
    }
}
