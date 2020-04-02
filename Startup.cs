using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace SportCompassRestApi
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
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SportCompassTestApi",
                    Description = "SportCompass Backend Test Api",
                    TermsOfService = new Uri("https://sportcompass.dk/"),
                    Contact = new OpenApiContact() { Name = "Kudzai Zishumba", Email = "zishumbak@gmail.com", Url = new Uri("https://sportcompass.dk/") }
                });
            });


            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
                c.AddPolicy("AllowMethod", options => options.AllowAnyMethod());
                c.AddPolicy("AllowHeader", options => options.AllowAnyHeader());
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvcCore().AddApiExplorer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseCookiePolicy();//place  befroe mvc

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "SPORTCOMPASS V1");
                c.OAuthClientId("api");
                c.OAuthAppName("api");
            });

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
            app.UseDeveloperExceptionPage();//show errors

            app.UseMvc();
            var app_name = env.ApplicationName;
            app.Run(async (context) =>
            {
                //await context.Response.WriteAsync(new Microsoft.AspNetCore.Html.HtmlString("<script>window.location='swagger';</script>").ToString());
                await context.Response.WriteAsync(new Microsoft.AspNetCore.Html.HtmlString($"<a href='../swagger'>Click here: to view {app_name} API</a>").ToString());
            });
        }
    }
}
