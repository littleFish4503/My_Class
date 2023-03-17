using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Class_类
{
    public class HttpRequestHelper
    {
        /// <summary>
        /// 发送 GET 请求并获取响应数据
        /// </summary>
        /// <param name="url">请求 URL</param>
        /// <returns>响应数据</returns>
        public static string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 发送 POST 请求并获取响应数据
        /// </summary>
        /// <param name="url">请求 URL</param>
        /// <param name="data">JSON 字符串</param>
        /// <returns>响应数据</returns>
        public static string Post(string url, string data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(data);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
