﻿using System;
using System.Collections.Generic;

namespace SmgAlumni.App.Models
{
    public class CauseNewsViewModelWithoutId
    {
        public string Heading { get; set; }
        public string Body { get; set; }
        public List<Guid> TempKeys { get; set; }
    }
}