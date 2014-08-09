using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace WSN.Ants.Controls
{
    /// <summary>
    /// 平滑绘图图面类
    /// </summary>
    public class SmoothingModeGraphics : IDisposable
    {
        private SmoothingMode _oldMode;
        private Graphics _graphics;
        /// <summary>
        /// 构造函数：
        ///     创建<c>SmoothingModeGraphics</c>,默认平滑处理模式为消除锯齿的呈现
        /// </summary>
        /// <param name="graphics">绘图图面</param>
        public SmoothingModeGraphics(Graphics graphics)
            : this(graphics, SmoothingMode.AntiAlias)
        {
        }
        /// <summary>
        /// 构造函数：
        ///     创建<c>SmoothingModeGraphics</c>
        /// </summary>
        /// <param name="graphics">绘图图面</param>
        /// <param name="newMode">平滑处理模式</param>
        public SmoothingModeGraphics(Graphics graphics, SmoothingMode newMode)
        {
            _graphics = graphics;
            _oldMode = graphics.SmoothingMode;
            graphics.SmoothingMode = newMode;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _graphics.SmoothingMode = _oldMode;
        }

        #endregion
    }
}
