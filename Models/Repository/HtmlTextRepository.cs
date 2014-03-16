using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class HtmlTextRepository : IHtmlTextRepository
    {
        private DbWeb db { get; set; }

        public HtmlTextRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public HtmlText Add(HtmlText htmlText)
        {
            db.HtmlTexts.Add(htmlText);
            db.SaveChanges();
            return htmlText;
        }

        public HtmlText Update(HtmlText htmlText)
        {
            db.Entry(htmlText).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return htmlText;
        }

        public HtmlText Delete(HtmlText htmlText)
        {
            db.HtmlTexts.Remove(htmlText);
            db.SaveChanges();
            return htmlText;
        }

        public IEnumerable<HtmlText> GetAll()
        {
            return db.HtmlTexts.ToList();
        }

        public HtmlText GetByName(string name)
        {
            return db.HtmlTexts.Where(c => c.Name == name).FirstOrDefault();
        }
    }
}