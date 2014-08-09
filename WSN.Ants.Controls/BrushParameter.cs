using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using WSN.Ants.Controls.TypeConverters;

namespace WSN.Ants.Controls
{
    /// <summary>
    /// 画刷参数
    /// </summary>
    [TypeConverter(typeof(BrushParameterConverter))]
    public class BrushParameter
    {
        Color _color1;
        Color _color2;
        LinearGradientMode _mode ;

        /// <summary>
        /// 画刷渐变样式
        /// </summary>
        [Description("画刷渐变样式")]
        public LinearGradientMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        /// <summary>
        /// 画刷渐变颜色2
        /// </summary>
        [Description("画刷渐变颜色2")]
        public Color Color2
        {
            get { return _color2; }
            set { _color2 = value; }
        }
        /// <summary>
        /// 画刷渐变颜色1
        /// </summary>
        [Description("画刷渐变颜色1")]
        public Color Color1
        {
            get { return _color1; }
            set { _color1 = value; }
        }

        #region 构造函数
        public BrushParameter(Color color1, Color color2, LinearGradientMode linarMode)
        {
            _color1 = color1;
            _color2 = color2;
            _mode = linarMode;
        }
        public BrushParameter()
        {
            _color1 = Color.Black;
            _color2 = Color.Empty;
            _mode = LinearGradientMode.Vertical;

        }
        #endregion
    }
}
