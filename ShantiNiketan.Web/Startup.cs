using Microsoft.Owin;
using Owin;
using ShantiNiketan.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace ShantiNiketan.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
