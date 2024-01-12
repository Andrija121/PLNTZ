using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Services;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using UserService.Identity;
using Microsoft.AspNetCore.Identity;
using UserService.Repository;
using RabbitMQ;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace UserService
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
                UserName= rabbitMQConfig.UserName,
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
