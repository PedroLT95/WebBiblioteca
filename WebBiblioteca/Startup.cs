using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBiblioteca.Startup))]
namespace WebBiblioteca
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
