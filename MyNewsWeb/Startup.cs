using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyNewsWeb.Startup))]
namespace MyNewsWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
