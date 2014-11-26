using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Blackbaud.OpenFx.ODataSample.Web.Startup))]
namespace Blackbaud.OpenFx.ODataSample.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
