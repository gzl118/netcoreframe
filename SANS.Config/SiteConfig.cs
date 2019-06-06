using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Config
{
    /// <summary>
    /// 配置信息读取模型
    /// </summary>
    public static class SiteConfig
    {
        private static IConfiguration _appSection = null;
        public static string AppSetting(string section, string key)
        {
            string str = string.Empty;
            if (_appSection.GetSection(section) != null)
            {
                if (_appSection.GetSection(key) != null)
                    str = _appSection.GetSection(section).GetSection(key).Value;
            }
            return str;
        }

        public static void SetAppSetting(IConfiguration section)
        {
            _appSection = section;
        }

        public static string GetSite(string section, string apiName)
        {
            return AppSetting(section, apiName);
        }
    }
}
