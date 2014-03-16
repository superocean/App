using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface IUserRepository
    {
        User GetById(int id);

        IEnumerable<User> GetAll();

        User Add(User user);

        User Update(User user);

        User Delete(User user);
    }
}