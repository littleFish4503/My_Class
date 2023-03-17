using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace Class_类
{
    public class NumHeler
    {
        /// <summary>
        /// 数字操作
        /// </summary>
        public static class NumHelper
        {
            // <summary>
            /// 求一组数字中的最大值。
            /// </summary>
            /// <param name="values">一组数字。</param>
            /// <returns>最大值。</returns>
            public static double Max(params double[] values)
            {
                return values.Max();
            }

            /// <summary>
            /// 求一组数字中的最小值。
            /// </summary>
            /// <param name="values">一组数字。</param>
            /// <returns>最小值。</returns>
            public static double Min(params double[] values)
            {
                return values.Min();
            }

            /// <summary>
            /// 求一组数字的平均值。
            /// </summary>
            /// <param name="values">一组数字。</param>
            /// <returns>平均值。</returns>
            public static double Average(params double[] values)
            {
                return values.Average();
            }

            /// <summary>
            /// 对一个数字进行四舍五入，保留指定位数的小数。
            /// </summary>
            /// <param name="value">要进行四舍五入的数字。</param>
            /// <param name="decimals">保留的小数位数，默认为 0。</param>
            /// <returns>四舍五入后的数字。</returns>
            public static double Round(double value, int decimals = 0)
            {
                return Math.Round(value, decimals);
            }
            /// <summary>
            /// 根据up-down最大最小判断输入数值是否在范围内
            /// </summary>
            /// <param name="Num"></param>
            /// <param name="Value"></param>
            static void FillNum(System.Windows.Forms.NumericUpDown Num, decimal Value)
            {
                if (Value < Num.Minimum) Value = Num.Minimum;
                else if (Value > Num.Maximum) Value = Num.Maximum;

                Num.Value = Value;
            }
            /// <summary>
            /// 根据up-down最大最小判断输入数值是否在范围内
            /// </summary>
            /// <param name="Num"></param>
            /// <param name="dValue"></param>
            public static void SafeFillNum(System.Windows.Forms.NumericUpDown Num, double dValue)
            {
                FillNum(Num, (decimal)dValue);
            }

            public static void SafeFillNum(System.Windows.Forms.NumericUpDown Num, int iValue)
            {
                FillNum(Num, (decimal)iValue);
            }

            public static void SafeFillNum(System.Windows.Forms.NumericUpDown Num, float fValue)
            {
                FillNum(Num, (decimal)fValue);
            }

            public static void MinMax<T>(ref T Value, T Min, T Max) where T : IComparable
            {
                if (Value.CompareTo(Min) < 0)
                {
                    Value = Min;
                }
                else if (Value.CompareTo(Max) > 0)
                {
                    Value = Max;
                }
            }


            /// <summary>
            /// 将列表中的数据转换为字符串（  (1.23,4.56,7.89) 转换为 "1.23,4.56,7.89" ）
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="lstData"></param>
            /// <returns></returns>
            public static string GetStringFromList<T>(IEnumerable<T> lstData)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var data in lstData)
                {
                    sb.AppendFormat("{0},", data);
                }

                return sb.ToString();
            }


            public static ushort CRC(List<byte> ListData)
            {
                ushort itemp = 0xFFFF;
                bool Loop = true;
                int iCount = 0;
                while (Loop)
                {
                    if (iCount >= ListData.Count)
                    {
                        Loop = false;
                        break;
                    }

                    itemp ^= ListData[iCount];
                    iCount++;

                    for (int i = 0; i < 8; i++)
                    {
                        int x = itemp & 0x01;

                        if (x > 0)
                        {
                            itemp >>= 1;
                            itemp ^= 0xA001;
                        }
                        else
                        {
                            itemp >>= 1;
                        }

                    }

                }


                return itemp;
            }

            /// <summary>
            /// 返回CRC16校验码，注意 outLo在前，outHi在后
            /// </summary>
            /// <param name="aryBytes"></param>
            /// <param name="outLo"></param>
            /// <param name="outHi"></param>
            static public void CRC(byte[] aryBytes, out byte outLo, out byte outHi)
            {
                int CRC16Lo, CRC16Hi; // CRC寄存器
                int CL, CH; // 多项式码&HA001
                int SaveHi, SaveLo;

                CRC16Lo = 0xFF;
                CRC16Hi = 0xFF;
                CL = 0x01;
                CH = 0xA0;

                int len = aryBytes.GetLength(0);

                for (int i = 0; i < len; i++)
                {
                    CRC16Lo = CRC16Lo ^ aryBytes[i]; // 每一个数据与CRC寄存器进行异或
                    for (int flag = 0; flag <= 7; flag++)
                    {
                        SaveHi = CRC16Hi;
                        SaveLo = CRC16Lo;
                        CRC16Hi = CRC16Hi >> 1; // 高位右移一位
                        CRC16Lo = CRC16Lo >> 1; // 低位右移一位
                        if ((SaveHi & 0x1) == 0x01) // 如果高位字节最后一位为1
                        {
                            CRC16Lo = CRC16Lo | 0x80;// 则低位字节右移后前面补1
                        } // 否则自动补0
                        if ((SaveLo & 0x01) == 0x01) // 如果LSB为1，则与多项式码进行异或
                        {
                            CRC16Hi = CRC16Hi ^ CH;
                            CRC16Lo = CRC16Lo ^ CL;
                        }
                    }
                }

                int[] aryRet = new int[2];
                aryRet[0] = CRC16Lo;
                aryRet[1] = CRC16Hi;

                outLo = (byte)CRC16Lo;
                outHi = (byte)CRC16Hi;
            }


            public static void GetCRC(byte[] byteData, int byteLength, out byte hight, out byte low)
            {
                UInt16 wCrc = 0xFFFF;
                for (int i = 0; i < byteLength; i++)
                {
                    wCrc ^= Convert.ToUInt16(byteData[i]);
                    for (int j = 0; j < 8; j++)
                    {
                        if ((wCrc & 0x0001) == 1)
                        {
                            wCrc >>= 1;
                            wCrc ^= 0xA001;//异或多项式
                        }
                        else
                        {
                            wCrc >>= 1;
                        }
                    }
                }
                // CRC[1] = (byte)((wCrc & 0xFF00) >> 8);//高位在后
                //CRC[0] = (byte)(wCrc & 0x00FF);       //低位在前

                hight = (byte)((wCrc & 0xFF00) >> 8);//高位在后
                low = (byte)(wCrc & 0x00FF);       //低位在前  
            }



            /// <summary>
            /// 字符串转换为整数
            /// </summary>
            /// <param name="sValue"></param>
            /// <returns></returns>
            public static int StrToInt(string sValue)
            {
                return StrToInt(sValue, 0);
            }

            /// <summary>
            /// 字符串转换为整数
            /// </summary>
            /// <param name="sValue"></param>
            /// <param name="iDefault"></param>
            /// <returns></returns>
            public static int StrToInt(string sValue, int iDefault)
            {
                int iValue = iDefault;
                if (!int.TryParse(sValue, out iValue))
                {
                    iValue = iDefault;
                }

                return iValue;
            }

            /// <summary>
            /// 字符串转换为实数
            /// </summary>
            /// <param name="sValue"></param>
            /// <returns></returns>
            public static double StrToDouble(string sValue)
            {
                return StrToDouble(sValue, 0);
            }

            /// <summary>
            /// 字符串转换为实数
            /// </summary>
            /// <param name="sValue"></param>
            /// <param name="dbDefault"></param>
            /// <returns></returns>
            public static double StrToDouble(string sValue, double dbDefault)
            {
                double dbValue = dbDefault;
                if (!double.TryParse(sValue, out dbValue))
                {
                    dbValue = dbDefault;
                }
                return dbValue;
            }

            /// <summary>
            /// 字符串转换为布尔值
            /// </summary>
            /// <param name="sValue"></param>
            /// <returns></returns>
            public static bool StrToBool(string sValue)
            {
                return StrToBool(sValue, false);
            }


            /// <summary>
            /// 字符串转换为布尔值
            /// </summary>
            /// <param name="sValue"></param>
            /// <param name="bDefault"></param>
            /// <returns></returns>
            public static bool StrToBool(string sValue, bool bDefault)
            {
                bool bValue = bDefault;
                if (!bool.TryParse(sValue, out bValue))
                {
                    bValue = bDefault;
                }

                return bValue;
            }
        }


    }
}
