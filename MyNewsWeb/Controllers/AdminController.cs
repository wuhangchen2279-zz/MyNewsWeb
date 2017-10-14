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
          
        ApplicationDbContext context = new ApplicationDbContext();

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
            
            List<ManageUserModel> userDetails = new List<ManageUserModel>();
            IEnumerable<ApplicationUser> users = UserManager.Users.ToList();
            foreach (var user in users)
            {
                //if(!User.Identity.GetUserId().Equals(user.Id))
                //{
                    ManageUserModel model = new ManageUserModel
                    {
                        Id = user.Id,
                        FirstName = user.UserInfo.FirstName,
                        LastName = user.UserInfo.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        DateOfBirth = user.UserInfo.DateOfBirth,
                        Street = user.UserInfo.Street,
                        Suburb = user.UserInfo.Suburb,
                        State = user.UserInfo.State,
                        PostCode = user.UserInfo.PostCode,
                        FileName = user.UserInfo.FileName,
                        UserRoles = generateMyRolesArr(UserManager.GetRoles(user.Id))
                    };
                    userDetails.Add(model);
                //}
                
            }
            return userDetails;
        }

        private MyRole[] generateMyRolesArr(IList<string> roles)
        {
            IList<MyRole> myroles = new List<MyRole>();
            foreach(string roleName in roles)
            { 
                string id = string.Empty;
                myroles.Add(new MyRole { id = roleName });
            }
            return myroles.ToArray();
        }

        [AcceptVerbs("GET", "PUT")]
        [HttpPut]
        public async Task<IEnumerable<ManageUserModel>> UpdateUser(string id, ManageUserModel model)
        {
            var user = UserManager.FindById(id);
            var roles = await UserManager.GetRolesAsync(user.Id);
            user.UserName = model.Email;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserInfo.FirstName = model.FirstName;
            user.UserInfo.LastName = model.LastName;
            user.UserInfo.DateOfBirth = model.DateOfBirth;
            user.UserInfo.Street = model.Street;
            user.UserInfo.Suburb = model.Suburb;
            user.UserInfo.State = model.State;
            user.UserInfo.PostCode = model.PostCode;

            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if(roles.Count > 0)
                {
                    await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                }
                if(model.UserRoles.Length > 0)
                {
                    foreach (MyRole role in model.UserRoles)
                    {
                        await UserManager.AddToRoleAsync(user.Id, role.id);
                    }
                }
                return GetAllUsers();
            }
            else
            {
                throw new Exception("User updted unsuccessfully");
            }
        }

        [AcceptVerbs("GET", "DELETE")]
        [HttpDelete]
        public async Task<IEnumerable<ManageUserModel>> DeleteUser(string id)
        {
            var user = UserManager.FindById(id);
            var roles = await UserManager.GetRolesAsync(user.Id);
            if (roles.Count > 0)
            {
                await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
            }
            var userinfo = context.UserInfos.Find(user.UserInfo.Id);
            context.UserInfos.Remove(userinfo);
            var result =  await UserManager.DeleteAsync(user);
            context.SaveChanges();
            if (result.Succeeded)
                return GetAllUsers();
            else
                throw new Exception("User deleted unsuccessfully");
        }

        [HttpPost]
        public async Task<ManageUserModel> AddUser(ManageUserModel model)
        {
            
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserInfo = new UserInfo
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Street = model.Street,
                    Suburb = model.Suburb,
                    State = model.State,
                    PostCode = model.PostCode
                }
            };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.UserRoles.Length > 0)
                {
                    foreach (MyRole role in model.UserRoles)
                    {
                        if (!RoleManager.RoleExists(role.id))
                        {
                            RoleManager.Create(new IdentityRole(role.id));
                        }

                        await UserManager.AddToRoleAsync(user.Id, role.id);
                    }
                }
                model.Id = user.Id;
                return model;
            }
            else
            {
                throw new Exception("User is not created successfully");
            }
        }

        [HttpPost]
        public IHttpActionResult SendEmail(EmailInput data)
        {
            try
            {
                EmailHelper mailHelper = new EmailHelper();
                mailHelper.SendEmail(data.Sender, data.Receivers, data.EmailSubject, data.EmailBody);
            }
            catch(Exception ex) {
                throw new Exception("Error: Email sent unsuccessfully");
            }

            return Ok();
        }
    }
}
