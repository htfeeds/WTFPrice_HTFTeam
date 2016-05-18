using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WTFPrice_HTFTeam.Startup))]
namespace WTFPrice_HTFTeam
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
