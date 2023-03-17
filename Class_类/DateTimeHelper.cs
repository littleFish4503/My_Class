using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_类
{
    /// <summary>
    /// 用于处理日期时间的帮助类
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 将时间戳转换为本地时间
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns>本地时间</returns>
        public static DateTime ConvertToLocalTime(long timestamp)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return startTime.AddMilliseconds(timestamp);
        }

        /// <summary>
        /// 将本地时间转换为时间戳
        /// </summary>
        /// <param name="dateTime">本地时间</param>
        /// <returns>时间戳</returns>
        public static long ConvertToTimestamp(DateTime dateTime)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return (long)(dateTime - startTime).TotalMilliseconds;
        }
    }
}
