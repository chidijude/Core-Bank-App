using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpressCoreBank.Startup))]
namespace ExpressCoreBank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
