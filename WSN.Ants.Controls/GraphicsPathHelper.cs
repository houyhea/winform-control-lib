using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WSN.Ants.Controls
{
    /// <summary>
    /// 画图路径帮助类
    ///     GraphicsPathHelper
    /// </summary>
    public static class GraphicsPathHelper
    {
        /// <summary>
        /// 建立带有圆角样式的路径。
        /// </summary>
        /// <param name="rect">用来建立路径的矩形。</param>
        /// <param name="_radius">圆角的大小。</param>
        /// <param name="style">圆角的样式。</param>
        /// <param name="correction">是否把矩形长宽减 1,以便画出边框。</param>
        /// <returns>建立的路径。</returns>
        public static GraphicsPath CreatePath(
            Rectangle rect, int radius, RoundStyle style, bool correction)
        {
            GraphicsPath path = new GraphicsPath();
            int radiusCorrection = correction ? 1 : 0;
            switch (style)
            {
                case RoundStyle.None:
                    path.AddRectangle(new Rectangle(rect.X,rect.Y,rect.Width-radiusCorrection,rect.Height-radiusCorrection));
                    break;
                case RoundStyle.All:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Bottom - radius - radiusCorrection,
                        radius ,
                        radius , 0, 90);
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    break;
                case RoundStyle.Left:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddLine(
                        rect.Right - radiusCorrection, rect.Y,
                        rect.Right - radiusCorrection, rect.Bottom - radiusCorrection);
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    break;
                case RoundStyle.Right:
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddArc(
                       rect.Right - radius - radiusCorrection,
                       rect.Bottom - radius - radiusCorrection,
                       radius,
                       radius,
                       0,
                       90);
                    path.AddLine(rect.X, rect.Bottom - radiusCorrection, rect.X, rect.Y);
                    break;
                case RoundStyle.Top:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddLine(
                        rect.Right - radiusCorrection, rect.Bottom - radiusCorrection,
                        rect.X, rect.Bottom - radiusCorrection);
                    break;
                case RoundStyle.Bottom:
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        0,
                        90);
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
                    break;
            }
            path.CloseFigure();

            return path;
        }




        /// <summary>
        /// 建立带有圆角样式的路径。取横向一半顶部（用于按钮玻璃效果）
        /// </summary>
        /// <param name="rect">用来建立路径的矩形。</param>
        /// <param name="_radius">圆角的大小。</param>
        /// <param name="style">圆角的样式。</param>
        /// <param name="correction">是否把矩形长宽减 1,以便画出边框。</param>
        /// <returns>建立的路径。</returns>
        public static GraphicsPath CreatePathHalf(
            Rectangle rect, int radius, RoundStyle style, bool correction)
        {
            GraphicsPath path = new GraphicsPath();
            int radiusCorrection = correction ? 1 : 0;
            switch (style)
            {
                case RoundStyle.None:
                    path.AddRectangle(new Rectangle(rect.X, rect.Y, rect.Width - radiusCorrection, rect.Height/2 - radiusCorrection));
                    break;
                case RoundStyle.All:
                    //顺时针
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);

                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddLine(rect.Right - radiusCorrection,
                        rect.Y + rect.Height / 2 - radiusCorrection,
                        rect.X,
                        rect.Y + rect.Height / 2 - radiusCorrection);

                   
                    break;
                case RoundStyle.Left:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);

                    path.AddLine(
                        rect.Right - radiusCorrection, rect.Y,
                        rect.Right - radiusCorrection, rect.Y + rect.Height / 2 - radiusCorrection);
                    path.AddLine(                        
                        rect.Right - radiusCorrection, rect.Y + rect.Height / 2 - radiusCorrection,
                        rect.X, rect.Y + rect.Height / 2 - radiusCorrection);
                    
                    break;
                case RoundStyle.Right:
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddLine(rect.Right - radiusCorrection, rect.Y + rect.Height / 2 - radiusCorrection,
                        rect.X, rect.Y + rect.Height / 2 - radiusCorrection);
                    path.AddLine(rect.X, rect.Y + rect.Height / 2 - radiusCorrection, 
                        rect.X, rect.Y);
                    break;
                case RoundStyle.Top:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddLine(
                        rect.Right - radiusCorrection, rect.Y + rect.Height / 2 - radiusCorrection,
                        rect.X, rect.Y + rect.Height / 2 - radiusCorrection);
                    break;
                case RoundStyle.Bottom:
                    path.AddRectangle(new Rectangle(rect.X, rect.Y, rect.Width - radiusCorrection, rect.Height / 2 - radiusCorrection));

                    break;
            }
            path.CloseFigure();

            return path;
        }
    }
}
