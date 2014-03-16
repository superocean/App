using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XPGroup.Models.ViewModels
{
    public class ParentChildCategory
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int SortOrder { get; set; }

        public List<ParentChildCategory> ChildCategories { get; set; }

        public List<Product> Products { get; set; }

        public ParentChildCategory()
        {
            ChildCategories = new List<ParentChildCategory>();
        }
    }
}