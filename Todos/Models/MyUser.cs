using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Todos.Models
{
    public class MyUser : IdentityUser
    {
        public string HomeTown { get; set; }
        public virtual ICollection<Todo> Todos { get; set; }
    }
}