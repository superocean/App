using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class AttributeRepository : IAttributeRepository
    {
        private DbWeb db { get; set; }

        public AttributeRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public Attribute Add(Attribute attribute)
        {
            db.Attributes.Add(attribute);
            db.SaveChanges();
            return attribute;
        }

        public Attribute Update(Attribute attribute)
        {
            db.Entry(attribute).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return attribute;
        }

        public Attribute Delete(Attribute attribute)
        {
            db.Attributes.Remove(attribute);
            db.SaveChanges();
            return attribute;
        }

        public List<Attribute> GetAll()
        {
            return db.Attributes.Include("Category").Include("Attributes").ToList();
        }

        public Attribute GetById(int id)
        {
            return db.Attributes.Find(id);
        }
    }
}