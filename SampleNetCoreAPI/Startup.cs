using AutoMapper;
using BusinessAccess.DataContract;
using BusinessAccess.Repository;
using BusinessAccess.Services.Implement;
using BusinessAccess.Services.Interface;
using Common;
using DataAccess.ConfigurationManager;
using DataAccess.DBContext;
using DataAccess.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Security.Utility;
using System.IO;

namespace SampleNetCoreAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var connectionStringConfig = builder.Build();

            ///ADd Config From Database
            var config = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddEntityFrameworkConfig(options =>
                    options.UseSqlServer(connectionStringConfig.GetConnectionString("MySqlConnection"))
                 );

            Configuration = config.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add connection string to EF
            var sqlConnectionString = Configuration.GetConnectionString("MySqlConnection");
            services.AddDbContext<SampleNetCoreAPIContext>
                 (options => options.UseSqlServer(sqlConnectionString));
            #endregion

            #region ADd Configuration to dependency injection

            services.AddSingleton<IConfiguration>(Configuration);

            #endregion

            #region Add Authorization by using JWT
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Audience = Configuration["TokenAuthentication:siteUrl"];
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Constant.SecretSercurityKey)),

                    ValidateIssuer = true,
                    ValidIssuer = Configuration["TokenAuthentication:siteUrl"],

                    ValidateAudience = true,
                    ValidAudience = Configuration["TokenAuthentication:siteUrl"],

                    ValidateLifetime = true,
                };
            });
            #endregion

            #region Add Service to dependency injection
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IAuthozirationUtility, AuthozirationUtility>();
            #endregion

            #region Add Repository

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            #endregion

            #region Add AutoMapper
            ConfigAutoMapper(services);
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseAuthentication();
        }

        public void ConfigAutoMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogContract>();

            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
