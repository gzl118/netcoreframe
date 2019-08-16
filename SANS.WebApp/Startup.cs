using DInjectionProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SANS.Common.Cache;
using SANS.Config;
using SANS.WebApp.Comm;
using SANS.WebApp.Data;
using SANS.WebApp.Filters;
using SANS.WebApp.Models;
using SANS.WebApp.SignalR;
using System;

namespace SANS.WebApp
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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // ���ݿ������ַ���
            var conStr = Configuration.GetConnectionString("dataConnection");
            var conDataType = Configuration.GetConnectionString("dataBaseType");

            #region ��ܵ����ù�ϵ
            //ע��ef����
            if (conDataType == "2")
            {
                services.AddDbContext<DbEntity.Models.MyEFContext>(options => options.UseSqlite(conStr),
                    ServiceLifetime.Scoped);
            }
            else if (conDataType == "3")
            {
                services.AddDbContext<DbEntity.Models.MyEFContext>(options => options.UseSqlServer(conStr, b => b.UseRowNumberForPaging()),
                    ServiceLifetime.Scoped);
            }
            else
            {
                services.AddDbContext<DbEntity.Models.MyEFContext>(options => options.UseMySQL(conStr),
                    ServiceLifetime.Scoped);
            }
            services.AddMemoryCache();
            var pageTimeOut = Configuration.GetSection("CustomConfiguration").GetSection("PageTimeout").Value;
            int tempTimeOut;
            if (!int.TryParse(pageTimeOut, out tempTimeOut))
                tempTimeOut = 30;
            //���session�м��
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(tempTimeOut);//����session�Ĺ���ʱ��
            });
            //.net core 2.1ʱĬ�ϲ�ע��HttpContextAccessor����ע���ϵ,�����ٴ��ֶ�ע��
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //ע��gzipѹ���м��
            services.AddResponseCompression();
            //ע��Response �����м��
            services.AddResponseCaching();
            //��������ƥ��·������
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/{2}Manager/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}Manager/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
                //options.AreaViewLocationFormats.Add("/Areas/{2}Layer/Views/{1}/{0}.cshtml");
            });
            //mvc��� 
            services.AddMvc(o =>
            {
                //ע��ȫ���쳣������
                o.Filters.Add<HttpGlobalExceptionFilter>();
                //���û�����Ϣ
                o.CacheProfiles.Add("default", new Microsoft.AspNetCore.Mvc.CacheProfile
                {
                    Duration = 60 * 10   // 10����
                });
                o.CacheProfiles.Add("Hourly", new Microsoft.AspNetCore.Mvc.CacheProfile
                {
                    Duration = 60 * 60  // 1 hour
                });
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });

            #endregion
            #region �Զ�������ù�ϵ
            //ע��ȫ������ע���ṩ����
            services.AddScoped(typeof(WholeInjection));
            services.AddSingleton<ICacheContext, CacheContext>();
            services.AddScoped(typeof(UserAccount));
            services.AddSingleton(new DAL.DapperContext(conStr, conDataType));
            services.AddSingleton<MsgHandling>();


            var MQType = Configuration.GetConnectionString("MQType");
            if (MQType.Equals("1"))
            {
            }
            else if (MQType.Equals("2"))
            {
                //kafka
                services.AddSingleton<IMsgSend, KafkaMsgSend>();
                services.AddSingleton<IMsgReceive, KafkaMsgReceiveConfluent>();
            }
            services.ResolveAllTypes(new string[] { "SANS.DAL", "SANS.BLL" });
            //��ʼ��Dto��ʵ��ӳ���ϵ
            BLL.MyMapper.Initialize();
            //ע�������ļ���
            services.AddOptions().Configure<CustomConfiguration>(Configuration.GetSection("CustomConfiguration"));
            SiteConfig.SetAppSetting(Configuration);
            #endregion

            #region websocket

            #region ����websocket
            //services.AddSingleton<ICustomWebSocketFactory, CustomWebSocketFactory>();
            //services.AddSingleton<ICustomWebSocketMessageHandler, CustomWebSocketMessageHandler>();

            #endregion

            #region SignalR
            //services.AddSignalR(options =>
            //{
            //    // Faster pings for testing
            //    options.KeepAliveInterval = TimeSpan.FromSeconds(5);//���������ʱ�䣬��λ �룬������΢����һ���
            //}).AddJsonProtocol(options =>
            //{
            //    //options.PayloadSerializerSettings.Converters.Add(JsonConver);
            //    //the next settings are important in order to serialize and deserialize date times as is and not convert time zones
            //    //options.PayloadSerializerSettings.Converters.Add(new IsoDateTimeConverter());
            //    //options.PayloadSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
            //    //options.PayloadSerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
            //    options.PayloadSerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            //});
            #endregion

            #endregion

            #region Swagger
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "KN API", Version = "v1" });
            //    c.IncludeXmlComments(Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location),
            //        "SANS.WebApp.xml")); // ע�⣺�˴��滻�������ɵ�XML documentation���ļ�����
            //    c.IncludeXmlComments(Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location),
            //        "SANS.DbEntity.xml"));
            //    c.DescribeAllEnumsAsStrings();
            //    c.OperationFilter<AddAuthTokenHeaderParameter>();
            //});
            #endregion

            #region Cors
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("SANS�������", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            //});
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            IMsgReceive msgHandler,
            IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            InitConfig.Init(app.ApplicationServices);

            //��־��¼�м��
            //repository = LogManager.CreateRepository("NETCoreRepository");
            //XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            app.UseStatusCodePages();
            app.UseStaticFiles();

            #region websocket

            #region ����websocket
            //var webSocketOptions = new WebSocketOptions()
            //{
            //    KeepAliveInterval = TimeSpan.FromSeconds(120),
            //    ReceiveBufferSize = 4 * 1024
            //};
            //app.UseWebSockets(webSocketOptions);
            //app.UseCustomWebSocketManager();
            #endregion

            #region SignalR
            //��ʼ�����ݿ�
            // ʹ��SignalR �����MessageHub�����Ϣ������
            //app.UseSignalR(r =>
            //{
            //    r.MapHub<MessageHub>("/messagehub", options => options.WebSockets.SubProtocolSelector = requestedProtocols =>
            //    {
            //        return requestedProtocols.Count > 0 ? requestedProtocols[0] : null;
            //    });
            //});
            #endregion

            #endregion

            app.UseSession();
            //gzipѹ���м��
            app.UseResponseCompression();
            //Response �����м��
            app.UseResponseCaching();

            #region Cors
            //app.UseCors("SANS�������");

            #endregion

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=Login}/");
            });
            // Ӧ������ʱ��ʼ������Ϣ
            applicationLifetime.ApplicationStarted.Register(msgHandler.receiveMsg);
            applicationLifetime.ApplicationStopping.Register(msgHandler.stopMsg);
            // Ӧ���˳�ʱ,�ͷ���Դ

            #region Swagger
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KNAPI");
            //});
            #endregion
        }
    }
}
