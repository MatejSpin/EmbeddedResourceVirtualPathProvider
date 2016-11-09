using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestWebProject.MVC.Startup))]
namespace TestWebProject.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
