using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Todos.Startup))]
namespace Todos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
