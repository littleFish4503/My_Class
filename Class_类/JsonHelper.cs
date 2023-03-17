using Newtonsoft.Json;

namespace Class_类
{
    public class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为 JSON 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
