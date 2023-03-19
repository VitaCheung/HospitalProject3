using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HospitalProject2.Startup))]
namespace HospitalProject2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
