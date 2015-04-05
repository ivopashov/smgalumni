﻿using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Data.Repositories
{
    public class ListingRepository : GenericRepository<Listing>
    {
        public ListingRepository(DbContext context)
            : base(context)
        {

        }
    }
}
