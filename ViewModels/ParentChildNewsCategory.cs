using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XPGroup.Models.ViewModels
{
    public class ParentChildNewsCategory
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int SortOrder { get; set; }

        public List<ParentChildNewsCategory> ChildCategories { get; set; }

        public List<News> News { get; set; }

        public ParentChildNewsCategory()
        {
            ChildCategories = new List<ParentChildNewsCategory>();
        }
    }
}