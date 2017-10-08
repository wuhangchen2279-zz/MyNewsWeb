using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyNewsWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MyNewsWeb.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public NewsController()
        {
        }

        public NewsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //GET: News

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var currentUser = UserManager.FindById(userId);
            var news = db.GoodNews.Include(n => n.UserInfo);
            List<NewsIndexViewModel> newsViewModel = new List<NewsIndexViewModel>();
            foreach (GoodNew goodNew in news)
            {
                newsViewModel.Add(new NewsIndexViewModel
                {
                    Title = goodNew.Title,
                    Content = goodNew.Content,
                    FileName = goodNew.FileName,
                    NewsType = goodNew.NewsType,
                    NewsDate = goodNew.NewsDate,
                    UserInfoId = currentUser.UserInfo.Id,
                    FirstName = currentUser.UserInfo.FirstName,
                    LastName = currentUser.UserInfo.LastName
                });
            }
            return View(newsViewModel.OrderBy(t => t.NewsDate));
        }


        // GET: Artiles/Create
        public ActionResult CreateIndividual()
        {
            GoodNew news = new GoodNew();
            string userId = User.Identity.GetUserId();
            var currentUser = _userManager.FindById(userId);

            news.UserInfoId = currentUser.UserInfo.Id;
            return View(news);
        }
        // POST: Artiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIndividual([Bind(Include = "Id,Title,NewsDate,UserInfoId,Content,NewsType,FileName")] GoodNew news)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var currentUser = _userManager.FindById(userId);
                news.UserInfoId = currentUser.UserInfo.Id;
                db.GoodNews.Add(news);
                db.SaveChanges();
                return RedirectToAction("IndexUserNames");
            }

            return View(news);
        }
    }
}