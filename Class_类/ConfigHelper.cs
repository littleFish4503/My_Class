using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_类
{
    /// <summary>
    /// 用于读取和写入应用程序配置文件的帮助类
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// 读取指定配置项的值
        /// </summary>
        /// <param name="key">配置项的名称</param>
        /// <returns>配置项的值</returns>
        public static string Read(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 写入指定配置项的值
        /// </summary>
        /// <param name="key">配置项的名称</param>
        /// <param name="value">配置项的值</param>
        public static void Write(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
