using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Todos.Models
{
    public class MyDbInitializer : DropCreateDatabaseAlways<MyDbContext>
    {
        protected override void Seed(MyDbContext context)
        {
            var userManager = new UserManager<MyUser>(new UserStore<MyUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            const string name = "Admin";
            const string password = "123456";

            // Create Role Admin if it doesn't exist
            if (!roleManager.RoleExists(name))
                roleManager.Create(new IdentityRole(name));

            // Create User=Admin with password=123456
            var user = new MyUser {UserName = name};
            var adminResult = userManager.Create(user, password);

            if (adminResult.Succeeded)
                userManager.AddToRole(user.Id, name);

            base.Seed(context);
        }
    }
}