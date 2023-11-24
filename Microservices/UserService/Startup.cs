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
            options.UseSqlServer(Configuration.GetConnectionString("UserDB")));
            services.AddTransient<IUserRepository,UserRepository>();
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api ", Version = "v1" });
            });

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

            app.UseSwagger();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            Debug.WriteLine("Database migration NOT YET applied successfully.");
            context.Database.Migrate();
            Debug.WriteLine("Database migration applied successfully.");

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapSwagger();
                endpoints.MapControllers();
            });
        }
    }
}
