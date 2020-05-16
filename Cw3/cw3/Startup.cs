using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using cw3.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using cw3.Middlewares;
using Microsoft.AspNetCore.Authentication;
using cw3.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using cw3.Models;
using Microsoft.EntityFrameworkCore;

namespace Wyklad3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IStudentsDbService, EfStudentsDbService>();
            services.AddDbContext<StudentDbContext>(options =>
            {
                options.UseSqlServer("Data Source=db-mssql;Initial Catalog=s19171;Integrated Security=True");
            });

            //services.AddScoped<IStudentsDbService, SqlServerDbService>();
            services.AddTransient<IDbService, SqlServerDbService>();
            //services.AddAuthentication("AuthenticationBasic").AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("AuthenticationBasic",null);
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidIssuer = "Gakko",
            //        ValidAudience = "Students",
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
            //    };
            //}); ;
            services.AddControllers();
                //.AddXmlSerializerFormatters();
           
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbService dbService)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            //app.UseMiddleware<LoggingMiddleware>();

            //app.Use(async (context, c) =>
            //{
            //    if (!context.Request.Headers.ContainsKey("Index"))
            //    {
            //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //        await context.Response.WriteAsync("Nie podano indeksu naglowku");
            //        return;
            //    }

            //    var index = context.Request.Headers["Index"].ToString();
            //    var bodyStream = string.Empty;

            //    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            //    {
            //        bodyStream = await reader.ReadToEndAsync();
            //    }

            //    if (!dbService.CheckIndexNumber(index))
            //    {
            //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //        await context.Response.WriteAsync("Podany student nie istnieje");
            //        return;
            //        //poinformować o nieistejacym studencie
            //    }
                //context.Response.Headers.Add("Secret", "1234");
               // await c.Invoke();
            //});
            //app.UseMiddleware<CustomMiddleware>();

            app.UseAuthorization();
            //app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
