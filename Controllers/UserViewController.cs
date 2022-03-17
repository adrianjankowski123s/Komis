using Komis.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Komis.Controllers
{
    public class UserViewController : Controller
    {
        private KomisContext context = new KomisContext();


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserViewController()
        {

        }

        public UserViewController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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



        [Authorize(Roles = "Admin")]
        // GET: UserView
        public ActionResult Index()
        {
            var role = (from r in context.AspNetRoles where r.Name.Contains("Uzytkownik") select r).FirstOrDefault();
            var users = context.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Id).Contains(role.Id)).ToList();

            var userVM = users.Select(user => new UserViewModel
            {
                Email = user.Email,
                Roles = "Uzytkownik"
            }).ToList();


            var role2 = (from r in context.AspNetRoles where r.Name.Contains("Admin") select r).FirstOrDefault();
            var admins = context.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Id).Contains(role2.Id)).ToList();

            var adminVM = admins.Select(user => new UserViewModel
            {
                Email = user.Email,
                Roles = "Admin"
            }).ToList();


            var model = new GroupedUserViewModel { Users = userVM, Admins = adminVM };
            return View(model);

        }

        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRole(string Email)
        {


            var currentUser = UserManager.FindByEmail(Email);
            string id = UserManager.FindByEmail(Email).Id;
            var role = UserManager.GetRoles(id);
            string roleName = role[0];
            if (roleName == "Admin")
            {
                UserManager.RemoveFromRole(id, "Admin");
                UserManager.AddToRole(id, "Uzytkownik");
                
            }
            else
            {
                UserManager.RemoveFromRole(id, "Uzytkownik");
                UserManager.AddToRole(id, "Admin");

            }


            return RedirectToAction("Index");
        }

        // GET: /Users/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string Email)
        {

            string id = UserManager.FindByEmail(Email).Id;
            AspNetUsers currentUser = context.AspNetUsers.Find(id);

            var model = new UserViewModel { currentUser = currentUser };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
           
            return View(model);
        }

        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string Email)
        {
            string id = UserManager.FindByEmail(Email).Id;

            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                var logins = user.Logins;
                var rolesForUser = await UserManager.GetRolesAsync(id);

                using (var transaction = context.Database.BeginTransaction())
                {
                    foreach (var login in logins.ToList())
                    {
                        await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                        }
                    }

                    await UserManager.DeleteAsync(user);
                    transaction.Commit();
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}