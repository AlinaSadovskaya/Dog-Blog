using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using test.Domain.Core;
using test.Services.BusinessLogic;
using test.Infrastructure.Data;
using test.Domain.Interfaces;

namespace test
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
            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddTransient<IPasswordValidator<User>,
            CustomPasswordValidator>(serv => new CustomPasswordValidator(6));
            services.AddTransient<IUserValidator<User>, CustomUserValidator>();
            
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;    // уникальный email
                opts.User.AllowedUserNameCharacters = "._1234567890@abcdefghijklmnopqrstuvwxyz";

            })
                 .AddEntityFrameworkStores<BlogContext>()
                .AddDefaultTokenProviders();
            services.AddDbContext<BlogContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("test")));
            services.AddScoped<PostRepository>();
            services.AddScoped<Repository<Dog, int>>();
            services.AddScoped<Repository<Topic, int>>();
            services.AddScoped<Repository<User, string>>();
            services.AddScoped<CommentRepository>();
            services.AddTransient<ISender, EmailService>();
            services.AddSingleton<ImageService>();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                  //  options.ClientId = "822625351685-35p3lib3eh36dhnrtt2so3n59l5saf08.apps.googleusercontent.com";
                  //  options.ClientSecret = "fD5jE4DXNTc6emdnsHIsbBxc";
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/ErrorPro");
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/Error/Index", "?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "MethodWithId",
                   pattern: "{controller}/{action}/{id}");
                endpoints.MapControllerRoute(
                   name: "MethodWithNullId",
                   pattern: "Posts/Index/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                endpoints.MapHub<HabrDotNetHub>("/chathub");
            });
            
        }

    }
}
