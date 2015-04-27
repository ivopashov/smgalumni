using Microsoft.Owin;
using Owin;
using SmgAlumni.App;

[assembly: OwinStartup(typeof(Startup))]

namespace SmgAlumni.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
