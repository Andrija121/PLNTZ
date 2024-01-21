using FriendshipService.DataContext;
using FriendshipService.Repository;
using FriendshipService.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ;

namespace FriendshipService
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;


        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<FriendshipDBContext>(options =>
            options.UseSqlServer("Server=tcp:friendship-db,1433;User ID=sa;Database=friendship_service;User=sa;Password=test@123;TrustServerCertificate=true", sqlServerOption => sqlServerOption.EnableRetryOnFailure(10, TimeSpan.FromSeconds(15), null)));
            var rabbitMQConfig = Configuration.GetSection("RabbitMQ").Get<RabbitMQConfiguration>();

            var connectionFactory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
            Console.WriteLine(connectionFactory);

            var connection = connectionFactory.CreateConnection();

            // Create a channel for the producer
            var producerChannel = connection.CreateModel();

            // Create a channel for the consumer
            var consumerChannel = connection.CreateModel();

            services.AddSingleton(new RabbitMQProducer(producerChannel));
            services.AddSingleton(new RabbitMQConsumer(consumerChannel));
            services.AddSingleton<RabbitMQSerivce>(sp =>
            {
                var producer = sp.GetRequiredService<RabbitMQProducer>();
                var consumer = sp.GetRequiredService<RabbitMQConsumer>();

                return new RabbitMQSerivce(rabbitMQConfig, producer, consumer);
            });

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
