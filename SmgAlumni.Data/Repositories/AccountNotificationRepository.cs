using System;
using System.Collections.Generic;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System.Linq;

namespace SmgAlumni.Data.Repositories
{
    public class AccountNotificationRepository : IAccountNotificationRepository
    {
        private readonly SmgAlumniContext _context;

        public AccountNotificationRepository(SmgAlumniContext context)
        {
            _context = context;
        }
        public int Add(Notification entity)
        {
            _context.PendingNotifications.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(Notification entity)
        {
            _context.PendingNotifications.Remove(entity);
            Save();
        }

        public Notification GetById(int id)
        {
            return _context.PendingNotifications.Find(id);
        }

        public void Update(Notification entity, bool save = true)
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

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Notification> GetUnSentNotifications()
        {
            return _context.PendingNotifications.Where(a => !a.Sent).ToList();
        }
    }
}
