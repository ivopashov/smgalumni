using System;
using Core.EmailQuerer.Serialization;

namespace Core.EmailQuerer.Templates
{
    public class ResetPasswordTemplate : IEmailNotificationTemplate
    {
        public string Subject { get { return "Reset Password"; } }

        public string Template
        {
            get
            {
                return String.Format(@"<p>Dear {0}</p>
                        <p>A request to reset your password was generated in our system. If you generated that request, please follow the <a href='{1}'>link</a> to do that. </p>
                        <p>If you didn't generate that email, please ignore this email.</p>", UserName, Link);
            }
        }
        public object Data {
            get { return new {UserName, Link}; }
        }

        public string UserName { get; set; }
        public string Link { get; set; }
    }
}
