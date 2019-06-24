using AutoMapper;
using BusinessAccess.DataContract;
using BusinessAccess.Repository;
using DataAccess.ConfigurationManager;
using DataAccess.DBContext;
using DataAccess.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
