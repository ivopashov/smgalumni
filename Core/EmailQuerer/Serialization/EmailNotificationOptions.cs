﻿namespace SmgAlumni.Utils.EmailQuerer.Serialization
{
    public class EmailNotificationOptions
    {
        public string To { get; set; }
        public IEmailNotificationTemplate Template { get; set; }
    }
}
