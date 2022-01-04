using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag.AspNetCore;
using System;

namespace TestApi
{
    public class Startup
    {
        public static IConfiguration _config { get; private set; }
        public static ILogger<Startup> _logger;
        


        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _config = configuration;


            _logger = logger;
            _logger.LogInformation("Startup is Executing");
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Test API -" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                    document.Info.Description = "Super simple Test Api";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.SwaggerContact
                    {
                        Name = "MikTo Solutions",
                        Email = string.Empty,
                        Url = "www.miktosolutions.com"
                    };
                    document.Info.License = new NSwag.SwaggerLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };

                };

            });


            services.AddMvc(opt =>
            {

            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddMvcOptions(o => o.OutputFormatters.Add(
                new XmlDataContractSerializerOutputFormatter()
            ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AnyOrigin");

            app.UseDefaultFiles();
            app.UseStatusCodePages();
            app.UseStaticFiles(); // this lets the api know that we need access to the wwwroot folder



            app.UseAuthentication();
            //app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi3(c =>
            {

                SwaggerUi3Route localRoute = new SwaggerUi3Route("TestApi", "/swagger/v1/swagger.json");
                c.SwaggerRoutes.Add(localRoute);
          

            });
        }
    }
}
