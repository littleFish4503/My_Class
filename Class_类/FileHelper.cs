using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_类
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 获取当前目录下文件路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件路径</returns>
        public static string GetFilePath(string fileName)
        {
            string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(directory, fileName);
        }
        public static string GetFilePath() { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        /// <summary>
        /// 创建目录（如果目录不存在）
        /// </summary>
        /// <param name="path">目录路径</param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 读取文件：用于读取文件内容，接受一个文件路径作为输入，返回文件内容的字符串表示。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }
            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 写入文件：用于写入文件内容，接受一个文件路径和文件内容作为输入，将内容写入指定的文件中。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void Write(string path, string content)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(content);
            }
        }

        /// <summary>
        /// 复制文件：用于复制文件，接受一个源文件路径和目标文件路径作为输入，将源文件复制到指定的目标路径下。
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        public static void Copy(string sourcePath, string destinationPath)
        {
            File.Copy(sourcePath, destinationPath);
        }

        /// <summary>
        /// 删除文件：用于删除文件，接受一个文件路径作为输入，删除指定的文件。
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        public static void Move(string sourceFilePath, string destinationFilePath)
        {
            File.Move(sourceFilePath, destinationFilePath);
        }
        /// <summary>
        /// 读取指定路径的文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件内容</returns>
        public static string ReadTextFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">文件内容</param>
        public static void WriteTextFile(string path, string content)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(content);
            }
        }

        /// <summary>
        /// 读取二进制文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>byte[] 数组</returns>
        public static byte[] ReadBinaryFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                return bytes;
            }
        }

        /// <summary>
        /// 写入二进制文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="bytes">byte[] 数组</param>
        public static void WriteBinaryFile(string path, byte[] bytes)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}

