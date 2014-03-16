using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface IAttributeRepository
    {
        Attribute GetById(int id);

        List<Attribute> GetAll();

        Attribute Add(Attribute attribute);

        Attribute Update(Attribute attribute);

        Attribute Delete(Attribute attribute);
    }
}