using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface IAttributeOptionRepository
    {
        AttributeOption GetById(int id);

        List<AttributeOption> GetAll();

        AttributeOption Add(AttributeOption attribute);

        AttributeOption Update(AttributeOption attribute);

        AttributeOption Delete(AttributeOption attribute);

        bool DeleteAllByAttributeId(int id);
    }
}