using Autofac;
using Feature.Api.Business;
using Feature.Api.Config;
using Feature.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RepoDb.DbHelpers;
using RepoDb.DbSettings;
using RepoDb.StatementBuilders;
using RepoDb;
using System.Reflection;

namespace Feature.Api
{
    public class AutofacModule: Autofac.Module
    {
        public ApiConfig Config{ get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            var dbConnection = new System.Data.SqlClient.SqlConnection(Config.ConnectionString);
            GlobalConfiguration
                .Setup()
                .UseSqlServer();
            var dbSetting = new SqlServerDbSetting();

            DbSettingMapper
                .Add<System.Data.SqlClient.SqlConnection>(dbSetting, true);
            DbHelperMapper
                .Add<System.Data.SqlClient.SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper
                .Add<System.Data.SqlClient.SqlConnection>(new SqlServerStatementBuilder(dbSetting), true);

            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetService<System.Net.Http.IHttpClientFactory>();

            builder.RegisterInstance(httpClientFactory);

            builder.RegisterAssemblyTypes(typeof(AdivinaQuienRepository).Assembly)
               .Where(t => t.Name.EndsWith("Repository"))
               .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(AdivinaQuienBusiness).Assembly)
               .Where(t => t.Name.EndsWith("Business"))
               .InstancePerLifetimeScope();

            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .WithProperty("DbConnection", dbConnection)
                .InstancePerDependency();

        }
    }
}
