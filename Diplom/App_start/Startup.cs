using System;
using System.Threading;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Castle.Windsor;
using Diplom.IoC;
using Microsoft.Owin.BuilderProperties;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using OAuth;

[assembly: OwinStartup(typeof(Diplom.Startup))]

namespace Diplom
{
    public class Startup
    {
        public static IWindsorContainer Container;

        public void Configuration(IAppBuilder app)
        {
            IWindsorContainer container;
            UseWindsor(app, out container);

            Container = container;
            var config = new HttpConfiguration();
            WebApiConfig.Build(config);
            ConfigureOAuth(app, Container);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app, IWindsorContainer container)
        {
            var tokenExpireTime = "10:00:00";
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.Parse(tokenExpireTime),
                Provider = new AuthorizationServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private IAppBuilder UseWindsor(IAppBuilder app, out IWindsorContainer container)
        {
            container = new WindsorContainer().Install(new DependencyInstaller());

            return RegisterForDisposing(app, container);
        }

        private IAppBuilder RegisterForDisposing(IAppBuilder app, IWindsorContainer container)
        {
            var properties = new AppProperties(app.Properties);
            var token = properties.OnAppDisposing;

            if (token != CancellationToken.None)
            {
                token.Register(
                    () =>
                    {
                        if (container == null) return;

                        container.Dispose();
                    });
            }

            return app;
        }
    }
}
