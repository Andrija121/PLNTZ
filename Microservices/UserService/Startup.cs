using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Services;

namespace UserService
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<UserDBContext>();
            services.AddDbContext<UserDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<DbContext, UserDBContext>();
            services.AddScoped<IUserService, Services.UserService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                ); ; });
        }
    }
}
