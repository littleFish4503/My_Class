using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_类
{
    /// <summary>
    /// 坐标转换等换算
    /// </summary>
    public static class MathHepler
    {
        /// <summary>
        /// 将路径列表转化新的坐标系中去
        /// </summary>
        /// <param name="basePt">模板的基点坐标值</param>
        /// <param name="baseCoords">模板的路径列表</param>
        /// <param name="diffAng">旋转量角度（360度制）（从0-360度，逆时针方向为正）</param>
        /// <param name="newPt">新的基点坐标值</param>
        /// <returns></returns>
        public static List<PointF> TransPoints(PointF basePt, List<PointF> baseCoords, float diffAng, PointF newPt)
        {
            // 第一步：旋转
            List<PointF> newCoords1 = new List<PointF>();
            foreach (PointF itemPt in baseCoords)
            {
                double dist = GetDist(basePt, itemPt);
                PointF tmpPt = Rotate1(basePt, itemPt, diffAng);
                newCoords1.Add(tmpPt);
            }

            // 第二步：平移
            List<PointF> newCoords2 = new List<PointF>();
            float diffX = newPt.X - basePt.X;
            float diffY = newPt.Y - basePt.Y;
            foreach (var item in newCoords1)
            {
                float newX = item.X + diffX;
                float newY = item.Y + diffY;
                PointF tmpPt = new PointF(newX, newY);
                newCoords2.Add(tmpPt);
            }

            return newCoords2;
        }


        /// <summary>
        /// 将路径列表转化新的坐标系中去（这个函数还需要再多做验证）
        /// </summary>
        /// <param name="basePt">模板的基点坐标值</param>
        /// <param name="baseCoords">模板的路径列表</param>
        /// <param name="diffAng">旋转量角度（360度制）（从0-360度，逆时针方向为正，顺时针方向为负）</param>
        /// <param name="newPt">新的基点坐标值</param>
        /// <returns></returns>
        public static List<PointF> TransPoints2(PointF basePt, List<PointF> baseCoords, double diffAng, PointF newPt)
        {
            Matrix m = new Matrix();
            m.RotateAt((float)diffAng, basePt);

            float offsetX = newPt.X - basePt.X;
            float offsetY = newPt.Y - basePt.Y;
            m.Translate(offsetX, offsetY, MatrixOrder.Append);

            PointF[] pts = new PointF[baseCoords.Count];
            for (int i = 0; i < baseCoords.Count; i++)
            {
                pts[i] = baseCoords[i];
            }

            m.TransformPoints(pts);

            List<PointF> newPts = new List<PointF>();
            newPts.AddRange(pts);

            return newPts;
        }


        /// <summary>
        /// <para> 圆心Sin/Cos </para>
        /// 得到旋转后的B点的坐标值（ptCir为圆心坐标，ptA为要旋转的点坐标，ang为要旋转的角度（从0-360度，逆时针方向为正，顺时针方向为负））
        /// </summary>
        /// <param name="ptCenter">圆心坐标</param>
        /// <param name="ptA">要旋转的点坐标</param>
        /// <param name="degAng">角度（360度制）（从0-360度，逆时针方向为正，顺时针方向为负）</param>
        /// <returns></returns>
        public static PointF Rotate1(PointF ptCenter, PointF ptA, double degAng)
        {
            double angle_A_Cir = GetAngle_A_B(ptCenter, ptA);
            double angle_B_Cir = angle_A_Cir + degAng;
            double angArc = angle_B_Cir / 180 * Math.PI;

            double radius = GetDist(ptCenter, ptA);

            double diffX = radius * Math.Cos(angArc);
            double diffY = radius * Math.Sin(angArc);


            float ptB_X = ptCenter.X + (float)diffX;
            float ptB_Y = ptCenter.Y + (float)diffY;

            return new PointF(ptB_X, ptB_Y);
        }

        /// <summary>
        /// <para> 网上算法Sin/Cos </para>
        /// 得到旋转后的B点的坐标值（ptCir为圆心坐标，ptA为要旋转的点坐标，ang为要旋转的角度（从0-360度，逆时针方向为正，顺时针方向为负））
        /// </summary>
        /// <param name="ptCenter">圆心坐标</param>
        /// <param name="ptA">要旋转的点坐标</param>
        /// <param name="degAng">角度（360度制）（从0-360度，逆时针方向为正，顺时针方向为负）</param>
        /// <returns></returns>
        public static PointF Rotate2(PointF ptCenter, PointF ptA, double degAng)
        {
            // 网上查找的算法：
            // 2D上的点围绕某另一个点旋转： If you rotate point (px, py) around point (ox, oy) by angle theta you’ll get:
            // p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
            // p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy
            // this is an easy way to rotate a point in 2D.

            double dX = ptA.X - ptCenter.X;
            double dY = ptA.Y - ptCenter.Y;
            double dAng_Arc = degAng / 180 * Math.PI;

            double dstX = Math.Cos(dAng_Arc) * dX - Math.Sin(dAng_Arc) * dY + ptCenter.X;
            double dstY = Math.Sin(dAng_Arc) * dX + Math.Cos(dAng_Arc) * dY + ptCenter.Y;


            return new PointF((float)dstX, (float)dstY);
        }

        /// <summary>
        /// <para> Matrix计算 </para>
        /// 得到旋转后的B点的坐标值（ptCir为圆心坐标，ptA为要旋转的点坐标，ang为要旋转的角度（从0-360度，逆时针方向为正，顺时针方向为负））
        /// </summary>
        /// <param name="ptCenter">圆心坐标</param>
        /// <param name="ptA">要旋转的点坐标</param>
        /// <param name="degAng">角度（360度制）（从0-360度，逆时针方向为正，顺时针方向为负）</param>
        /// <returns></returns>
        public static PointF Rotate3(PointF ptCenter, PointF ptA, double degAng)
        {
            Matrix m = new Matrix();
            m.RotateAt((float)degAng, ptCenter);

            PointF[] pts = new PointF[] { ptA, };
            m.TransformPoints(pts);

            return pts[0];
        }

        ///// <summary>
        ///// <para> halcon坐标转换 </para>
        ///// 得到旋转后的B点的坐标值（ptCir为圆心坐标，ptA为要旋转的点坐标，ang为要旋转的角度（从0-360度，逆时针方向为正，顺时针方向为负））
        ///// </summary>
        ///// <param name="ptCenter">圆心坐标</param>
        ///// <param name="ptA">要旋转的点坐标</param>
        ///// <param name="degAng">角度（360度制）（从0-360度，逆时针方向为正，顺时针方向为负）</param>
        ///// <returns></returns>
        //public static PointF Rotate4(PointF ptCenter, PointF ptA, double degAng)
        //{
        //    double oldAng = GetAngle_A_B(ptCenter, ptA);
        //    double oldAng_arc = oldAng / 180 * Math.PI;
        //    double newAng_arc = (oldAng + degAng) / 180 * Math.PI;
        //    CTrans trans = new CTrans(ptCenter, (float)oldAng_arc, (float)newAng_arc);
        //    return trans.GetPoint(ptA);
        //}

        /// <summary>
        /// 以ptCenter为圆心，以radius为半径，以ang为角度（角度制），得到极坐标的值（从0-360度，逆时针方向为正，顺时针方向为负）
        /// </summary>
        /// <param name="ptCenter">圆心坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="degAng">角度（360度制）（从0-360度，逆时针方向为正，顺时针方向为负）</param>
        /// <returns></returns>
        public static PointF Polar(PointF ptCenter, double radius, double degAng)
        {
            double arcAng = degAng / 180 * Math.PI;

            double diffX = radius * Math.Cos(arcAng);
            double diffY = radius * Math.Sin(arcAng);

            float newX = ptCenter.X + (float)diffX;
            float newY = ptCenter.Y + (float)diffY;

            return new PointF(newX, newY);
        }


        /// <summary>
        /// 得到A点与B点之间的距离
        /// </summary>
        /// <param name="PtA"></param>
        /// <param name="PtB"></param>
        /// <returns></returns>
        public static double GetDist(PointF PtA, PointF PtB)
        {
            float dX = PtA.X - PtB.X;
            float dY = PtA.Y - PtB.Y;

            double dist = Math.Sqrt(dX * dX + dY * dY);

            return dist;
        }

        public static double GetDist(double aX, double aY, double bX, double bY)
        {
            PointF ptA = new PointF((float)aX, (float)aY);
            PointF ptB = new PointF((float)bX, (float)bY);

            return GetDist(ptA, ptB);
        }

        public static double GetAngle_A_B(double aX, double aY, double bX, double bY)
        {
            PointF ptA = new PointF((float)aX, (float)aY);
            PointF ptB = new PointF((float)bX, (float)bY);

            return GetAngle_A_B(ptA, ptB);
        }

        /// <summary>
        /// 得到B点到A点的角度(返回0到 360的角度)（相当于A点为零点，B点为相对点）
        /// </summary>
        /// <param name="PtA">A点为零点</param>
        /// <param name="PtB">B点为相对点</param>
        /// <returns></returns>
        public static double GetAngle_A_B(PointF PtA, PointF PtB)
        {
            double Angle = 0.0;

            if (Math.Abs(PtA.Y - PtB.Y) <= 0.0001) // 当A点与B点的Y坐标相同的时候
            {
                if (PtA.X < PtB.X)
                {
                    Angle = 0;
                }
                else
                {
                    Angle = Math.PI;
                }
            }
            else if (Math.Abs(PtA.X - PtB.X) <= 0.0001) // 当A点与B点的X坐标相同的时候
            {
                if (PtA.Y < PtB.Y)
                {
                    Angle = Math.PI * 0.5;
                }
                else
                {
                    Angle = Math.PI * 1.5;
                }
            }
            else
            {
                Angle = Math.Atan((PtB.Y - PtA.Y) / (PtB.X - PtA.X));

                // 根据四个象限区间来看，不同的时候要加上不同的角度  
                // （相当于A点为零点，B点为相对点）
                double dX = PtB.X - PtA.X;
                double dY = PtB.Y - PtA.Y;

                if (dX > 0 && dY > 0)
                {
                    // 第一象限
                }
                else if (dX < 0 && dY > 0)
                {
                    // 第二象限
                    Angle = Math.PI + Angle;
                }
                else if (dX < 0 && dY < 0)
                {
                    // 第三象限
                    Angle = Math.PI + Angle;
                }
                else if (dX > 0 && dY < 0)
                {
                    // 第四象限
                    Angle = Math.PI * 2 + Angle;
                }
            }

            return Angle * 180.0 / Math.PI;
        }

        public static double GetAngle_A_B_1_4(double ptA_X, double ptA_Y, double ptB_X, double ptB_Y)
        {
            PointF ptA = new PointF((float)ptA_X, (float)ptA_Y);
            PointF ptB = new PointF((float)ptB_X, (float)ptB_Y);

            return GetAngle_A_B_1_4(ptA, ptB);
        }

        /// <summary>
        /// 得到直线的角度(返回 -89.99999到 90.0000 的角度)（第一象限和第四象限）
        /// </summary>
        /// <param name="PtA"></param>
        /// <param name="PtB"></param>
        /// <returns></returns>
        public static double GetAngle_A_B_1_4(PointF PtA, PointF PtB)
        {
            if (GetDist(PtA, PtB) < 0.00001)
            {
                return 0;
            }

            double angle = GetAngle_A_B(PtA, PtB);
            if (angle > 90 && angle <= 180)
            {
                angle = angle - 180;
            }
            else if (angle > 180 && angle <= 270)
            {
                angle = angle - 180;
            }
            else if (angle > 270 && angle <= 360)
            {
                angle = angle - 360;
            }

            return angle;
        }

        ///// <summary>
        ///// 调用Halcon的HMsic函数来计算两点与X轴相差的夹角
        ///// </summary>
        ///// <param name="PtA"></param>
        ///// <param name="PtB"></param>
        ///// <returns></returns>
        //public static double GetAngleLx_A_B(PointF PtA, PointF PtB)
        //{
        //    double x1 = PtA.X;
        //    double y1 = PtA.Y;

        //    double x2 = PtB.X; // X = 72.5273     Y = 12.7885 
        //    double y2 = PtB.Y;
        //    double arcAng = HMisc.AngleLx(y1, x1, y2, x2);
        //    double degAng = arcAng * 180 / Math.PI;

        //    return degAng;
        //}

        ///// <summary>
        ///// 调用Halcon的HMsic函数来计算两条线的夹角
        ///// </summary>
        ///// <param name="lineA_StartPt"></param>
        ///// <param name="lineA_EndPt"></param>
        ///// <param name="lineB_StartPt"></param>
        ///// <param name="lineB_EndPt"></param>
        ///// <returns></returns>
        //public static double GetAngleLL(PointF lineA_StartPt, PointF lineA_EndPt, PointF lineB_StartPt, PointF lineB_EndPt)
        //{
        //    double rA1 = lineA_StartPt.Y;
        //    double cA1 = lineA_StartPt.X;

        //    double rA2 = lineA_EndPt.Y;
        //    double cA2 = lineA_EndPt.X;

        //    double rB1 = lineB_StartPt.Y;
        //    double cB1 = lineB_StartPt.X;

        //    double rB2 = lineB_EndPt.Y;
        //    double cB2 = lineB_EndPt.X;

        //    return HMisc.AngleLl(rA1, cA1, rA2, cA2, rB1, cB1, rB2, cB2);
        //}

        /// <summary> 保证角度在0-360度 </summary> 
        public static void BesureAngle_360(ref double angle)
        {
            while (angle < 0)
            {
                angle += 360;
            }
            angle = angle % 360;
        }

        /// <summary> (保证角度在 -89.99999到 90.0000 的角度)（第一象限和第四象限）</summary> 
        public static void BesureAngle_1_4(ref double angle)
        {
            BesureAngle_360(ref angle);

            if (angle >= 0 && angle <= 90) // 第一象限
            {
                ;
            }
            else if (angle > 90 && angle <= 180) // 第二象限
            {
                angle = angle - 180;
            }
            else if (angle > 180 && angle <= 270)  // 第三象限
            {
                angle = angle - 180;
            }
            else if (angle > 270)   // 第四象限
            {
                angle = angle - 360;
            }

        }

        /// <summary>
        /// 根据三点来计算圆心（用作三点画圆或三点画弧）
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="ptCenter">圆心坐标</param>
        /// <param name="radius">半径</param>
        /// <returns></returns>
        public static bool GetCircleInfo(PointF pt0, PointF pt1, PointF pt2, out PointF ptCenter, out float radius)
        {
            float fTol = 0.000001f;

            ptCenter = new PointF();
            radius = 0;


            { // 去除不符合数据的情况
                List<double> lstDist = new List<double>()
                {
                    GetDist(pt0,pt1),
                    GetDist(pt1,pt2),
                    GetDist(pt2,pt0),
                };

                foreach (double dist in lstDist) // 如果有很短的线段，立即返回false
                {
                    if (dist <= fTol * 2)
                    {
                        return false;
                    }
                }
            }

            float a = 2 * (pt1.X - pt0.X);
            float b = 2 * (pt1.Y - pt0.Y);
            float c = pt1.X * pt1.X + pt1.Y * pt1.Y - pt0.X * pt0.X - pt0.Y * pt0.Y;
            float d = 2 * (pt2.X - pt1.X);
            float e = 2 * (pt2.Y - pt1.Y);
            float f = pt2.X * pt2.X + pt2.Y * pt2.Y - pt1.X * pt1.X - pt1.Y * pt1.Y;
            float x = (b * f - e * c) / (b * d - e * a);
            float y = (d * c - a * f) / (b * d - e * a);

            radius = (float)Math.Sqrt((double)((x - pt0.X) * (x - pt0.X) + (y - pt0.Y) * (y - pt0.Y)));
            ptCenter = new PointF(x, y);

            return true;
        }


        /// <summary>
        /// 得到直线函数的k和b  (y=kx+b) （注意：这条线不能 垂直或水平线）
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="k"></param>
        /// <param name="b"></param>
        public static void GetLine_k_b(PointF pt1, PointF pt2, out double k, out double b)
        {
            float x1 = pt1.X, y1 = pt1.Y;
            float x2 = pt2.X, y2 = pt2.Y;

            k = (y1 - y2) / (x1 - x2); // 注意 (x1-x2) 不能等于0
            b = y1 - k * x1;
        }


        /// <summary>
        /// true表示顺时针，false表示逆时针（这个函数好像有问题，以好要慎用）
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static bool GetClockDir(PointF[] pts)
        {
            double m1 = 0;
            double m2 = 0;

            for (int i = 0; i < pts.GetLength(0) - 1; i++)
            {
                PointF pt0 = pts[i + 0];
                PointF pt1 = pts[i + 1];

                m1 += (pt0.X * pt1.Y);
                m2 += (pt1.X * pt0.Y);
            }

            if (m2 - m1 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// true表示顺时针，false表示逆时针，
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <returns></returns>
        public static bool GetClockDir(PointF pt1, PointF pt2, PointF pt3)
        {
            float x1 = pt1.X, y1 = pt1.Y;
            float x2 = pt2.X, y2 = pt2.Y;
            float x3 = pt3.X, y3 = pt3.Y;

            float m = (x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1);

            // m=0 为三点共线

            if (m < 0)
            {
                return true;// 顺时针
            }
            else
            {
                return false;// 逆时针
            }
        }

        static void GetArcInfo(PointF arcStart, PointF arcMid, PointF arcEnd,
                               out float radius, out PointF ptCenter,
                               out float startAngle, out float endAngle,
                               out bool clockDir)
        {
            GetCircleInfo(arcStart, arcMid, arcEnd, out ptCenter, out radius);

            startAngle = (float)GetAngle_A_B(ptCenter, arcStart);
            endAngle = (float)GetAngle_A_B(ptCenter, arcEnd);
            clockDir = GetClockDir(arcStart, arcMid, arcEnd);

            if (clockDir) // 顺时针
            {
                if (startAngle < endAngle)
                {
                    startAngle += 360;
                }
            }
            else // 逆时针
            {
                if (startAngle > endAngle)
                {
                    endAngle += 360;
                }
            }
        }



        /// <summary>
        /// 三点是否共线（true表示三点共线，false表示三点不共线）
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <returns></returns>
        public static bool IsSameLine(PointF pt1, PointF pt2, PointF pt3)
        {
            float x1 = pt1.X, y1 = pt1.Y;
            float x2 = pt2.X, y2 = pt2.Y;
            float x3 = pt3.X, y3 = pt3.Y;

            float m = (x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1);

            if (m == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 将直线根据长度拆分成细分点集合
        /// </summary>
        /// <param name="ptStart">直线的起点</param>
        /// <param name="ptEnd">直线的结束点</param>
        /// <param name="aoi_gap">单段长度</param>
        /// <returns></returns>
        public static List<PointF> SplitLine(PointF ptStart, PointF ptEnd, float aoi_gap)
        {
            List<PointF> lstPt = new List<PointF>();
            if (aoi_gap < 0.05) { aoi_gap = 0.05f; }
            float fTol = 0.000001f; // 

            float x1 = ptStart.X;
            float y1 = ptStart.Y;

            float x2 = ptEnd.X;
            float y2 = ptEnd.Y;

            double lenLine = GetDist(ptStart, ptEnd);

            if (lenLine < aoi_gap) // 假如两个点之间的距离小于一个间距
            {
                ;
            }
            else if (lenLine <= 2 * aoi_gap) // 假如两个点之间的距离小于两个间距
            {
                PointF ptMid = new PointF();
                ptMid.X = (x1 + x2) / 2;
                ptMid.Y = (y1 + y2) / 2;

                lstPt.Add(ptMid);
            }
            else if (Math.Abs(y2 - y1) < fTol) // Y差值很小，表示为水平线
            {
                int count = (int)(Math.Abs(x2 - x1) / aoi_gap);
                float fGap = (x2 - x1) / count;

                for (int i = 1; i < count; i++)
                {
                    float tmpX = x1 + fGap * i;
                    PointF tmpPt = new PointF(tmpX, y1);

                    lstPt.Add(tmpPt);
                }
            }
            else if (Math.Abs(x2 - x1) < fTol) // X差值很小，表示为垂直线
            {
                int count = (int)(Math.Abs(y2 - y1) / aoi_gap);
                float fGap = (y2 - y1) / count;

                for (int i = 1; i < count; i++)
                {
                    float tmpY = y1 + fGap * i;
                    PointF tmpPt = new PointF(x1, tmpY);
                    lstPt.Add(tmpPt);
                }
            }
            else
            {
                // 到这里那肯定是斜线啦 Y=k*X+b
                float k = (y2 - y1) / (x2 - x1);
                float b = y1 - k * x1;

                double A = Math.Atan(k); // 角度（弧度制）

                if (x1 > x2) { A += Math.PI; }

                double degAng = A * 180 / Math.PI;

                int count = (int)(lenLine / aoi_gap);
                for (int i = 1; i < count; i++)
                {
                    double tmpX = x1 + (i * aoi_gap) * Math.Cos(A);
                    double tmpY = y1 + (i * aoi_gap) * Math.Sin(A);
                    PointF tmpPt = new PointF((float)tmpX, (float)tmpY);
                    lstPt.Add(tmpPt);
                }
            }

            return lstPt;
        }


        /// <summary>
        /// 将圆根据长度拆分成细分点集合
        /// </summary>
        /// <param name="ptCenter">圆心坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="aoi_gap">单段长度</param>
        /// <returns></returns>
        public static List<PointF> SplitCircle(PointF ptCenter, float radius, float aoi_gap)
        {
            List<PointF> lstPt = new List<PointF>();
            float lenCircle = (float)(Math.PI * 2 * radius);
            if (aoi_gap < 0.05) { aoi_gap = 0.05f; }

            if (lenCircle <= 2 * aoi_gap) // 圆周长小于两个间距
            {
                ;
            }
            else
            {
                int count = (int)(lenCircle / aoi_gap);
                double addArcAng = 360.0 / count;
                for (int i = 0; i < count; i++)
                {
                    double tmpAng = i * addArcAng;
                    PointF tmpPt = Polar(ptCenter, radius, tmpAng);

                    lstPt.Add(tmpPt);
                }
            }


            return lstPt;
        }

        /// <summary>
        /// 将圆弧根据长度拆分成细分点集合
        /// </summary>
        /// <param name="arcStart">圆弧的起点</param>
        /// <param name="arcMid">圆弧的中间点</param>
        /// <param name="arcEnd">圆弧的结束点</param>
        /// <param name="aoi_gap"></param>
        /// <returns>单段长度</returns>
        public static List<PointF> SplitArc(PointF arcStart, PointF arcMid, PointF arcEnd, float aoi_gap)
        {
            List<PointF> lstPt = new List<PointF>();
            if (aoi_gap < 0.05) { aoi_gap = 0.05f; }

            PointF ptCenter;
            float radius;
            float startAngle;
            float endAngle;
            bool clockDir;

            GetArcInfo(arcStart, arcMid, arcEnd, out radius, out ptCenter, out startAngle, out endAngle, out clockDir);

            double degDiffAng = endAngle - startAngle;
            double arcDiffAng = degDiffAng / 180 * Math.PI;

            double arcLen = radius * arcDiffAng; // 弧长

            if (Math.Abs(arcLen) < aoi_gap) // 小于一个间距，直接略过了
            {
                ;
            }
            else if (Math.Abs(arcLen) <= 2 * aoi_gap) // 小于等于两个间距，直接用弧中间点
            {
                lstPt.Add(arcMid);
            }
            else
            {
                int count = (int)(Math.Abs(arcLen) / aoi_gap);
                double addArcAng = degDiffAng / count;

                addArcAng = Math.Abs(addArcAng);

                for (int i = 1; i < count; i++)
                {
                    if (clockDir) // 顺时针
                    {
                        double tmpAng = startAngle - i * addArcAng;
                        PointF tmpPt = Polar(ptCenter, radius, tmpAng);
                        lstPt.Add(tmpPt);
                    }
                    else // 逆时针
                    {
                        double tmpAng = startAngle + i * addArcAng;
                        PointF tmpPt = Polar(ptCenter, radius, tmpAng);
                        lstPt.Add(tmpPt);
                    }
                }
            }


            return lstPt;
        }


        /// <summary>
        /// 根据相机视野的大小将细分点集合拆分成相机检测
        /// </summary>
        /// <param name="allItemPts"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <returns></returns>
        public static List<List<PointF>> GetSizePts(List<List<PointF>> allItemPts, float sizeX, float sizeY)
        {
            List<List<PointF>> sizePts = new List<List<PointF>>();

            foreach (var oneItem in allItemPts)
            {
                List<PointF> tmpPts = new List<PointF>();
                foreach (var pt in oneItem)
                {
                    tmpPts.Add(pt);
                    if (tmpPts.Count >= 2)
                    {
                        float minX, maxX, minY, maxY;
                        CalMinMax(tmpPts, out minX, out maxX, out minY, out maxY);

                        if (maxX - minX >= sizeX || maxY - minY >= sizeY)
                        {
                            sizePts.Add(new List<PointF>(tmpPts));
                            tmpPts = new List<PointF>();
                        }
                    }
                }

                if (tmpPts.Count >= 2)
                {
                    sizePts.Add(new List<PointF>(tmpPts));
                }
            }


            return sizePts;
        }


        /// <summary>
        /// 得到点集合的 minX,maxX,minY,maxY四个极限值
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        static void CalMinMax(List<PointF> pts, out float minX, out float maxX, out float minY, out float maxY)
        {
            minX = pts[0].X;
            maxX = minX;

            minY = pts[0].Y;
            maxY = minY;

            for (int i = 1; i < pts.Count; i++)
            {
                PointF tmp = pts[i];

                if (tmp.X < minX) { minX = tmp.X; }
                if (tmp.X > maxX) { maxX = tmp.X; }

                if (tmp.Y < minY) { minY = tmp.Y; }
                if (tmp.Y > maxY) { maxY = tmp.Y; }
            }
        }

        /// <summary>
        /// 得到点集合的中心点
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static PointF GetMidPt(List<PointF> pts)
        {
            float minX, maxX, minY, maxY;
            CalMinMax(pts, out minX, out maxX, out minY, out maxY);

            float x = (minX + maxX) / 2f;
            float y = (minY + maxY) / 2f;

            return new PointF(x, y);
        }



        public static bool CalCenter2Point(PointF pA, PointF pB, double rotateAng, out PointF pCenter, out double radius)
        {
            pCenter = new PointF();
            radius = 0;

            double distAB = GetDist(pA, pB);
            if (distAB <= 0.1 || Math.Abs(rotateAng) < 0.1) { return false; }

            // R*Cos(ang)=distAB/2.0;
            // => R=distAB/2.0/Cos(ang)
            double degAng = 90 - rotateAng / 2.0;
            double arcAng = degAng / 180.0 * Math.PI;
            radius = distAB / 2.0 / Math.Cos(arcAng);

            // ChuiGao/(distAB/2.0)=tan(ang);
            // =>ChuiGao=tan(ang)*distAB/2.0;
            double ChuiGao = Math.Tan(arcAng) * distAB / 2.0;

            double midAB_X = (pA.X + pB.X) / 2.0;
            double midAB_Y = (pA.Y + pB.Y) / 2.0;

            double degAng_AB = GetAngle_A_B(pA, pB);
            double degAng_AB_Rotate = degAng_AB + 90;

            double diffX = ChuiGao * Math.Cos(degAng_AB_Rotate / 180.0 * Math.PI);
            double diffY = ChuiGao * Math.Sin(degAng_AB_Rotate / 180.0 * Math.PI);

            double circleX = midAB_X + diffX;
            double circleY = midAB_Y + diffY;

            pCenter.X = (float)circleX;
            pCenter.Y = (float)circleY;

            return true;
        }
    }
}
