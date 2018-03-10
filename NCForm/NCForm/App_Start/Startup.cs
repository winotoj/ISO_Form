using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NCForm.Startup))]

namespace NCForm
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
