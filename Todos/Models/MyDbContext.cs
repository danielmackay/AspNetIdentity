using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Todos.Models
{
    public class MyDbContext : IdentityDbContext<MyUser>
    {
        public MyDbContext():base("DefaultConnection")
        {
        }

        public DbSet<Todo> Todos { get; set; }
    }
}