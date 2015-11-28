using System;
using System.Collections.Generic;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System.Linq;

namespace SmgAlumni.Data.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SmgAlumniContext _context;

        public NotificationRepository(SmgAlumniContext context)
        {
            _context = context;
        }        
        public int Add(Notification entity)
        {
            _context.Notifications.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(Notification entity)
        {
            _context.Notifications.Remove(entity);
            Save();
        }

        public Notification GetById(int id)
        {
            return _context.Notifications.Find(id);
        }

        public void Update(Notification entity)
        {
            var oldEntity = GetById(entity.Id);
            if (oldEntity == null)
            {
                throw new Exception("Could not find searched for object of type" + typeof(Activity) + " with id " + entity.Id);
            }

            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Notification> GetSentNotifications()
        {
            return _context.Notifications.Where(a => a.Sent).ToList();
        }
    }
}
