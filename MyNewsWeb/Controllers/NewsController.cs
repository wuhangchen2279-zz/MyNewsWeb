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
using System.Threading.Tasks;
using System.Collections;
using System.IO;

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


        //for admin and author to manage news
        [Authorize(Roles = "Admin,Journalist")]
        public ActionResult ManageNews()  
        {
            var userId = User.Identity.GetUserId();
            var currentUser = UserManager.FindById(userId);
            List<NewsViewModel> newsViewModels = new List<NewsViewModel>();

            if (UserManager.IsInRole(userId, "Admin"))
            {
                var newslist = (from user in db.UserInfos
                            join news in db.GoodNews on user.Id equals news.UserInfoId
                            select new { news.Id, news.Title, news.Content, news.NewsType, news.NewsDate, news.UserInfoId, news.FileName, user.FirstName, user.LastName }
                            ).ToList();
                foreach (var item in newslist)
                {
                    newsViewModels.Add(new NewsViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Content = item.Content,
                        FileName = item.FileName,
                        NewsType = item.NewsType,
                        NewsDate = item.NewsDate,
                        UserInfoId = item.UserInfoId,
                        FirstName = item.FirstName,
                        LastName = item.LastName
                    });
                }
            }
            else
            {
               var newslist = (from user in db.UserInfos
                                join news in db.GoodNews on user.Id equals news.UserInfoId
                               where news.UserInfoId == currentUser.UserInfo.Id
                               select new { news.Id, news.Title, news.Content, news.NewsType, news.NewsDate, news.UserInfoId, news.FileName, user.FirstName, user.LastName }
                            ).ToList();

                foreach (var item in newslist)
                {
                    newsViewModels.Add(new NewsViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Content = item.Content,
                        FileName = item.FileName,
                        NewsType = item.NewsType,
                        NewsDate = item.NewsDate,
                        UserInfoId = item.UserInfoId,
                        FirstName = item.FirstName,
                        LastName = item.LastName
                    });
                }
            }
            return View(newsViewModels);
        }

        private IDictionary<int, string> GetAuthorsFromDB()
        {
            IDictionary<int, string> authorsDict = new Dictionary<int, string>();
            var users = UserManager.Users.ToList();
            foreach(var user in users) 
            {
                authorsDict.Add(user.UserInfo.Id, user.UserInfo.FirstName + " " + user.UserInfo.LastName);
            }
            return authorsDict;
            
        }
        [Authorize(Roles = "Admin,Journalist")]
        // GET: Artiles/Create
        public ActionResult Create()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            NewsViewModel news = new NewsViewModel();
            ViewBag.IsUserAdmin =  UserManager.IsInRole(User.Identity.GetUserId(), "Admin");
            news.Authors = GetAuthorsFromDB();
            news.UserInfoId = currentUser.UserInfo.Id;
            news.FirstName = currentUser.UserInfo.FirstName;
            news.LastName = currentUser.UserInfo.LastName;
            news.NewsTypes = new Dictionary<string, string>
            {
                {"SchoolLife", "School Life" },
                {"BreakingNews", "Break News" },
                {"Entertainment", "Entertainment" },
                {"Sports", "Sports" }
            };
            return View(news);
        }
        // POST: Artiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Journalist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsViewModel news, HttpPostedFileBase newsFile)
        {
            if (ModelState.IsValid)
            {
                if (newsFile != null)
                {
                    string path = Server.MapPath("~/Content/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    newsFile.SaveAs(path + Path.GetFileName(newsFile.FileName));
                    news.FileName = newsFile.FileName;
                }

                GoodNew goodNew = new GoodNew
                {
                    Title = news.Title,
                    Content = news.Content,
                    NewsType = news.NewsType,
                    NewsDate = DateTime.Now,
                    FileName = news.FileName,
                    UserInfoId = news.UserInfoId
                };
                db.GoodNews.Add(goodNew);
                db.SaveChanges();
                return RedirectToAction("ManageNews");
            }

            return View(news);
        }

        // GET: Artiles/Edit/5
        [Authorize(Roles = "Admin,Journalist")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GoodNew selectedNews = db.GoodNews.Find(id);
            if(selectedNews == null)
            {
                return HttpNotFound();
            }

            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            NewsViewModel news = new NewsViewModel();
            ViewBag.IsUserAdmin = UserManager.IsInRole(User.Identity.GetUserId(), "Admin");
            news.Authors = GetAuthorsFromDB();
            news.UserInfoId = currentUser.UserInfo.Id;
            news.FirstName = currentUser.UserInfo.FirstName;
            news.LastName = currentUser.UserInfo.LastName;
            news.NewsTypes = new Dictionary<string, string>
            {
                {"SchoolLife", "School Life" },
                {"BreakingNews", "Break News" },
                {"Entertainment", "Entertainment" },
                {"Sports", "Sports" }
            };
            news.NewsType = selectedNews.NewsType;
            news.Title = selectedNews.Title;
            news.Content = selectedNews.Content;
            news.Id = selectedNews.Id;
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Journalist")]
        public ActionResult Edit(NewsViewModel news)
        {
            if (ModelState.IsValid)
            {
                GoodNew selectedNews = db.GoodNews.Where(x => x.Id == news.Id).SingleOrDefault();
                selectedNews.UserInfoId = news.UserInfoId;
                selectedNews.Title = news.Title;
                selectedNews.NewsType = news.NewsType;
                selectedNews.Content = news.Content;
                db.Entry(selectedNews).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageNews");
            }
            return View(news);
        }

        [Authorize(Roles = "Admin,Journalist")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodNew selectednews = db.GoodNews.Find(id);
            if (selectednews == null)
            {
                return HttpNotFound();
            }
            NewsViewModel news = new NewsViewModel();
            news.Id = selectednews.Id;
            news.Title = selectednews.Title;
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Journalist")]
        public ActionResult Delete(int id)
        {
            GoodNew news = db.GoodNews.Find(id);
            db.GoodNews.Remove(news);
            db.SaveChanges();
            return RedirectToAction("ManageNews");
        }
    }
}