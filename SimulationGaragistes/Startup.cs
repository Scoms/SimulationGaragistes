using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimulationGaragistes.Startup))]
namespace SimulationGaragistes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
