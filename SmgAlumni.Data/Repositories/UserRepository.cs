﻿using System;
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
            var result = _context.Users.Where(a => a.UserName.Equals(username));
            return result.ToList();
        }

        public IEnumerable<User> UsersByName(string name)
        {
            var trimmedname = RemoveWhiteSpace(name);
            var loweredName = name.ToLower();

            var result = _context.Users.Where(a =>
                a.FirstName.ToLower().Equals(loweredName)
                || a.MiddleName.ToLower().Equals(loweredName)
                || a.LastName.ToLower().Equals(loweredName)
             || (a.FirstName + a.MiddleName + a.LastName).ToLower().Equals(trimmedname.ToLower()));

            return result.ToList();
        }

        public IEnumerable<User> UsersByEmail(string email)
        {
            var result = _context.Users.Where(a => a.Email.ToLower().Equals(email.ToLower()));
            return result.ToList();
        }

        public IEnumerable<User> UnSubscribedUsersToNewsLetter()
        {
            return _context.Users.Where(a => !a.AddedToNewsLetterList).ToList();
        }

        private string RemoveWhiteSpace(string input)
        {
            var result = input.ToCharArray().Where(ch => !char.IsWhiteSpace(ch)).ToArray();
            return new string(result);
        }
    }
}
