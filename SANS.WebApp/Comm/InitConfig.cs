using Microsoft.Extensions.DependencyInjection;
using SANS.BLL.Interface;
using SANS.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Comm
{
    public class InitConfig
    {
        /// <summary>
        /// 初始化配置信息
        /// </summary>
        /// <param name="service"></param>
        public static void Init(IServiceProvider service)
        {
            try
            {
                using (var serviceScope = service.CreateScope())
                {
                    var sysDataDictionaryBLL = serviceScope.ServiceProvider.GetService<ISysDataDictionaryBLL>();

                    #region Kafka配置
                    var messageModel = sysDataDictionaryBLL.GetDictListByTypeCode("Kafka");
                    if (messageModel != null && messageModel.Data != null && messageModel.Data.Count > 0)
                    {
                        var s = (List<SysDataDictionary>)messageModel.Data;
                        var scode = s.FirstOrDefault(a => a.code == "Url");
                        if (scode != null)
                        {
                            ProjectConfig.KafkaUrl = scode.code_value;
                        }
                        var groupId = s.FirstOrDefault(a => a.code == "GroupId");
                        if (groupId != null)
                        {
                            ProjectConfig.KafkaGroup = groupId.code_value;
                        }
                    }
                    #endregion

                    #region 系统名称
                    var sysModel = sysDataDictionaryBLL.GetDictListByTypeCode("Sys");
                    if (sysModel != null && sysModel.Data != null && sysModel.Data.Count > 0)
                    {
                        var s = (List<SysDataDictionary>)sysModel.Data;
                        var scode = s.FirstOrDefault(a => a.code == "SysName");
                        if (scode != null)
                        {
                            ProjectConfig.SysName = scode.code_value;
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Log.Log4netHelper.Error(typeof(InitConfig), ex);
                throw ex;
            }
        }
    }
}
