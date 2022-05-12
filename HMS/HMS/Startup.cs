using HMS.Data;
using HMS.Security;
using HMS.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.OnDeleteCookie = context =>
                {
                    context.Context.Request.HttpContext.Response.Redirect("/Home/logout");
                };
            });
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = "SessionCookie";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "AuthenticationCookie";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = new PathString("/Home/login");
                options.LogoutPath = new PathString("/Home/logout");
                options.SlidingExpiration = true;
                options.AccessDeniedPath = new PathString("/Shared/AccessDenied");
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "ConfigureApplicationCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Cookie.Name = "ApplicationCookie";
                options.LoginPath = new PathString("/Home/Index");
                options.LogoutPath = new PathString("/Home/logout");
            });
            services.AddDbContext<HMSDBContext>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSingleton<ICustomDataProtector, CustomDataProtector>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDataProtection();
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
