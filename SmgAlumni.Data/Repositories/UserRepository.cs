using System;
using System.Collections.Generic;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using System.Linq;

namespace SmgAlumni.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SmgAlumniContext _context;

        public UserRepository(SmgAlumniContext context)
        {
            _context = context;
        }
        public int Add(User entity)
        {
            _context.Users.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
            Save();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Update(User entity, bool save = true)
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

        public IEnumerable<User> UsersByDivisionAndYearOfGraduation(ClassDivision division, int year)
        {
            return _context.Users.Where(a => a.Division == division && a.YearOfGraduation == year).ToList();
        }

        public IEnumerable<User> UsersByUserName(string username)
        {
            return _context.Users.Where(a => a.UserName.Equals(username)).ToList();
        }

        public IEnumerable<User> UsersByName(string name)
        {
            return _context.Users.Where(a => a.FirstName.ToLower().Equals(name.ToLower()) || a.MiddleName.ToLower().Equals(name.ToLower()) || a.LastName.ToLower().Equals(name.ToLower())).ToList();
        }

        public IEnumerable<User> UsersByEmail(string email)
        {
            return _context.Users.Where(a => a.Email.ToLower().Equals(email.ToLower())).ToList();
        }

        public IEnumerable<User> UnSubscribedUsersToNewsLetter()
        {
            return _context.Users.Where(a => !a.AddedToNewsLetterList).ToList();
        }
    }
}
