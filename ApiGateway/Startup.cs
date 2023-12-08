using Microsoft.Extensions.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiGateway
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            string authority = $"https://{Configuration["Auth0:Domain"]}/";
            var key = Encoding.ASCII.GetBytes("SecureKeyRequiredForValidationAdmin");
            string audience = Configuration["Auth0:Audience"]?.ToString() ?? "defaultAudience";
            //var authenticationProviderKey = "TestKey";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("ProviderKey",options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Authority = authority;
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                //options.TokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidateAudience = true,
                //};
            });
            services.AddOcelot(Configuration);

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseOcelot();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
