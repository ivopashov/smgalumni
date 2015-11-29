using SmgAlumni.EF.Models.enums;
using System;

namespace SmgAlumni.EF.Models
{
    public class NotificationSubscription
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public Guid UnsubscribeToken { get; set; }
        public virtual User User { get; set; }
        public NotificationKind NotificationKind { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
