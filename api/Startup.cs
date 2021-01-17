using System;
using System.Text;
using System.Threading.Tasks;
using api.Middleware;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var connection = Configuration["ConnectionStrings:SQlite"];

            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlite(connection);
            });

            // Configure Entity Framework Initializer for seeding
            services.AddTransient<IDefaultDbContextInitializer, ApiDbContextInitializer>();


            ConfigureAuthentication(services);
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
               .AddEntityFrameworkStores<ApiDbContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;

                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["jwt:issuer"],
                    ValidAudience = Configuration["jwt:issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"]))
                };
            });

            services.Configure<JwtOptions>(Configuration.GetSection("jwt"));

            services.AddSingleton<IPasswordHasher<User>, Md5PasswordHasher<User>>();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roles = { "Administrator", "Seller" };

            foreach (var roleName in roles)
            {
               var roleExist = await roleManager.RoleExistsAsync(roleName);

               if (!roleExist)
                  await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseSpaFallback(new SpaFallbackOptions()
            {
               ApiPathPrefix = "/api",
               RewritePath = "/"
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
               app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
               endpoints.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(serviceProvider).Wait();
        }
    }
}
