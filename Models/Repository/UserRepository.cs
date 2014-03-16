using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        private DbWeb db { get; set; }

        public UserRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public User Add(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }

        public User Update(User user)
        {
            db.Entry(user).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return user;
        }

        public User Delete(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.ToList();
        }

        public User GetById(int id)
        {
            return db.Users.Find(id);
        }
    }
}