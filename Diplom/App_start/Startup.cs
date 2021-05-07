using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using OAuth;

[assembly: OwinStartup(typeof(Diplom.Startup))]

namespace Diplom
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var services = new ServiceCollection();
            ConfigureServices(services);
            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());
            config.DependencyResolver = resolver;
            WebApiConfig.Build(config);
            ConfigureOAuth(app, resolver);
            app.UseWebApi(config);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IHttpController).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
            services.AddSingleton<IOAuthAuthorizationServerProvider, AuthorizationServerProvider>();
            services.AddSingleton<IAuthenticationTokenProvider, RefreshTokenProvider>();
        }

        private void ConfigureOAuth(IAppBuilder app, DefaultDependencyResolver resolver)
        {
            var tokenExpireTime = "10:00:00";
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.Parse(tokenExpireTime),
                Provider = (IOAuthAuthorizationServerProvider)resolver.GetService(typeof(IOAuthAuthorizationServerProvider)),
                RefreshTokenProvider = (IAuthenticationTokenProvider)resolver.GetService(typeof(IAuthenticationTokenProvider))
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }

    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services,
            IEnumerable<Type> controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }

    public class DefaultDependencyResolver : IDependencyResolver
    {
        protected IServiceProvider serviceProvider;

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.serviceProvider.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new DefaultDependencyResolver(this.serviceProvider.CreateScope().ServiceProvider);
        }

        public void Dispose()
        {
            
        }
    }
}
