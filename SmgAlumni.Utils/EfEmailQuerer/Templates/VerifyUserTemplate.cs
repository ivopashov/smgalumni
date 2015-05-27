﻿using SmgAlumni.Utils.EfEmailQuerer.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.EfEmailQuerer.Templates
{
    class VerifyUserTemplate : IEmailNotificationTemplate
    {
        public string Subject { get { return "Одобрение за СМГ Алумни"; } }

        public string Template
        {
            get
            {
                return String.Format(@"<p>Здравейте, {0}</p>
                        <p>Вашата регистрация за СМГ Алумни е одобрена. Може да се логнете тук: <a href='smg-alumni.com'>smg-alumni.com</a>. </p>", UserName);
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