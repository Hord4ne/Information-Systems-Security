using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RolesAndStuff.Models;
using RolesAndStuff.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RolesAndStuff.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Employee")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles()
        {
            {
                var usersWithRoles = (from user in _context.Users
                                      select new
                                      {
                                          UserId = user.Id,
                                          Username = user.UserName,
                                          Email = user.Email,
                                          RoleNames = (from userRole in user.Roles
                                                       join role in _context.Roles on userRole.RoleId
                                                       equals role.Id
                                                       select role.Name).ToList()
                                      }).ToList().Select(p => new UsersRoleViewModel()

                                      {
                                          UserId = p.UserId,
                                          Username = p.Username,
                                          Email = p.Email,
                                          Role = string.Join(",", p.RoleNames)
                                      }).ToList();


                return View(usersWithRoles);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ManageRoles( UsersRoleViewModel valu2e)
        {
            var roleNames = (from userRole in _context.Roles
                             select userRole.Name).ToList();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            if (valu2e.Role != null)
                UserManager.RemoveFromRole(valu2e.UserId, valu2e.Role);
            var index = roleNames.FindIndex(x => x == valu2e.Role);
            var result1 = UserManager.AddToRole(valu2e.UserId, roleNames[(index+1)%roleNames.Count]);
            return ManageRoles();
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}