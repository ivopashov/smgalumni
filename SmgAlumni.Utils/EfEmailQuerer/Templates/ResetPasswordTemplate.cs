﻿using System;
using SmgAlumni.Utils.EfEmailQuerer.Serialization;

namespace SmgAlumni.Utils.EfEmailQuerer.Templates
{
    public class ResetPasswordTemplate : IEmailNotificationTemplate
    {
        public string Subject { get { return "Възстановяване на парола"; } }

        public string Template
        {
            get
            {
                return String.Format(@"<p>Здравейте, {0}</p>
                        <p>В системата постъпи искане за възстановяване на забравена парола. Ако сте го генерирали Вие последвайте този <a href='{1}'>линк</a> за да го направите. </p>
                        <p>Ако не сте, може да игнорирате този имейл.</p>", UserName, Link);
            }
        }
        public object Data
        {
            get { return new { UserName, Link }; }
        }

        public string UserName { get; set; }
        public string Link { get; set; }
    }
}
