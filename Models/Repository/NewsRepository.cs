using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class NewsRepository : INewsRepository
    {
        private DbWeb db { get; set; }

        public NewsRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public News Add(News news)
        {
            db.Newss.Add(news);
            db.SaveChanges();
            return news;
        }

        public News Update(News news)
        {
            db.Entry(news).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return news;
        }

        public News Delete(News news)
        {
            db.Newss.Remove(news);
            db.SaveChanges();
            return news;
        }

        public List<News> GetAll()
        {
            return db.Newss.ToList();
        }

        public News GetById(int id)
        {
            return db.Newss.Find(id);
        }

        public List<NewsView> GetNewsView()
        {
            List<NewsView> newss = new List<NewsView>();
            foreach (var item in db.Newss.OrderByDescending(p => p.Date).ToList())
            {
                newss.Add(new NewsView()
                {
                    NewsId = item.NewsId,
                    Title = item.Title,
                    CategoryName = item.Category.Name,
                    Date = item.Date
                });
            }
            return newss;
        }

        public NewsViews GetNewsPaging(int pagesize, int index, int categoryId = -1, string searchString = "")
        {
            #region newss

            List<News> newss = new List<News>();
            searchString = searchString.ToLower();
            int totalNews = 1;
            if (categoryId == -1)
            {
                #region category -1

                if (searchString == "")
                {
                    totalNews = db.Newss.Count();
                    newss = db.Newss.OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                }
                else
                {
                    totalNews = db.Newss.Where(c => c.Title.Contains(searchString)).Count();
                    newss = db.Newss.Where(c => c.Title.Contains(searchString)).OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                }

                #endregion category -1
            }
            else
            {
                #region category !=-1

                if (searchString == "")
                {
                    totalNews = db.Newss.Where(c => c.CategoryId == categoryId).Count();
                    newss = db.Newss.Where(c => c.CategoryId == categoryId).OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                }
                else
                {
                    totalNews = db.Newss.Where(c => c.CategoryId == categoryId).Where(c => c.Title.Contains(searchString)).Count();
                    newss = db.Newss.Where(c => c.CategoryId == categoryId).Where(c => c.Title.Contains(searchString)).OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                }

                #endregion category !=-1
            }

            #endregion newss

            List<NewsView> newsviews = new List<NewsView>();
            foreach (var item in newss)
            {
                newsviews.Add(new NewsView()
                {
                    NewsId = item.NewsId,
                    Title = item.Title,
                    CategoryName = item.Category.Name,
                    Date = item.Date,
                });
            }
            NewsViews newsview = new NewsViews();
            newsview.Newss = newsviews;
            newsview.AllItemsCount = totalNews;
            return newsview;
        }
    }
}