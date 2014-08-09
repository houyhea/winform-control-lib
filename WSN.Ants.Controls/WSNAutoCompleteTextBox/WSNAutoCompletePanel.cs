using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace WSN.Ants.Controls.WSNAutoCompleteTextBox
{
    public partial class WSNAutoCompleteTextBox : WSNTextBox.WSNTextBox
    {
        /// <summary>
        /// WSN控件： 
        ///     WSNAutoCompletePanel
        /// </summary>
        public class WSNAutoCompletePanel : Panel
        {
            /// <summary>
            /// 控件属性：
            ///     获取或设置边框颜色
            /// </summary>
            public Color BorderColor { get; set; }

            /// <summary>
            /// 新建控件 <c>WSNAutoCompletePanel</c>
            /// </summary>
            public WSNAutoCompletePanel()
            {
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                BackColor = Color.Transparent;

                BorderColor = Color.FromArgb(179,179,179);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;


                using (SolidBrush br = new SolidBrush(Color.White))
                {
                    g.FillRectangle(br, ClientRectangle);
                }




                #region 画border
                Rectangle r = ClientRectangle;
                r.Width -= 1;
                r.Height -= 1;

                using (Pen p = new Pen(BorderColor))
                {

                    g.DrawRectangle(p, r);
                }


                #endregion

            }
        }

    }
}
