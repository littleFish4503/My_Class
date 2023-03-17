using System;
using System.Security.Cryptography;
using System.Text;

namespace Class_类
{

    public class MD5Helper
    {
        /// <summary>
        /// 生成 MD5 值，用于生成 MD5 值，接受一个字符串作为输入，返回一个经过 MD5 加密的字符串。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// 验证 MD5 值，用于验证 MD5 值，接受一个字符串和一个 MD5 值作为输入，返回一个 bool 类型的值，表示输入字符串是否与 MD5 值匹配。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool Validate(string input, string hash)
        {
            string hashOfInput = Encrypt(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }

        /// <summary>
        /// 比较两个字符串是否相等；用于比较两个字符串是否相等，接受两个字符串作为输入，返回一个 bool 类型的值，表示两个字符串是否相等。
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        public static bool Compare(string input1, string input2)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(input1, input2) == 0;
        }
    }
}
