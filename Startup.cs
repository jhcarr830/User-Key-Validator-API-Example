using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExampleAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // load list of user names and keys from appsettings.json
            var apiSection = configuration.GetSection("ApiKeys");
            foreach (IConfigurationSection section in apiSection.GetChildren())
            {
                var userName = section.GetValue<string>("UserName");
                var userKey = section.GetValue<string>("UserKey");
                var permission = section.GetValue<string>("Permission");
                Globals.userList.Add(new User { UserName = userName, UserKey = userKey, Permission = permission });
            }
            //foreach (User user in Globals.userList)
            //{
            //    Console.WriteLine($"userName: {user.UserName}, userKey: {user.UserKey}");
            //}

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExampleAPI", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExampleAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<UserKeyValidator>();   // user key validation

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
