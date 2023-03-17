using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Class_类
{
    /// <summary>
    /// 用于处理字符串的帮助类
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 将字符串转换为 byte[] 数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>byte[] 数组</returns>
        public static byte[] ConvertToBytes(string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 将 byte[] 数组转换为字符串
        /// </summary>
        /// <param name="bytes">byte[] 数组</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>字符串</returns>
        public static string ConvertToString(byte[] bytes, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将字符串进行 MD5 加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5Encrypt(string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(str);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
