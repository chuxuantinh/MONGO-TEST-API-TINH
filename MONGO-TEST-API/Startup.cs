using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MONGO_TEST_API.Interfaces;
using MONGO_TEST_API.Data;
using MONGO_TEST_API.Model;

namespace MONGO_TEST_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc();
            
            //services.AddMvc(options => options.EnableEndpointRouting = false);
            //services.AddControllers(options => options.EnableEndpointRouting = false);
            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddTransient<INoteRepository, NoteRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            //app.UseMvcWithDefaultRoute();
        }
    }
}
