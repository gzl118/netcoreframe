using SANS.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SANS.Config;

namespace SANS.WebApp.Comm
{
    public class ServiceProviderHelper
    {
        public static ServiceCollection CreateServices()
        {
            return new ServiceCollection();
        }
        public static ServiceCollection GetDBServiceProvider()
        {
            var services = CreateServices();
            var conStr = SiteConfig.AppSetting("ConnectionStrings", "dataConnection");
            var conDataType = SiteConfig.AppSetting("ConnectionStrings", "dataBaseType");
            if (conDataType == "2")
            {
                services.AddDbContext<DbEntity.Models.MyEFContext>(options => options.UseSqlite(conStr),
                    ServiceLifetime.Singleton);
            }
            else if (conDataType == "3")
            {
                services.AddDbContext<DbEntity.Models.MyEFContext>(options => options.UseSqlServer(conStr, b => b.UseRowNumberForPaging()),
                    ServiceLifetime.Singleton);
            }
            else
            {
                services.AddDbContext<DbEntity.Models.MyEFContext>(options => options.UseMySQL(conStr),
                    ServiceLifetime.Scoped);
            }
            services.AddSingleton(new DAL.DapperContext(conStr, conDataType));
            return services;
        }

        #region 使用方式
        //        var services = ServiceProviderHelper.GetDBServiceProvider()
        //                            .AddScoped(typeof(IBusinessSatelliteMonitorDAL), typeof(BusinessSatelliteMonitorDAL))
        //                            .AddScoped(typeof(IBusinessSatelliteMonitorBLL), typeof(BusinessSatelliteMonitorBLL))
        //                            .BuildServiceProvider();
        //                        using (var scope = services.CreateScope())
        //                        {
        //                            if (Payload != null)
        //                            {
        //                                var blltest = scope.ServiceProvider.GetRequiredService<IBusinessSatelliteMonitorBLL>();
        //    var tmPack = ProtobufHelper.BytesToObject<TmPack>(Payload, 0, Payload.Length);
        //                                if (tmPack != null)
        //                                {
        //                                    List<BusinessSatelliteMonitor> lst = new List<BusinessSatelliteMonitor>();
        //                                    if (tmPack.eu != null && tmPack.eu.Count > 0)
        //                                    {
        //                                        tmPack.eu.ForEach(p =>
        //                                        {
        //                                            BusinessSatelliteMonitor monitorData = new BusinessSatelliteMonitor();
        //    monitorData.SatelliteID = tmPack.cid.ToString();
        //                                            monitorData.MonitorParamID = p.tmid.ToString();
        //                                            monitorData.ParamRaw = p.raw.ToString();
        //                                            monitorData.ParamVoltage = p.voltage.ToString();
        //                                            monitorData.ParamValue = p.eu.ToString();
        //                                            monitorData.ParamDesc = p.display;
        //                                            monitorData.ParamTime = DateTools.DoubleToTime(tmPack.time).ToString("yyyy-MM-dd HH:mm:ss");  //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    lst.Add(monitorData);
        //                                        });
        //                                        var isSuccess = blltest.UpdateMonitorData(lst);
        //                                        if (!isSuccess)
        //                                            Log4netHelper.Error(this, "保存遥测数据出错：" + JsonHelper.ObjectToJson(tmPack));
        //}
        //                                }
        //                            }
        //                        }
        #endregion
    }
}
