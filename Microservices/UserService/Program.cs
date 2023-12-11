using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UserService;
using UserService.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddDbContext<UserDBContext>(opitons =>opitons.UseSqlServer())

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                // Wait for the database to be ready
                WaitForDb(services);

                // Continue with other initialization logic
                var context = services.GetRequiredService<DbContext>();
                Debug.WriteLine("Database migration NOT YET applied successfully.");
                context.Database.Migrate();
                Debug.WriteLine("Database migration applied successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while waiting for the database: " + ex.Message);
            }
        }

        host.Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.AddConsole();
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

    private static void WaitForDb(IServiceProvider services)
    {
        var retryCount = 0;
        var maxRetries = 30; // Adjust as needed

        while (retryCount < maxRetries)
        {
            try
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("UserDB");

                using var client = new SqlConnection(connectionString);
                client.Open();
                Debug.WriteLine("Database is available.");
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to connect to the database. Retrying in 1 second. Exception: {ex.Message}");
                Thread.Sleep(1000); // Wait for 1 second before retrying
                retryCount++;
            }
        }

        throw new InvalidOperationException("Failed to connect to the database after several attempts.");
    }

}
