using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using BizRules.CompanyBizRules;
using BizRules.InspectionBizRules;
using BizRules.UsersBizRules;
using DataAccess;
using DataAccess.DataAccess.CompanyRepository;
using DataAccess.DataAccess.EvaluationRepository;
using DataAccess.DataAccess.InspectionRepository;
using DataAccess.DataAccess.TokenRepository;
using DataAccess.DataAccess.UserRepository;
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
            using (var context = new ISControlDbContext())
            {
                if (context.Categories.SelectMany(x => x.Requirements).Count() != 129)
                {
                    var scriptCommand = File.ReadAllText($"{HttpRuntime.BinDirectory}/script.sql");
                    context.Database.ExecuteSqlCommand(scriptCommand);
                }
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IHttpController).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
            services.AddSingleton<IOAuthAuthorizationServerProvider, AuthorizationServerProvider>();
            services.AddSingleton<IAuthenticationTokenProvider, RefreshTokenProvider>();
            services.AddSingleton<ITokenRepository, TokenRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IUsersBizRules, UsersBizRules>();
            services.AddSingleton<ICompanyBizRules, CompanyBizRules>();
            services.AddSingleton<ICompanyRepository, CompanyRepository>();
            services.AddSingleton<IInspectionBizRules, InspectionBizRules>();
            services.AddSingleton<IInspectionRepository, InspectionRepository>();
            services.AddSingleton<IEvaluationRepository, EvaluationRepository>();
        }

        private void ConfigureOAuth(IAppBuilder app, DefaultDependencyResolver resolver)
        {
            var tokenExpireTime = ConfigurationManager.AppSettings["Security.AccessTokenLifetime"];
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
        protected IServiceProvider ServiceProvider;

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ServiceProvider.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new DefaultDependencyResolver(ServiceProvider.CreateScope().ServiceProvider);
        }

        public void Dispose()
        {
            
        }
    }
}
