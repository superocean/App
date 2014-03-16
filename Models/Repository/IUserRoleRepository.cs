using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface IUserRoleRepository
    {
        UserRole GetById(int id);

        List<UserRole> GetAll();

        UserRole Add(UserRole userRole);

        UserRole Update(UserRole userRole);

        UserRole Delete(UserRole userRole);
    }
}