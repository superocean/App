using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class UserInRolesRepository : IUserInRolesRepository
    {
        private DbWeb db { get; set; }

        public UserInRolesRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public UserInRoles Add(UserInRoles user)
        {
            db.UsersInRoles.Add(user);
            db.SaveChanges();
            return user;
        }

        public UserInRoles Update(UserInRoles user)
        {
            db.Entry(user).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return user;
        }

        public UserInRoles Delete(UserInRoles user)
        {
            db.UsersInRoles.Remove(user);
            db.SaveChanges();
            return user;
        }

        public IEnumerable<UserInRoles> GetAll()
        {
            return db.UsersInRoles.ToList();
        }

        public UserInRoles GetById(int id)
        {
            return db.UsersInRoles.Find(id);
        }
    }
}