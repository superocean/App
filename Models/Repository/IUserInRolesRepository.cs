using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface IUserInRolesRepository
    {
        UserInRoles GetById(int id);

        IEnumerable<UserInRoles> GetAll();

        UserInRoles Add(UserInRoles user);

        UserInRoles Update(UserInRoles user);

        UserInRoles Delete(UserInRoles user);
    }
}