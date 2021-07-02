using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using eStore.Models;
using eStore;

namespace eStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        [HttpGet]
        public ActionResult Index(string search, int? pageNo, string role)
        {
            var usersList = context.Users.Where(x => x.UserType != "Admin").Select(x => new UsersViewModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                UserType = x.UserType,
                IsActive = x.IsActive
            });
            ViewBag.role = new SelectList(context.Roles.Where(x => x.Name != "Admin").ToList(), "Name", "Name");
            return View(usersList.OrderByDescending(x => x.Id).ToPagedList(pageNo ?? 1, 10));
        }

        //
        // GET: /Users/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewBag.AccountType = new SelectList(await context.Roles.Where(x => x.Name != "Admin").ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity.GetUserId();
                    var user = new ApplicationUser
                    {
                        UserName = userViewModel.Username,
                        Email = userViewModel.Email,
                        UserType = userViewModel.UserType,
                        IsActive = true,
                        EmailConfirmed = true
                    };
                    var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                    //Add User to the selected Roles 
                    if (adminresult.Succeeded)
                    {
                        if (userViewModel.UserType != null)
                        {
                            var result = await UserManager.AddToRolesAsync(user.Id, userViewModel.UserType);
                            if (!result.Succeeded)
                            {
                                TempData["Message"] = "حدث خطأ غير متوقع عند إضافة الصلاحية لهذا المستخدم";
                                TempData["Status"] = 2;
                                ModelState.AddModelError("", result.Errors.First());
                                ViewBag.AccountType = new SelectList(await context.Roles.Where(x => x.Name != "Admin").ToListAsync(), "Name", "Name");
                                return View(userViewModel);
                            }
                        }
                    }
                    else
                    {
                        TempData["Message"] = "حدث خطأ غير متوقع";
                        TempData["Status"] = 2;
                        ModelState.AddModelError("", adminresult.Errors.First());
                        ViewBag.AccountType = new SelectList(await context.Roles.Where(x => x.Name != "Admin").ToListAsync(), "Name", "Name");
                        return View(userViewModel);
                    }
                    TempData["Message"] = "تمت إضافة المستخدم بنجاح";
                    TempData["Status"] = 1;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "حدث خطأ غير متوقع";
                    TempData["Status"] = 2;
                }
            }
            ViewBag.AccountType = new SelectList(await context.Roles.Where(x => x.Name != "Admin").ToListAsync(), "Name", "Name");
            return View(userViewModel);
        }

        //
        // GET: /Users/Edit/1
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRole = await UserManager.GetRolesAsync(user.Id);
            IEnumerable<SelectListItem> rolesList = context.Roles.Where(x => x.Name != "Admin")
                .ToList().Select(x => new SelectListItem()
                {
                    Selected = userRole.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });
            ViewBag.SelectedRole = rolesList;
            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserManager.FindByIdAsync(editUser.Id);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }

                    user.UserName = editUser.UserName;
                    user.Email = editUser.Email;
                    user.UserType = editUser.SelectedRole;

                    var userRoles = await UserManager.GetRolesAsync(user.Id);

                    if (editUser.SelectedRole != userRoles[0])
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, editUser.SelectedRole);
                        if (!result.Succeeded)
                        {
                            TempData["Message"] = "حدث خطأ غير متوقع";
                            TempData["Status"] = 2;
                            ModelState.AddModelError("", result.Errors.First());
                            return View(editUser);
                        }
                    }
                    else
                    {
                        var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                        await UserManager.UpdateAsync(user);
                        var ctx = userStore.Context;
                        await ctx.SaveChangesAsync();
                    }

                    if (userRoles[0] != editUser.SelectedRole)
                    {
                        await UserManager.RemoveFromRoleAsync(user.Id, userRoles[0]);
                    }
                    TempData["Message"] = "تم تعديل بيانات المستخدم بنجاح";
                    TempData["Status"] = 1;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "حدث خطأ غير متوقع";
                    TempData["Status"] = 2;
                }
            }
            ModelState.AddModelError("", "حدث خطأ ما");
            var userRole = await UserManager.GetRolesAsync(editUser.Id);
            IEnumerable<SelectListItem> rolesList = context.Roles.Where(x => x.Name != "Admin")
                .ToList().Select(x => new SelectListItem()
                {
                    Selected = userRole.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });
            ViewBag.SelectedRole = rolesList;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            try
            {
                if (user.IsActive)
                {
                    user.IsActive = false;
                    TempData["Message"] = "تم إيقاف التفعيل بنجاح";
                }
                else
                {
                    user.IsActive = true;
                    TempData["Message"] = "تم التفعيل بنجاح";
                }
                TempData["Status"] = 1;
                await UserManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Status"] = 2;
                TempData["Message"] = "حدث خطأ";
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
