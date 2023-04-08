using Autofac;
using Autofac.Extensions.DependencyInjection;
using Feature.Api.Config;
using Feature.Api.Hub;
using Feature.Core.Config;
using Serilog;
using Serilog.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace AdivinaQuien.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        public GeneralConfig GeneralConfig { get; set; }


        public Startup(IConfiguration configuration)
        {
            //Fix rename ClaimType.NameIdentifier by nameid (http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier)
            //Source: https://github.com/dotnet/aspnetcore/issues/4660
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            Configuration = configuration;
            GeneralConfig = new GeneralConfig
            {
                CorsUrls = Configuration["GeneralConfig:CorsUrls"]?.Split(";")
            };
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var allowedOrigins = GeneralConfig.CorsUrls;

            services.AddLogging();
            services.AddResponseCaching();

            services.AddControllers();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builders =>
            {
                builders.WithOrigins(allowedOrigins.ToArray())
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));

            services.AddMemoryCache();

            //Logger
            var logger = new LoggerConfiguration()
                                     .Enrich.WithExceptionDetails()
                                     .ReadFrom.Configuration(Configuration)
                                     .CreateLogger();
            services.AddSingleton<Serilog.ILogger>(logger);

            var apiConfig = ApiConfig.Build(Configuration);
            
           

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdivinaQuien", Version = "v1" });
            //    c.CustomSchemaIds(c => c.FullName);
            //});
            //Se registran los Configs de Features
            services.AddSingleton(apiConfig);
            services.AddSignalR();

            // Create the container builder.
            var builder = new ContainerBuilder();

            //Se registran los Autofacs de Features
            builder.RegisterModule(new Feature.Api.AutofacModule() { Config = apiConfig });

            builder.Populate(services);

            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);

        }

        public void Configure(IApplicationBuilder app,
                            IWebHostEnvironment env,
                            ILoggerFactory loggerFactory,
                            IHostApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseResponseCaching();

            app.UseHttpsRedirection();

            //app.UseSwagger();

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdivinaQuien.Api");
            //});

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<CustomHub>("/signalr/adivinaquien");
            });
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
