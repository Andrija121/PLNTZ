using FriendshipService.DataContext;
using FriendshipService.Repository;
using FriendshipService.Services;
using Microsoft.EntityFrameworkCore;

namespace FriendshipService
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FriendshipDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FriendshipDB"), sqlServerOption => sqlServerOption.EnableRetryOnFailure()));

            services.AddTransient<IFriendshipRepository, FriendshipRepository>();
            services.AddScoped<IFriendshipService, Services.FriendshipService>();
            services.AddControllers();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FriendshipDBContext context)
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
