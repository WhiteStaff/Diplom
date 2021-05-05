using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Castle.Windsor;

[assembly: OwinStartup(typeof(Diplom.Startup))]

namespace Diplom
{
    public class Startup
    {
        public static IWindsorContainer Container;

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Build(config);
            app.UseWebApi(config);
        }
    }
}
