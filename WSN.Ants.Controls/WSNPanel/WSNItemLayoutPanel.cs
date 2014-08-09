using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Drawing;

namespace WSN.Ants.Controls.WSNPanel
{
    /// <summary>
    /// WSN控件：
    ///     WSNItemLayoutPanel
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNItemLayoutPanel
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNPanelExamples.cs" region="WSNItemLayoutPanelExample"/>
    /// </example>
    public class WSNItemLayoutPanel : Panel
    {
        private WSNItemLayoutEngine itemLayoutEnigne;

        /// <summary>
        /// 创建<c>WSNItemLayoutPanel</c>
        /// </summary>
        public WSNItemLayoutPanel()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// 控件属性：
        ///     获取布局引擎
        /// </summary>
        public override LayoutEngine LayoutEngine
        {
            get
            {
                if (itemLayoutEnigne == null)
                {
                    itemLayoutEnigne = new WSNItemLayoutEngine();
                }
                return itemLayoutEnigne;
            }
        }
    }
    /// <summary>
    /// 布局引擎<c>WSNItemLayoutEngine</c>
    /// </summary>
    public class WSNItemLayoutEngine : LayoutEngine
    {
        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="layoutEventArgs">Layout事件数据</param>
        /// <returns>是否成功 true-是 false-否</returns>
        public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
        {
            Control parent = container as Control;

            Rectangle parentClientRectangle = parent.ClientRectangle;
            Point nextControlLocation = parentClientRectangle.Location;
            int parentWidth = parentClientRectangle.Width;

            foreach (Control c in parent.Controls)
            {
                if (!c.Visible)
                {
                    continue;
                }

                nextControlLocation.Offset(c.Margin.Left, 0);

                c.Location = nextControlLocation;

                if (c.Location.X + c.Width * 2 > parentWidth)
                {
                    nextControlLocation.X = parentClientRectangle.X;
                    nextControlLocation.Y += c.Height;
                }
                else
                {
                    nextControlLocation.X += c.Width;
                }
            }

            return false;
        }
    }
}
