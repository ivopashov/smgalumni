using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

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
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Delete(Notification entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public Notification GetById(int id)
        {
            return this._context.Notifications.Find(id);
        }

        public void Update(Notification entity)
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
