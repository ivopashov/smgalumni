﻿using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Data.Repositories
{
    public class CauseRepository : GenericRepository<Cause>
    {
        public CauseRepository(DbContext context)
            : base(context)
        {

        }
    }
}