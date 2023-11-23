using Microsoft.EntityFrameworkCore;
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
        try
        {
        CreateHostBuilder(args).Build().Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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
}
