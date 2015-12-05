using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmgAlumni.Data.Repositories
{
    public class NewsLetterCandidateRepository : INewsLetterCandidateRepository
    {
        private readonly SmgAlumniContext _context;

        public NewsLetterCandidateRepository(SmgAlumniContext context)
        {
            _context = context;
        }

        public int Add(NewsLetterCandidate entity)
        {
            _context.NewsLetterCandidates.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(NewsLetterCandidate entity)
        {
            _context.NewsLetterCandidates.Remove(entity);
            Save();
        }

        public NewsLetterCandidate GetById(int id)
        {
            return _context.NewsLetterCandidates.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(NewsLetterCandidate entity)
        {
            var oldEntity = GetById(entity.Id);
            if (oldEntity == null)
            {
                throw new Exception("Could not find searched for object of type" + typeof(NewsLetterCandidate) + " with id " + entity.Id);
            }

            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            Save();
        }

        public IEnumerable<NewsLetterCandidate> GetUnsent()
        {
            return _context.NewsLetterCandidates.Where(a => !a.Sent);
        }

        public IEnumerable<NewsLetterCandidate> GetUnsentOfType(NewsLetterItemType type)
        {
            return _context.NewsLetterCandidates.Where(a => !a.Sent && type == a.Type);
        }


        public IEnumerable<NewsLetterCandidate> GetOfType(NewsLetterItemType type)
        {
            return _context.NewsLetterCandidates.Where(a => a.Type == type);
        }
    }
}
