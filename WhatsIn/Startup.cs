using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WhatsIn.Startup))]
namespace WhatsIn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
