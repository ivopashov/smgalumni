using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly SmgAlumniContext _context;

        public ListingRepository(SmgAlumniContext context)
        {
            _context = context;
        }
        public int Add(Listing entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public Listing GetById(int id)
        {
            return this._context.Listings.Find(id);
        }

        public void Delete(Listing entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Update(Listing entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Save()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}
