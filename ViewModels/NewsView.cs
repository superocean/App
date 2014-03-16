using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XPGroup.Models.ViewModels
{
    public class NewsView
    {
        public int NewsId { get; set; }

        public string Title { get; set; }

        public string CategoryName { get; set; }

        public DateTime Date { get; set; }
    }

    public class NewsViews
    {
        public List<NewsView> Newss { get; set; }

        public int AllItemsCount { get; set; }
    }
}