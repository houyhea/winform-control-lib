using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WSN.Ants.Controls.WSNTabControl
{
    /// <summary>
    /// WSNTabControl的渲染器类
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNTabControlRender
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNTabControlExamples.cs" region="WSNTabControlRenderExample"/>
    /// </example>
    public class WSNTabControlRender : IDisposable
    {
        #region  字段
        private WSNTabControl _owner;
        private ImageList leftRightImages = null;

        private Color _backColor = Color.FromArgb(32, 46, 62);
        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置当控件宽度不够现实tabpagg的时候，出现的左右箭头按钮图标
        /// </summary>
        public ImageList LeftRightImages
        {
            get
            {
                if (leftRightImages == null)
                {
                    leftRightImages = new ImageList();

                    Bitmap updownImage = new Bitmap(typeof(WSNTabControl), "TabIcons.bmp");//((System.Drawing.Bitmap)(resources.GetObject("TabIcons.bmp")));

                    if (updownImage != null)
                    {
                        updownImage.MakeTransparent(Color.White);
                        leftRightImages.Images.AddStrip(updownImage);
                    }

                }
                return leftRightImages;
            }
            set { leftRightImages = value; }
        }
        internal WSNTabControl Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        #endregion

        #region 构造函数
        public WSNTabControlRender(WSNTabControl tabControl)
        {
            if (tabControl == null)
            {
                throw new ArgumentNullException("WSNTabControl 对象为空！");
            }
            else
            {
                _owner = tabControl;

            }
            leftRightImages = new ImageList();

            Bitmap updownImage = new Bitmap(typeof(WSNTabControl), "TabIcons.bmp");//((System.Drawing.Bitmap)(resources.GetObject("TabIcons.bmp")));

            if (updownImage != null)
            {
                updownImage.MakeTransparent(Color.White);
                leftRightImages.Images.AddStrip(updownImage);
            }
        }
        #endregion

        #region 虚函数
        /// <summary>
        /// 渲染控件背景
        /// </summary>
        /// <param name="g">绘图画面</param>
        /// <param name="TabControlArea">绘制的区域</param>
        public virtual void RenderControlBackground(System.Drawing.Graphics g, System.Drawing.Rectangle TabControlArea)
        {
            Brush br = new SolidBrush(_backColor);

            g.FillRectangle(br, TabControlArea);
            br.Dispose();
        }
        /// <summary>
        /// 渲染控件边框
        /// </summary>
        /// <param name="g">绘图画面</param>
        /// <param name="TabArea">绘制的区域</param>
        public virtual void RenderTabPanelBorder(System.Drawing.Graphics g, System.Drawing.Rectangle TabArea)
        {
            Pen border1 = new Pen(Color.FromArgb(19, 28, 37));
            //Pen border1 = new Pen(Color.FromArgb(0,255,0));
            g.DrawRectangle(border1, TabArea);
            border1.Dispose();

            //内边框
            Pen border2 = new Pen(Color.FromArgb(91, 125, 170));
            //Pen border2 = new Pen(Color.FromArgb(255,0,0));
            TabArea.Inflate(-1, -1);
            g.DrawRectangle(border2, TabArea);
            //g.DrawRectangle(border2,tabArea.X+1,tabArea.Y+1,tabArea.Width-3,tabArea.Height-3);
            border2.Dispose();
        }
        /// <summary>
        /// 绘制覆盖边框的区域
        /// </summary>
        /// <param name="g">绘图画面</param>
        /// <param name="TabArea">绘制的区域</param>
        public virtual void DrawCoverBorderAreas(System.Drawing.Graphics g, System.Drawing.Rectangle TabArea)
        {

            //如果边框是两条线，则这里就只需要覆盖底线和右边的竖线，根据实际观察得知
            if (Owner.SelectedTab != null)
            {
                TabPage tabPage = Owner.SelectedTab;
                Color color = tabPage.BackColor;
                Pen border = new Pen(color);

                g.DrawLine(border, new Point(TabArea.X + 2, TabArea.Bottom - 2)
                    , new Point(TabArea.Right - 2, TabArea.Bottom - 2));

                g.DrawLine(border, new Point(TabArea.Right - 2, TabArea.Bottom - 2)
                , new Point(TabArea.Right - 2, TabArea.Y + 2));

                border.Dispose();
            }


        }
        /// <summary>
        /// 绘制子页面
        /// </summary>
        /// <param name="g">绘图画面</param>
        /// <param name="tabPage">tab页面</param>
        /// <param name="nIndex">页面索引</param>
        /// <param name="bHovered">是否hover状态 如果不是选中，就是hover状态</param>
        public virtual void DrawTab(System.Drawing.Graphics g, System.Windows.Forms.TabPage tabPage, int nIndex, bool bHovered)
        {
            Rectangle recBounds = _owner.GetTabRect(nIndex);
            RectangleF tabTextArea = (RectangleF)_owner.GetTabRect(nIndex);

            bool bSelected = (_owner.SelectedIndex == nIndex);



            if (bSelected)
            {
                Point[] pt = new Point[7];
                switch (_owner.Alignment)
                {
                    case TabAlignment.Top:
                        {

                            pt[0] = new Point(recBounds.Left, recBounds.Bottom);
                            pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                            pt[2] = new Point(recBounds.Left + 3, recBounds.Top);
                            pt[3] = new Point(recBounds.Right - 3, recBounds.Top);
                            pt[4] = new Point(recBounds.Right, recBounds.Top + 3);
                            pt[5] = new Point(recBounds.Right, recBounds.Bottom);
                            pt[6] = new Point(recBounds.Left, recBounds.Bottom);
                        }
                        break;
                    case TabAlignment.Bottom:
                        {
                            pt[0] = new Point(recBounds.Left, recBounds.Top);
                            pt[1] = new Point(recBounds.Right, recBounds.Top);
                            pt[2] = new Point(recBounds.Right, recBounds.Bottom - 3);
                            pt[3] = new Point(recBounds.Right - 3, recBounds.Bottom);
                            pt[4] = new Point(recBounds.Left + 3, recBounds.Bottom);
                            pt[5] = new Point(recBounds.Left, recBounds.Bottom - 3);
                            pt[6] = new Point(recBounds.Left, recBounds.Top);
                        }
                        break;
                    case TabAlignment.Left:
                        {
                            pt[0] = new Point(recBounds.Right, recBounds.Top);
                            pt[1] = new Point(recBounds.Right, recBounds.Bottom);
                            pt[2] = new Point(recBounds.Left + 3, recBounds.Bottom);
                            pt[3] = new Point(recBounds.Left, recBounds.Bottom - 3);
                            pt[4] = new Point(recBounds.Left, recBounds.Top + 3);
                            pt[5] = new Point(recBounds.Left + 3, recBounds.Top);
                            pt[6] = new Point(recBounds.Right, recBounds.Top);
                        }
                        break;
                    case TabAlignment.Right:
                        {
                            pt[0] = new Point(recBounds.Left, recBounds.Top);
                            pt[1] = new Point(recBounds.Right - 3, recBounds.Top);
                            pt[2] = new Point(recBounds.Right, recBounds.Top + 3);
                            pt[3] = new Point(recBounds.Right, recBounds.Bottom - 3);
                            pt[4] = new Point(recBounds.Right - 3, recBounds.Bottom);
                            pt[5] = new Point(recBounds.Left, recBounds.Bottom);
                            pt[6] = new Point(recBounds.Left, recBounds.Top);
                        }
                        break;

                }

                // fill this tab with background color
                Brush br = new SolidBrush(tabPage.BackColor);
                //Brush br = new SolidBrush(Color.White);
                g.FillPolygon(br, pt);
                br.Dispose();

                // draw border
                using (Pen pen1 = new Pen(Color.FromArgb(19, 28, 37)))
                {
                    g.DrawPolygon(pen1, pt);

                }

                #region 内边框

                switch (_owner.Alignment)
                {
                    case TabAlignment.Top:
                        {
                            pt[0] = new Point(recBounds.Left + 1, recBounds.Bottom);
                            pt[1] = new Point(recBounds.Left + 1, recBounds.Top + 3);
                            pt[2] = new Point(recBounds.Left + 3, recBounds.Top + 1);
                            pt[3] = new Point(recBounds.Right - 3, recBounds.Top + 1);
                            pt[4] = new Point(recBounds.Right - 1, recBounds.Top + 3);
                            pt[5] = new Point(recBounds.Right - 1, recBounds.Bottom);
                            pt[6] = new Point(recBounds.Left + 1, recBounds.Bottom);
                        }
                        break;
                    case TabAlignment.Bottom:
                        {
                            pt[0] = new Point(recBounds.Left + 1, recBounds.Top);
                            pt[1] = new Point(recBounds.Right - 1, recBounds.Top);
                            pt[2] = new Point(recBounds.Right - 1, recBounds.Bottom - 3);
                            pt[3] = new Point(recBounds.Right - 3, recBounds.Bottom - 1);
                            pt[4] = new Point(recBounds.Left + 3, recBounds.Bottom - 1);
                            pt[5] = new Point(recBounds.Left + 1, recBounds.Bottom - 3);
                            pt[6] = new Point(recBounds.Left + 1, recBounds.Top);
                        }
                        break;
                    case TabAlignment.Left:
                        {
                            pt[0] = new Point(recBounds.Right, recBounds.Top + 1);
                            pt[1] = new Point(recBounds.Right, recBounds.Bottom - 1);
                            pt[2] = new Point(recBounds.Left + 3, recBounds.Bottom - 1);
                            pt[3] = new Point(recBounds.Left + 1, recBounds.Bottom - 3);
                            pt[4] = new Point(recBounds.Left + 1, recBounds.Top + 3);
                            pt[5] = new Point(recBounds.Left + 3, recBounds.Top + 1);
                            pt[6] = new Point(recBounds.Right, recBounds.Top + 1);
                        }
                        break;
                    case TabAlignment.Right:
                        {
                            pt[0] = new Point(recBounds.Left, recBounds.Top + 1);
                            pt[1] = new Point(recBounds.Right - 3, recBounds.Top + 1);
                            pt[2] = new Point(recBounds.Right - 1, recBounds.Top + 3);
                            pt[3] = new Point(recBounds.Right - 1, recBounds.Bottom - 3);
                            pt[4] = new Point(recBounds.Right - 3, recBounds.Bottom - 1);
                            pt[5] = new Point(recBounds.Left, recBounds.Bottom - 1);
                            pt[6] = new Point(recBounds.Left, recBounds.Top + 1);
                        }
                        break;

                }

                //----------------------------
                // fill this tab with background color
                //Brush br1 = new SolidBrush(Color.FromArgb(119,140,166));
                using (Pen pen1 = new Pen(Color.FromArgb(91, 125, 170)))
                {
                    g.DrawPolygon(pen1, pt);

                }
                #endregion







                //----------------------------
                // clear bottom lines
                Pen pen = new Pen(tabPage.BackColor);

                switch (_owner.Alignment)
                {
                    case TabAlignment.Top:
                        g.DrawLine(pen, recBounds.Left + 2, recBounds.Bottom, recBounds.Right - 2, recBounds.Bottom);
                        g.DrawLine(pen, recBounds.Left + 2, recBounds.Bottom + 1, recBounds.Right - 2, recBounds.Bottom + 1);
                        break;

                    case TabAlignment.Bottom:
                        g.DrawLine(pen, recBounds.Left + 2, recBounds.Top, recBounds.Right - 2, recBounds.Top);
                        g.DrawLine(pen, recBounds.Left + 2, recBounds.Top - 1, recBounds.Right - 2, recBounds.Top - 1);
                        break;
                    case TabAlignment.Left:
                        g.DrawLine(pen, recBounds.Right + 1, recBounds.Top + 2, recBounds.Right + 1, recBounds.Bottom - 2);
                        g.DrawLine(pen, recBounds.Right, recBounds.Top + 2, recBounds.Right, recBounds.Bottom - 2);
                        break;
                    case TabAlignment.Right:
                        g.DrawLine(pen, recBounds.Left - 1, recBounds.Top + 2, recBounds.Left - 1, recBounds.Bottom - 2);
                        g.DrawLine(pen, recBounds.Left, recBounds.Top + 2, recBounds.Left, recBounds.Bottom - 2);
                        break;
                }

                pen.Dispose();
                //----------------------------
            }
            else if (bHovered)//如果不是选中，是hover状态
            {
                Color tabBackColor = Color.FromArgb(61, 84, 115);
                Color tabBorderColor = Color.FromArgb(19, 28, 37);
                Color tabBorderInnerColor = Color.FromArgb(89, 114, 145);

                Point[] pt = new Point[7];

                switch (_owner.Alignment)
                {
                    case TabAlignment.Top:
                        {
                            pt[0] = new Point(recBounds.Left, recBounds.Bottom);
                            pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                            pt[2] = new Point(recBounds.Left + 3, recBounds.Top);
                            pt[3] = new Point(recBounds.Right - 3, recBounds.Top);
                            pt[4] = new Point(recBounds.Right, recBounds.Top + 3);
                            pt[5] = new Point(recBounds.Right, recBounds.Bottom);
                            pt[6] = new Point(recBounds.Left, recBounds.Bottom);
                        }
                        break;

                    case TabAlignment.Bottom:
                        {
                            pt[0] = new Point(recBounds.Left, recBounds.Top);
                            pt[1] = new Point(recBounds.Right, recBounds.Top);
                            pt[2] = new Point(recBounds.Right, recBounds.Bottom - 3);
                            pt[3] = new Point(recBounds.Right - 3, recBounds.Bottom);
                            pt[4] = new Point(recBounds.Left + 3, recBounds.Bottom);
                            pt[5] = new Point(recBounds.Left, recBounds.Bottom - 3);
                            pt[6] = new Point(recBounds.Left, recBounds.Top);
                        }
                        break;
                    case TabAlignment.Left:
                        {
                            pt[0] = new Point(recBounds.Right, recBounds.Top);
                            pt[1] = new Point(recBounds.Right, recBounds.Bottom);
                            pt[2] = new Point(recBounds.Left + 3, recBounds.Bottom);
                            pt[3] = new Point(recBounds.Left, recBounds.Bottom - 3);
                            pt[4] = new Point(recBounds.Left, recBounds.Top + 3);
                            pt[5] = new Point(recBounds.Left + 3, recBounds.Top);
                            pt[6] = new Point(recBounds.Right, recBounds.Top);
                        }
                        break;
                    case TabAlignment.Right:
                        {
                            pt[0] = new Point(recBounds.Left, recBounds.Top);
                            pt[1] = new Point(recBounds.Right - 3, recBounds.Top);
                            pt[2] = new Point(recBounds.Right, recBounds.Top + 3);
                            pt[3] = new Point(recBounds.Right, recBounds.Bottom - 3);
                            pt[4] = new Point(recBounds.Right - 3, recBounds.Bottom);
                            pt[5] = new Point(recBounds.Left, recBounds.Bottom);
                            pt[6] = new Point(recBounds.Left, recBounds.Top);
                        }
                        break;
                }




                //----------------------------
                // fill this tab with background color
                Brush br = new SolidBrush(tabBackColor);
                g.FillPolygon(br, pt);
                br.Dispose();
                //----------------------------
                //----------------------------
                // draw border
                using (Pen pen1 = new Pen(tabBorderColor))
                {
                    g.DrawPolygon(pen1, pt);

                }

                #region 内边框

                switch (_owner.Alignment)
                {
                    case TabAlignment.Top:
                        {
                            pt[0] = new Point(recBounds.Left + 1, recBounds.Bottom);
                            pt[1] = new Point(recBounds.Left + 1, recBounds.Top + 3);
                            pt[2] = new Point(recBounds.Left + 3, recBounds.Top + 1);
                            pt[3] = new Point(recBounds.Right - 3, recBounds.Top + 1);
                            pt[4] = new Point(recBounds.Right - 1, recBounds.Top + 3);
                            pt[5] = new Point(recBounds.Right - 1, recBounds.Bottom);
                            pt[6] = new Point(recBounds.Left + 1, recBounds.Bottom);
                        }
                        break;

                    case TabAlignment.Bottom:
                        {
                            pt[0] = new Point(recBounds.Left + 1, recBounds.Top);
                            pt[1] = new Point(recBounds.Right - 1, recBounds.Top);
                            pt[2] = new Point(recBounds.Right - 1, recBounds.Bottom - 3);
                            pt[3] = new Point(recBounds.Right - 3, recBounds.Bottom - 1);
                            pt[4] = new Point(recBounds.Left + 3, recBounds.Bottom - 1);
                            pt[5] = new Point(recBounds.Left + 1, recBounds.Bottom - 3);
                            pt[6] = new Point(recBounds.Left + 1, recBounds.Top);
                        }
                        break;
                    case TabAlignment.Left:
                        {
                            pt[0] = new Point(recBounds.Right, recBounds.Top + 1);
                            pt[1] = new Point(recBounds.Right, recBounds.Bottom - 1);
                            pt[2] = new Point(recBounds.Left + 3, recBounds.Bottom - 1);
                            pt[3] = new Point(recBounds.Left + 1, recBounds.Bottom - 3);
                            pt[4] = new Point(recBounds.Left + 1, recBounds.Top + 3);
                            pt[5] = new Point(recBounds.Left + 3, recBounds.Top + 1);
                            pt[6] = new Point(recBounds.Right, recBounds.Top + 1);
                        }
                        break;
                    case TabAlignment.Right:
                        {
                            pt[0] = new Point(recBounds.Left, recBounds.Top + 1);
                            pt[1] = new Point(recBounds.Right - 3, recBounds.Top + 1);
                            pt[2] = new Point(recBounds.Right - 1, recBounds.Top + 3);
                            pt[3] = new Point(recBounds.Right - 1, recBounds.Bottom - 3);
                            pt[4] = new Point(recBounds.Right - 3, recBounds.Bottom - 1);
                            pt[5] = new Point(recBounds.Left, recBounds.Bottom - 1);
                            pt[6] = new Point(recBounds.Left, recBounds.Top + 1);
                        }
                        break;
                }



                //----------------------------
                // fill this tab with background color
                //Brush br1 = new SolidBrush(Color.FromArgb(119,140,166));
                using (Pen pen1 = new Pen(tabBorderInnerColor))
                {
                    g.DrawPolygon(pen1, pt);

                }
                #endregion







                //----------------------------
                // clear bottom lines
                Pen pen = new Pen(tabBorderColor);

                switch (_owner.Alignment)
                {
                    case TabAlignment.Top:
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom, recBounds.Right - 1, recBounds.Bottom);
                        //g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom - 2, recBounds.Right - 1, recBounds.Bottom - 2);
                        break;
                    case TabAlignment.Bottom:
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top, recBounds.Right - 1, recBounds.Top);
                        //g.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 1, recBounds.Right - 1, recBounds.Top - 1);
                        break;
                    case TabAlignment.Left:
                        g.DrawLine(pen, recBounds.Right, recBounds.Top + 1, recBounds.Right, recBounds.Bottom - 1);
                        //g.DrawLine(pen, recBounds.Right - 1, recBounds.Top + 1, recBounds.Right - 1, recBounds.Bottom - 1);
                        break;
                    case TabAlignment.Right:
                        g.DrawLine(pen, recBounds.Left, recBounds.Top + 1, recBounds.Left, recBounds.Bottom - 1);
                        //g.DrawLine(pen, recBounds.Left + 1, recBounds.Top + 1, recBounds.Left + 1, recBounds.Bottom - 1);
                        break;
                }

                pen.Dispose();
            }


            //----------------------------

            //----------------------------
            // draw tab's icon
            if ((tabPage.ImageIndex >= 0) && (_owner.ImageList != null) && (_owner.ImageList.Images[tabPage.ImageIndex] != null))
            {
                int nLeftMargin = 8;
                int nRightMargin = 2;

                Image img = _owner.ImageList.Images[tabPage.ImageIndex];

                Rectangle rimage = new Rectangle(recBounds.X + nLeftMargin, recBounds.Y + 1, img.Width, img.Height);

                // adjust rectangles
                float nAdj = (float)(nLeftMargin + img.Width + nRightMargin);

                rimage.Y += (recBounds.Height - img.Height) / 2;
                tabTextArea.X += nAdj;
                tabTextArea.Width -= nAdj;

                // draw icon
                g.DrawImage(img, rimage);
            }
            //----------------------------

            //----------------------------
            // draw string
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            if (bSelected)
            {
                using (SolidBrush br = new SolidBrush(Color.White))
                {
                    g.DrawString(tabPage.Text, _owner.Font, br, tabTextArea, stringFormat);
                }
            }
            else
            {
                using (SolidBrush br = new SolidBrush(Color.FromArgb(147, 170, 199)))
                {
                    g.DrawString(tabPage.Text, _owner.Font, br, tabTextArea, stringFormat);
                }

            }
            //----------------------------
        }
        #endregion




        #region IDisposable Members

        public void Dispose()
        {
            if (LeftRightImages != null)
                LeftRightImages.Dispose();
            Dispose();
        }

        #endregion

        internal void DrawUpDownButtonIcons(Graphics g, SubClass scUpDown)
        {

            if ((LeftRightImages == null) || (LeftRightImages.Images.Count != 4))
                return;

            //----------------------------
            // calc positions
            Rectangle TabControlArea = _owner.ClientRectangle;

            Rectangle r0 = new Rectangle();
            Win32.GetClientRect(scUpDown.Handle, ref r0);

            Brush br = new SolidBrush(SystemColors.Control);
            g.FillRectangle(br, r0);
            br.Dispose();

            Pen border = new Pen(SystemColors.ControlDark);
            Rectangle rborder = r0;
            rborder.Inflate(-1, -1);
            g.DrawRectangle(border, rborder);
            border.Dispose();

            int nMiddle = (r0.Width / 2);
            int nTop = (r0.Height - 16) / 2;
            int nLeft = (nMiddle - 16) / 2;

            Rectangle r1 = new Rectangle(nLeft, nTop, 16, 16);
            Rectangle r2 = new Rectangle(nMiddle + nLeft, nTop, 16, 16);
            //----------------------------

            //----------------------------
            // draw buttons
            Image img = LeftRightImages.Images[1];
            if (img != null)
            {
                if (_owner.TabCount > 0)
                {
                    Rectangle r3 = _owner.GetTabRect(0);
                    if (r3.Left < TabControlArea.Left)
                        g.DrawImage(img, r1);
                    else
                    {
                        img = LeftRightImages.Images[3];
                        if (img != null)
                            g.DrawImage(img, r1);
                    }
                }
            }

            img = LeftRightImages.Images[0];
            if (img != null)
            {
                if (_owner.TabCount > 0)
                {
                    Rectangle r3 = _owner.GetTabRect(_owner.TabCount - 1);
                    if (r3.Right > (TabControlArea.Width - r0.Width))
                        g.DrawImage(img, r2);
                    else
                    {
                        img = LeftRightImages.Images[2];
                        if (img != null)
                            g.DrawImage(img, r2);
                    }
                }
            }
            //----------------------------

        }
    }
}
