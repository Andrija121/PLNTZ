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
            var rabbitMQConfig = Configuration.GetSection("RabbitMQ").Get<RabbitMQConfiguration>();

            var connectionFactory = new ConnectionFactory
            {
                HostName = rabbitMQConfig.Hostname,
                UserName = rabbitMQConfig.UserName,
                Password = rabbitMQConfig.Password,
                Port = rabbitMQConfig.Port
            };

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
