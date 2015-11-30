using SmgAlumni.EF.Models.enums;
using System;

namespace SmgAlumni.EF.Models
{
    public class Notification : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime SentOn { get; set; }
        public int Retries { get; set; }
        public bool Sent { get; set; }
        public byte[] Message { get; set; }
        public string HtmlMessage { get; set; }
        public string To { get; set; }
        public NotificationKind Kind { get; set; }
        public int Priority
        {
            set
            {
                switch (Kind)
                {
                    case NotificationKind.ForgotPassword:
                        value = 0;
                        break;
                    case NotificationKind.UserVerified:
                        value = 1;
                        break;
                }
            }
        }
    }
}
