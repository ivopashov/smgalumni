﻿using SmgAlumni.EF.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.Models
{
    public class Notification : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Sent { get; set; }
        public byte[] Message { get; set; }
        public NotificationKind Kind { get; set; }
        public int Priority
        {
            set
            {
                if (Kind == NotificationKind.ForgotPassword) value = 0;
                if (Kind == NotificationKind.UserVerified) value = 1;
            }
        }
    }
}