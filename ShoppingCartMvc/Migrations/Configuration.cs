namespace ShoppingCartMvc.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppingCartMvc.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShoppingCartMvc.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if(!roleManager.RoleExists("Admin"))
            roleManager.Create(new IdentityRole("Admin"));

            if (!roleManager.RoleExists("Manager"))
                roleManager.Create(new IdentityRole("Manager"));

            if (!roleManager.RoleExists("Editor"))
                roleManager.Create(new IdentityRole("Editor"));
          
            var user = new ApplicationUser {
                Email = "admin@email.com",
                UserName = "admin",
            };
            string myPass = "123456";
         var result =    userManager.Create(user, myPass);
            if (result.Succeeded)
            {
                userManager.AddToRole(user.Id, "Admin");
            }


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


        }
    }
}
