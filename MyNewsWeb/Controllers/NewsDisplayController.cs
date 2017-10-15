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

        public IEnumerable<NewsUserCommentsViewModel> GetAllNewsWithComments()
        {
            List<NewsUserCommentsViewModel> newsList = new List<NewsUserCommentsViewModel>();
            var datalist = (from user in context.UserInfos
                            join comment in context.NewsComments on user.Id equals comment.UserInfoId
                            join news in context.GoodNews on comment.GoodNewId equals news.Id
                            select new
                            {
                                news.Id,
                                news.Title,
                                news.Content,
                                news.NewsType,
                                news.NewsDate,
                                news.UserInfoId,
                                news.FileName,
                                user.FirstName,
                                user.LastName,
                                comment.Comment
                            }).ToList();
            foreach (var item in datalist)
            {
                newsList.Add(new NewsUserCommentsViewModel
                {
                    Id = item.Id,
                    
                });
            }
            return newsList;
        }
    }
}
