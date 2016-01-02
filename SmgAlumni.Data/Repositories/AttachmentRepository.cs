using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SmgAlumni.Data.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly SmgAlumniContext _context;

        public AttachmentRepository(SmgAlumniContext context)
        {
            _context = context;
        }

        public int Add(Attachment entity)
        {
            _context.Attachments.Add(entity);
            Save();
            return entity.Id;
        }

        public IEnumerable<Attachment> AttachmentsWithoutParentAndAtLeastOneHourOld()
        {
            var result = _context.Attachments.Where(a => a.Listing == null && DbFunctions.DiffHours(a.CreatedOn, DateTime.Now) >= 1).ToList();
            return result;
        }

        public void Delete(Attachment entity)
        {
            _context.Attachments.Remove(entity);
            Save();
        }

        public Attachment FindByTempKey(Guid tempKey)
        {
            var result = _context.Attachments.Where(a => a.TempKey == tempKey).SingleOrDefault();
            return result;
        }

        public Attachment GetById(int id)
        {
            return _context.Attachments.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Attachment entity, bool save = true)
        {
            var oldEntity = GetById(entity.Id);
            if (oldEntity == null)
            {
                throw new Exception("Could not find searched for object of type" + typeof(Attachment) + " with id " + entity.Id);
            }

            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            if (save)
            {
                Save();
            }
        }
    }
}
