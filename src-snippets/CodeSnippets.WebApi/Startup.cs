﻿using CodeSnippets.WebApi.Models;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using CodeSnippets.WebApi.Extensions;
using CodeSnippets.WebApi.Filters;
using CodeSnippets.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CodeSnippets.WebApi.Interfaces;
using CodeSnippets.WebApi.Services;

namespace CodeSnippets.WebApi
{
    /// <summary>
    /// 从 ASP.NET Core 2.2 迁移到3.0:
    /// https://docs.microsoft.com/zh-cn/aspnet/core/migration/22-to-30?view=aspnetcore-3.0&tabs=visual-studio
    /// </summary>
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
            #region obsolete
            //services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            //var jwtSettings = new JwtSettings();
            //Configuration.Bind("JwtSettings", jwtSettings);

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(o =>
            //{
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidIssuer = jwtSettings.Issuer,
            //        ValidAudience = jwtSettings.Audience,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            //    };

            //    #region 定制JWT，在基于Role授权不管用，可在使用Policy授权下使用
            //    //o.SecurityTokenValidators.Clear();
            //    //o.SecurityTokenValidators.Add(new MyTokenValidator());

            //    //o.Events = new JwtBearerEvents()
            //    //{
            //    //    OnMessageReceived = context =>
            //    //    {
            //    //        var token = context.Request.Headers["mytoken"];
            //    //        context.Token = token.FirstOrDefault();
            //    //        return Task.CompletedTask;
            //    //    }
            //    //}; 
            //    #endregion

            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("SuperAdminOnly", policy => policy.RequireClaim("SuperAdminOnly"));
            //}); 
            #endregion

            services.AddEntityFrameworkSqlite()
                .AddDbContext<SqliteDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("sqlite")));

            services.AddControllers();

            // 将身份验证服务添加到DI并配置Bearer为默认方案。
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", option =>
                {
                    option.Authority = "http://localhost:5000";
                    option.RequireHttpsMetadata = false;
                    option.Audience = "CodeSnippets.WebApi";
                });

            services.AddHttpClient();
            services.AddHealthChecks()
                .AddCheck<SqlServerHealthCheck>("sqlserver")
                .AddCheck<SqliteHealthCheck>("sqlite");

            services.AddScoped<IFoo, Foo>();

            // 全球化和本地化
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc(options => options.Filters.Add(typeof(GlobalExceptionFilter)));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();    // 将身份验证中间件添加到管道中，以便对主机的每次调用都将自动执行身份验证。
            app.UseAuthorization();    // 添加了授权中间件，以确保匿名客户端无法访问我们的API端点。

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
                //endpoints.MapHealthChecks("")
            });
            app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                // 自定义状态码
                ResultStatusCodes = new Dictionary<HealthStatus, int> { { HealthStatus.Healthy, 200 }, { HealthStatus.Unhealthy, 420 }, { HealthStatus.Degraded, 419 } },
                ResponseWriter = CustomResponseWriter
            });

            // 自定义中间件
            app.UseRequestIP();
        }

        private static Task CustomResponseWriter(HttpContext context, HealthReport healthReport)
        {
            context.Response.ContentType = "application/json";

            var result = JsonOperator.ToJson(new
            {
                status = healthReport.Status.ToString(),
                errors = healthReport.Entries.Select(e => new
                {
                    key = e.Key,
                    value = e.Value.Status.ToString()
                })
            });
            return context.Response.WriteAsync(result);

        }
    }
}
