using dotnetapp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetapp
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ACServiceAPI", Version = "v1" });
            });

            services.AddDbContext<ACServiceDbContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
           services.AddCors();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
                (opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["Jwt:Isuuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                        
                    };
            });
            // services.AddCors(options =>
            // {
            // options.AddPolicy("AllowAny",builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithOrigin("https://8081-bcbbdfcfbcfacbdcbaeaceadebfeffeb.project.examly.io"));
            // // options.AddPolicy("FrontEndClient",builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigin("https://8081-bcbbdfcfbcfacbdcbaeaceadebfeffeb.project.examly.io"));

            // });
            services.AddCors(options =>
            {
            options.AddPolicy(name: "AllowAny",builder => 
            {
            builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();   });
            });

                
            // services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseCors(options =>
            // options.WithOrigins("https://8081-bcbbdfcfbcfacbdcbaeaceadebfeffeb.project.examly.io")
            // .AllowAnyMethod()
            // .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ACServiceAPI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseCors("AllowAny");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
