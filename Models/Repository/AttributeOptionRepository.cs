using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class AttributeOptionRepository : IAttributeOptionRepository
    {
        private DbWeb db { get; set; }

        public AttributeOptionRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public AttributeOption Add(AttributeOption attribute)
        {
            db.AttributeOptions.Add(attribute);
            db.SaveChanges();
            return attribute;
        }

        public AttributeOption Update(AttributeOption attribute)
        {
            db.Entry(attribute).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return attribute;
        }

        public AttributeOption Delete(AttributeOption attribute)
        {
            db.AttributeOptions.Remove(attribute);
            db.SaveChanges();
            return attribute;
        }

        public List<AttributeOption> GetAll()
        {
            return db.AttributeOptions.ToList();
        }

        public AttributeOption GetById(int id)
        {
            return db.AttributeOptions.Find(id);
        }

        public bool DeleteAllByAttributeId(int id)
        {
            try
            {
                List<AttributeOption> attributeOptions = db.AttributeOptions.Where(c => c.AttributeId == id).ToList();
                foreach (var item in attributeOptions)
                {
                    Delete(item);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}