using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Services;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using UserService.Identity;
using Microsoft.AspNetCore.Identity;
using UserService.Repository;

namespace UserService
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("UserDB"),sqlServerOption => sqlServerOption.EnableRetryOnFailure()));

            services.AddTransient<IUserRepository,UserRepository>();
            //services.AddMvc();

            services.AddScoped<IUserService, Services.UserService>();
            services.AddControllers();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserDBContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapSwagger();
                endpoints.MapControllers();
            });
        }
    }
}
