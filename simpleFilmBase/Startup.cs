using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(simpleFilmBase.Startup))]
namespace simpleFilmBase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
