using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using SampleNetCoreWebApiTemplate.WebApi.SwaggerHelp;
using SampleNetCoreWebApiTemplate.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using SampleNetCoreWebApiTemplate.WebApi.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SampleNetCoreWebApiTemplate.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="hostingEnvironment"></param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 根据不同环境对服务进行配置
            if (HostingEnvironment.IsDevelopment())
            {

            }
            else
            {

            }

            // 添加自定义配置文件,可选文件，修改后重新加载
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettingCustomer.json", true, true);

            var appsettingCustomer = builder.Build();

            // 获取配置文件中链接字符串，待验证会不会根据环境变量加载不同配置
            services.AddDbContext<ColorDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultDBConnectString")));

            //添加jwt验证：
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = Configuration["ValidAudience"],//Audience
                        ValidIssuer = Configuration["ValidIssuer"],//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))//拿到SecurityKey
                    };
                });

            // 依赖注册

            // 每次从容器请求时都重新创建
            //services.AddTransient<IEmailSender, AuthMessageSender>();

            services.AddTransient<IDbSession, DbSession>();

            // 每个Http请求创建一次的方式
            // services.AddScoped<IOperationScoped, Operation>();

            // 在第一次被请求 或者 ConfigureServices方法执行的时候被创建
            // services.AddSingleton<IOperationSingleton, Operation>();

            // 使用内存缓存，通过IMemoryCache 注入
            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // 使用时通过依赖注入IHttpClientFactory 使用
            services.AddHttpClient();

            // 需要在自定义组件中访问HttpContext时 需要使用以下方式注入，在构造函数中注入IHttpContextAccessor类型的对象
            // services.AddHttpContextAccessor();

            // 启用跨域
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllRequests",
                    builders =>
                    {
                        builders.AllowAnyOrigin();
                        builders.AllowAnyHeader();
                        builders.AllowAnyMethod();
                    });
            }
            );

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "SampleNetCoreWebApiTemplate",
                    Description = "A simple Template ASP.NET Core Web API"
                    //TermsOfService = "None"
                    // Contact = new Contact
                    // {
                    //     Name = "Shayne Boyer",
                    //     Email = string.Empty,
                    //     Url = "https://twitter.com/spboyer"
                    // },
                    // License = new License
                    // {
                    //     Name = "Use under LICX",
                    //     Url = "https://example.com/license"
                    // }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                //添加对控制器的标签(描述)
                c.DocumentFilter<SwaggerDocTag>();

                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}

            // 使用自定义的异常处理中间件
            app.UseCustomerExceptionMiddleWare();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors("AllowAllRequests");

            var IsEnableTraceLog = string.IsNullOrEmpty(Configuration["IsEnableTraceLog"]) ? false : bool.Parse(Configuration["IsEnableTraceLog"]);

            if (IsEnableTraceLog)
            {
                app.UseTraceLogMiddleWare();
            }

            var isEnableJWT = string.IsNullOrEmpty(Configuration["IsEnableJWT"]) ? false : bool.Parse(Configuration["IsEnableJWT"]);

            if (isEnableJWT)
            {
                app.UseAuthentication();
            }            

            app.UseMvc();
        }
    }
}
