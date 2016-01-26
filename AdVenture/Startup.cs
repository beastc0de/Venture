using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdVenture.Startup))]
namespace AdVenture
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
