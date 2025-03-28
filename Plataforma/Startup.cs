using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Plataforma.Startup))]
namespace Plataforma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
