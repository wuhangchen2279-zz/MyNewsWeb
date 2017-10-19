using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MyNewsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace MyNewsWeb.Controllers
{
    public class NewsDisplayController : ApiController
    {
        private ApplicationUserManager _userManager;
        ApplicationDbContext context = new ApplicationDbContext();

        public NewsDisplayController() : this(new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {

        }

        public NewsDisplayController(ApplicationUserManager _userManager)
        {
            UserManager = _userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AcceptVerbs("GET")]
        [HttpGet]
        public IEnumerable<NewsCommentsViewModel> GetAllCommentsForNews(int newsId)
        {
            var comments = (from user in context.UserInfos
                            join comment in context.NewsComments on user.Id equals comment.UserInfoId
                            where comment.GoodNewId == newsId
                            select new { comment.GoodNewId, comment.Comment, comment.UserInfoId, user.FirstName, user.LastName }).ToList();

            List<NewsCommentsViewModel> commentlist = new List<NewsCommentsViewModel>();
            foreach (var item in comments)
            {
                commentlist.Add(new NewsCommentsViewModel
                {
                    GoodNewId = item.GoodNewId,
                    Comment = item.Comment,
                    UserInfoId = item.UserInfoId,
                    FirstName = item.FirstName,
                    LastName = item.LastName
                });
            }
            return commentlist;
        }

        [HttpPost]
        public NewsCommentsViewModel PostComment(UserCommentViewModel comment)
        {
            //throw new NotImplementedException();
            if (comment == null)
            {
                throw new ArgumentNullException("comment not provied");
            }

            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            NewsComment newsComment = new NewsComment
            {
                GoodNewId = comment.GoodNewId,
                Comment = comment.Comment,
                UserInfoId = currentUser.UserInfo.Id
            };

            context.NewsComments.Add(newsComment);
            context.SaveChanges();
            return new NewsCommentsViewModel
            {
                Comment = comment.Comment,
                GoodNewId = comment.GoodNewId,
                UserInfoId = currentUser.UserInfo.Id,
                FirstName = currentUser.UserInfo.FirstName,
                LastName = currentUser.UserInfo.LastName
            };
        }

        public IEnumerable<NewsViewModel> GetAllNews()
        {
            List<NewsViewModel> newsList = new List<NewsViewModel>();
            var goodnews = context.GoodNews.Include(n => n.UserInfo);
            foreach (var item in goodnews)
            {
                NewsViewModel data = new NewsViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Content = item.Content,
                    FileName = item.FileName,
                    NewsType = item.NewsType,
                    NewsDate = item.NewsDate,
                    UserInfoId = item.UserInfoId,
                    FirstName = item.UserInfo.FirstName,
                    LastName = item.UserInfo.LastName,
                    IsAuthenticate = HttpContext.Current.User.Identity.IsAuthenticated
                };
                newsList.Add(data);

            }
            return newsList;
        }

        public IEnumerable<NewsTypeCountViewModel> GetNewsTypeCount()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            List<NewsTypeCountViewModel> newsTypeCounts = new List<NewsTypeCountViewModel>();

            if (UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
            {
                var newsTypeCountList = (from user in context.UserInfos
                                         join news in context.GoodNews on user.Id equals news.UserInfoId
                                         group news.Id by news.NewsType into g
                                         select new { NewsType = g.Key, NewsTypeCount = g.Count() }).ToList();
                foreach (var typeCount in newsTypeCountList)
                {
                    newsTypeCounts.Add(new NewsTypeCountViewModel
                    {
                        NewsType = typeCount.NewsType,
                        NewsTypeCount = typeCount.NewsTypeCount
                    });
                }
            }
            else
            {
                var newsTypeCountList = (from user in context.UserInfos
                                         join news in context.GoodNews on user.Id equals news.UserInfoId
                                         where news.UserInfoId == currentUser.UserInfo.Id
                                         group news.Id by news.NewsType into g
                                         select new { NewsType = g.Key, NewsTypeCount = g.Count() }).ToList();
                foreach (var typeCount in newsTypeCountList)
                {
                    newsTypeCounts.Add(new NewsTypeCountViewModel
                    {
                        NewsType = typeCount.NewsType,
                        NewsTypeCount = typeCount.NewsTypeCount
                    });
                }
            }

            return newsTypeCounts;
        }
    }
}
