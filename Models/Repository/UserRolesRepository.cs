using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private DbWeb db { get; set; }

        public UserRoleRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public UserRole Add(UserRole user)
        {
            db.UserRoles.Add(user);
            db.SaveChanges();
            return user;
        }

        public UserRole Update(UserRole user)
        {
            db.Entry(user).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return user;
        }

        public UserRole Delete(UserRole user)
        {
            db.UserRoles.Remove(user);
            db.SaveChanges();
            return user;
        }

        public List<UserRole> GetAll()
        {
            return db.UserRoles.ToList();
        }

        public UserRole GetById(int id)
        {
            return db.UserRoles.Find(id);
        }
    }
}