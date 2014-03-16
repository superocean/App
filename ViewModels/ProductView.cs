using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XPGroup.Models.ViewModels
{
    public class ProductView
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public bool IsShowHomePage { get; set; }

        public bool IsShowBanner { get; set; }
    }

    public class ProductViews
    {
        public List<ProductView> Products { get; set; }

        public int AllItemsCount { get; set; }
    }
}