using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Class_类
{
    /// <summary>
    /// 通用数据访问类，连接字符串写App.config文件下的ConnectionStrings
    /// </summary>
   public class SQLHelper
    {
        
        private static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ToString();
        /// <summary>
        /// 返回一行一列结果的方法
        /// </summary>
        /// <param name="commStr">sql语句</param>
        /// <returns></returns>
        public static object GetSingleResult(string commStr)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(commStr, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //写错误日志~
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        /// <summary>
        /// 返回一行一列结果的方法
        /// </summary>
        /// <param name="commStr">sql语句</param>
        /// <returns></returns>
        public static object GetSingleResult(string commStr, SqlParameter[] sqlParameters)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(commStr, conn);
            foreach (SqlParameter parameter in sqlParameters)
            {
                cmd.Parameters.Add(parameter);
            }
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //写错误日志~
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// 更新操作（insert，update，delete），返回受影响的行数
        /// </summary>
        /// <param name="commStr"></param>
        /// <returns></returns>
        public static int Update(string commStr)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(commStr, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //写错误日志~
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 更新操作（insert，update，delete），返回受影响的行数
        /// </summary>
        /// <param name="commStr"></param>
        /// <returns></returns>
        public static int Update(string commStr, SqlParameter[] sqlParameters)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(commStr, conn);
           
            foreach (SqlParameter parameter in sqlParameters)
            {
                cmd.Parameters.Add(parameter);
            }
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //写错误日志~
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public static  int Update(SqlCommand cmd,string commStr)
        {
            SqlConnection conn = new SqlConnection(connStr);
            cmd.CommandText = commStr;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //写错误日志~
                throw ex;
            }
            finally
            {
                conn.Close();
            }


        } 
        /// <summary>
        /// 返回一个结果集，后面写关闭reader
        /// </summary>
        /// <param name="commStr"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string commStr)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(commStr, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 返回一个结果集，后面写关闭reader
        /// </summary>
        /// <param name="commStr"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string commStr, SqlParameter[] sqlParameters)
        {
            SqlConnection conn = new SqlConnection(connStr);
            using (SqlCommand cmd = new SqlCommand(commStr, conn))
            {
                
                foreach (SqlParameter parameter in sqlParameters)
                {
                    cmd.Parameters.Add(parameter);
                }
                try
                {
                    conn.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 得到datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql)
        {
            // 创建连接对象
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // 创建命令对象
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        // 打开连接
                        conn.Open();

                        // 创建数据适配器对象
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        // 创建数据表对象
                        DataTable dt = new DataTable();

                        // 填充数据
                        adapter.Fill(dt);

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        // 写错误日志
                        // ...

                        // 抛出异常
                        throw ex;
                    }
                }
            }
        }
        public static DataTable GetDataTable(string sql,SqlParameter[] sqlParameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // 创建命令对象
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    
                    foreach (SqlParameter parameter in sqlParameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    try
                    {
                        // 打开连接
                        conn.Open();

                        // 创建数据适配器对象
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        // 创建数据表对象
                        DataTable dt = new DataTable();

                        // 填充数据
                        adapter.Fill(dt);

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        // 写错误日志
                        // ...

                        // 抛出异常
                        throw ex;
                    }
                }
            }
        }





    }
}
