﻿using System;
using System.Linq;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System.Collections.Generic;

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
            _context.Listings.Add(entity);
            Save();
            return entity.Id;
        }

        public Listing GetById(int id)
        {
            return _context.Listings.Find(id);
        }

        public void Delete(Listing entity)
        {
            _context.Listings.Remove(entity);
            Save();
        }

        public void Update(Listing entity, bool save = true)
        {
            var oldEntity = GetById(entity.Id);
            if (oldEntity == null)
            {
                throw new Exception("Could not find searched for object of type" + typeof(Activity) + " with id " + entity.Id);
            }

            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            if (save)
            {
                Save();
            }
        }

        public int GetCount(int? userId=null)
        {
            if (userId == null)
            {
                return _context.Listings.Count();
            }
            else
            {
                return _context.Listings.Where(a=>a.User.Id==userId).Count();
            }
        }

        public IEnumerable<Listing> PageListingForUser(int id,int skip, int take, bool orderByDesc = true)
        {
            if (orderByDesc)
            {
                return _context.Listings.Where(listing => listing.User.Id == id).OrderByDescending(a => a.DateCreated).Skip(skip).Take(take).ToList();
            }
            return _context.Listings.Where(listing => listing.User.Id == id).OrderBy(a => a.DateCreated).Skip(skip).Take(take).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Listing> Page(int skip, int take, bool orderByDescDate = true)
        {
            if (orderByDescDate)
            {
                return _context.Listings.OrderByDescending(a => a.DateCreated).Skip(skip).Take(take).ToList();
            }

            return _context.Listings.OrderBy(a => a.DateCreated).Skip(skip).Take(take).ToList();
        }
    }
}
