using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin;
using System.Web;
using Microsoft.AspNet.Identity;
using MyNewsWeb.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using MyNewsWeb.Infrastructure;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Diagnostics;
using MyNewsWeb.Helper;

namespace MyNewsWeb.Controllers
{
    public class AdminController : ApiController
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        //ApplicationDbContext context = new ApplicationDbContext();

        public AdminController() : this(new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext())), 
            new ApplicationRoleManager(new RoleStore<IdentityRole>(new ApplicationDbContext())))
        {

        }

        public AdminController(ApplicationUserManager _userManager, ApplicationRoleManager _roleManager)
        {
            UserManager = _userManager;
            RoleManager = _roleManager;
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public IEnumerable<ManageUserModel> GetAllUsers()
        {
            /*var role = new IdentityRole();
            role.Name = "Admin";
            RoleManager.Create(role);
            
            RoleManager.RoleExistsAsync("test");
            */

            List<ManageUserModel> userDetails = new List<ManageUserModel>();
            IEnumerable<ApplicationUser> users = UserManager.Users.ToList();
            foreach (var user in users)
            {
                /*
                try
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var errors in ex.EntityValidationErrors)
                    {
                        foreach (var error in errors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage);
                        }
                    }
                }
                */
                //context.SaveChanges();
                //await UserManager.AddToRoleAsync(user.Id, "Admin");
                ManageUserModel model = new ManageUserModel
                {
                    FirstName = user.UserInfo.FirstName,
                    LastName = user.UserInfo.LastName,
                    Email = user.Email,
                    UserRoles = UserManager.GetRoles(user.Id).ToArray()
                };
                userDetails.Add(model);
            }
            return userDetails;
        }

        [HttpPost]
        public IHttpActionResult SendEmail(EmailInput data)
        {
            try
            {
                EmailHelper mailHelper = new EmailHelper();
                mailHelper.SendEmail(data.Sender, data.Receivers, data.EmailSubject, data.EmailBody);
            }
            catch(Exception ex) { }

            return Ok();
        }
    }
}
