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

            var server = Configuration["DbServer"];
            var name = Configuration["Dbname"];
            var password = Configuration["Password"];
            var port = Configuration["Dbport"];
            var user = Configuration["Dbuser"];

            var connString = String.Format("Server={0},{1};Database={2};User={3};Password={4};TrustServerCertificate=True", server, port, name, user, password);
            Console.WriteLine($"Connection String: {connString}");
            services.AddDbContext<UserDBContext>(options =>
            options.UseSqlServer(connString));

            services.AddDbContext<DbContext, UserDBContext>();
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

            //app.UseHttpsRedirection();
            //ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
            //ThreadPool.SetMinThreads(workerThreads * 2, completionPortThreads * 2);


            //app.Use(async (context, next) =>
            //{
            //context.Request.Scheme = "https";
            //await next();
            //});

            //context.Database.Migrate();

            app.UseRouting();

            app.UseAuthorization();
            Console.WriteLine("Database migration NOT YET applied successfully.");
            context.Database.Migrate();
            Console.WriteLine("Database migration applied successfully.");

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=ApplicationUsers}/{action=Index}/{id?}"
                ); ; });
        }
    }
}
