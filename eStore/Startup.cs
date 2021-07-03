using eStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eStore.Startup))]
namespace eStore
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }
        public void CreateDefaultRolesAndUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new
                RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new
                UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();
            IdentityRole role2 = new IdentityRole();
            if (!roleManager.RoleExists("Admin"))
            {
                role.Name = "Admin";
                roleManager.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.Email = "Admin@example.com";
                user.UserName = "Admin";
                user.UserType = "Admin";
                user.IsActive = true;
                user.EmailConfirmed = true;
                user.Balance = 100000;
                var Check = userManager.Create(user, "Admin@123");
                if (Check.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("مستهلك"))
            {
                role2.Name = "مستهلك";
                roleManager.Create(role2);
            }
        }
    }
}
