using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WSN.Ants.Controls.WSNToolStrip
{
    /// <summary>
    /// WSN工具栏渲染器类
    /// </summary>
    public abstract class WSNToolStripRender : ToolStripRenderer
    {
        protected Color normalColor = Color.White;
        protected Color selectedColor = Color.FromArgb(255, 211, 115);
        protected Color PressedColor = Color.Orange;

        //protected Color _BackColor1 = Color.FromArgb(232, 236, 239);
        //protected Color _BackColor2 = Color.FromArgb(207, 216, 225);
        protected Color _BackColor1 = Color.FromArgb(241, 241, 241);//弹出菜单栏背景色
        protected Color _BackColor2 = Color.FromArgb(241, 241, 241);
        //protected Color _ImageSideBarBackColor1 = Color.FromArgb(232, 236, 239);
        //protected Color _ImageSideBarBackColor2 = Color.FromArgb(232, 236, 239);
        protected Color _ImageSideBarBackColor1 = Color.FromArgb(228, 228, 228);//弹出菜单栏图片区域背景色
        protected Color _ImageSideBarBackColor2 = Color.FromArgb(228, 228, 228);

        protected Color _splitButtonSeperatorArrowColor = Color.White;//OnRenderSplitButtonBackground中箭头颜色

        protected int imageBlockWidth = 26;

        protected Color _splitButtonSperatorLineColor1 = Color.FromArgb(0,0,0,0);
        protected Color _splitButtonSperatorLineColor2 = Color.FromArgb(179,179,179);
        protected Color _splitButtonSperatorLineColor3 = Color.FromArgb(0,0,0,0);

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            //base.OnRenderToolStripBackground(e);
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown)
            {
                CreateRegion(e.ToolStrip, e.AffectedBounds);

                Rectangle rect = e.AffectedBounds;

                Rectangle imageRect = new Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, imageBlockWidth, e.AffectedBounds.Height);
                Rectangle contentRect = new Rectangle(e.AffectedBounds.X + imageBlockWidth, e.AffectedBounds.Y, e.AffectedBounds.Width - imageBlockWidth, e.AffectedBounds.Height);

                using (GraphicsPath imagePath = CreateLeftRadiusPath(imageRect, 3))
                {
                    if (e.AffectedBounds.Width > 00 && e.AffectedBounds.Height > 0)
                    {
                        using (LinearGradientBrush br = new LinearGradientBrush(imageRect,
                            this._ImageSideBarBackColor1, this._ImageSideBarBackColor2, LinearGradientMode.Vertical))
                        {
                            g.FillPath(br, imagePath);
                        }
                    }
                }

                using (GraphicsPath contentPath = CreateRightRadiusPath(contentRect, 3))
                {
                    if (e.AffectedBounds.Width > 00 && e.AffectedBounds.Height > 0)
                    {
                        using (LinearGradientBrush br = new LinearGradientBrush(contentRect, this._BackColor1, this._BackColor2, LinearGradientMode.Vertical))
                        {
                            g.FillPath(br, contentPath);
                        }
                    }
                }

                using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
                {
                    g.DrawRectangle(pen, e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);
                }
            }
            else
            {
                DrawToolStripBackground(g, bounds);
            }
        }

        /// <summary>
        /// 当工具栏不是ToolStripDropDown类型的时候，
        /// 画工具栏背景的方法，即整个工具条的方法
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds">待绘制区域边界的矩形</param>
        protected virtual void DrawToolStripBackground(Graphics g,Rectangle bounds)
        {
            if (bounds.Width > 0 && bounds.Height > 0)
            {
                //画背景
                using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(212, 212, 212), Color.FromArgb(255, 255, 255), -90))
                {
                    g.FillRectangle(br, bounds);
                }
            }

            using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
            {
                g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            }
        }

        /// <summary>
        /// 画分割线，包括dropdown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Rectangle rect = e.Item.ContentRectangle;
            Graphics g = e.Graphics;

            Color baseColor = Color.FromArgb(255, 255, 255);
            RenderSeparatorLine(g, rect, baseColor, Color.Red, Color.Snow, e.Vertical);
        }

        /// <summary>
        /// 绘制ToolStripMenuItem 的背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripItem item = e.Item;

            if (!item.Enabled)
            {
                return;
            }

            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

            if (toolStrip is ToolStripDropDown)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                //选中、移动上去时item的背景
                if (e.Item.Selected)
                {
                    Rectangle selectedRect = new Rectangle(2, 0, e.Item.Size.Width - 4, e.Item.Size.Height - 1);
                    using (GraphicsPath path = CreatePath(selectedRect, 2))
                    {
                        using (LinearGradientBrush br = new LinearGradientBrush(selectedRect,
                            Color.FromArgb(255, 251, 239), Color.FromArgb(255, 237, 181), LinearGradientMode.Vertical))
                        {
                            g.FillPath(br, path);
                            using (Pen pen = new Pen(Color.FromArgb(230, 195, 101)))
                            {
                                g.DrawPath(pen, path);
                            }
                        }
                    }
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Item.Selected && e.Item is ToolStripMenuItem)
                e.TextColor = Color.FromArgb(0, 0, 0);
            base.OnRenderItemText(e);
        }

        protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderOverflowButtonBackground(e);
            ToolStripItem item = e.Item;
            ToolStrip toolstrip = e.ToolStrip;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            bool rightToLeft = item.RightToLeft == RightToLeft.Yes;

            bool bHorizontal = toolstrip.Orientation == Orientation.Horizontal;
            Rectangle rect = Rectangle.Empty;

            if (rightToLeft)
            {
                rect = new Rectangle(0, item.Height - 8, 10, 5);
            }
            else
            {
                rect = new Rectangle(item.Width - 12, item.Height - 8, 10, 5);
            }

            ArrowDirection direction = bHorizontal ? ArrowDirection.Down : ArrowDirection.Right;
            int x = (rightToLeft && bHorizontal) ? -1 : 1;
            rect.Offset(x, 1);

            Color color;

            if (item.Pressed)
            {
                color = PressedColor;
            }
            else if (item.Selected)
            {
                color = selectedColor;
            }
            else
            {
                color = normalColor;
            }

            using (Brush brush = new SolidBrush(color))
            {
                RenderArrowInternal(g, rect, direction, brush);
            }

            if (bHorizontal)
            {
                using (Pen pen = new Pen(color))
                {
                    g.DrawLine(pen, rect.Right - 8, rect.Y - 2, rect.Right - 2, rect.Y - 2);
                    g.DrawLine(pen, rect.Right - 8, rect.Y - 1, rect.Right - 2, rect.Y - 1);
                }

                Rectangle rect2 = new Rectangle(rect.Right - 8, 2, 6, item.Height - 13);
                using (Brush bbrush = new SolidBrush(color))
                {
                    g.FillRectangle(bbrush, rect2);
                }
            }
            else
            {
                using (Pen pen = new Pen(normalColor))
                {
                    g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom - 1);
                    g.DrawLine(pen, rect.X, rect.Y + 1, rect.X, rect.Bottom);
                }
            }
        }

        internal void RenderArrowInternal(Graphics g, Rectangle dropDownRect, ArrowDirection direction, Brush brush)
        {
            Point point = new Point(
                dropDownRect.Left + (dropDownRect.Width / 2),
                dropDownRect.Top + (dropDownRect.Height / 2));
            Point[] points = null;
            switch (direction)
            {
                case ArrowDirection.Left:
                    points = new Point[] { 
                        new Point(point.X + 2, point.Y - 3), 
                        new Point(point.X + 2, point.Y + 3), 
                        new Point(point.X - 1, point.Y) };
                    break;

                case ArrowDirection.Up:
                    points = new Point[] { 
                        new Point(point.X - 3, point.Y + 2), 
                        new Point(point.X + 3, point.Y + 2), 
                        new Point(point.X, point.Y - 2) };
                    break;

                case ArrowDirection.Right:
                    points = new Point[] {
                        new Point(point.X - 2, point.Y - 3), 
                        new Point(point.X - 2, point.Y + 3), 
                        new Point(point.X + 1, point.Y) };
                    break;

                default:
                    points = new Point[] {
                        new Point(point.X - 3, point.Y - 1), 
                        new Point(point.X + 3, point.Y - 1), 
                        new Point(point.X, point.Y + 2) };
                    break;
            }
            g.FillPolygon(brush, points);
        }

        internal void RenderSeparatorLine(Graphics g, Rectangle rect, Color baseColor, Color backColor, Color shadowColor, bool vertical)
        {
            if (vertical)
            {
                //画工具栏上的分割线

                Color Color1 = Color.FromArgb(0, 179, 179, 179);
                Color Color2 = Color.FromArgb(179, 179, 179);
                Color Color3 = Color.FromArgb(0, 179, 179, 179);

                if (rect.Width > 0 && rect.Height > 0)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(rect,
                            Color1, Color3, 90))
                    {
                        ColorBlend blend = new ColorBlend();
                        Color[] colors = new Color[3]{
                    Color1,Color2,Color3
                    };
                        blend.Colors = colors;
                        blend.Positions = new float[] { 0f, .5f, 1f };
                        brush.InterpolationColors = blend;

                        using (Pen pen = new Pen(brush))
                        {
                            g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
                        }
                        colors[0] = Color.FromArgb(0, 255, 255, 255);
                        colors[1] = Color.FromArgb(255, 255, 255);
                        colors[2] = Color.FromArgb(0, 255, 255, 255);
                        blend.Colors = colors;
                        brush.InterpolationColors = blend;
                        using (Pen pen = new Pen(brush))
                        {
                            g.DrawLine(pen, rect.X + 1, rect.Y, rect.X + 1, rect.Bottom);
                        }
                    }
                }
            }
            else
            {
                //画弹出菜单的分割线

                using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
                {
                    g.DrawLine(pen, rect.X + this.imageBlockWidth, rect.Y, rect.Right - 2, rect.Top);
                }
                using (Pen pen = new Pen(Color.FromArgb(255, 255, 255)))
                {
                    g.DrawLine(pen, rect.X + this.imageBlockWidth, rect.Y + 1, rect.Right - 2, rect.Top + 1);
                }
            }
        }

        public void CreateRegion(
            Control control,
            Rectangle bounds,
            int radius)
        {
            using (GraphicsPath path =
                CreatePath(
                bounds, radius))
            {
                Region region = new Region(path);
                path.Widen(Pens.White);
                region.Union(path);
                if (control.Region != null)
                {
                    control.Region.Dispose();
                }
                control.Region = region;
            }
        }

        /// <summary>
        /// 建立带有圆角样式的路径。
        /// </summary>
        /// <param name="rect">用来建立路径的矩形。</param>
        /// <param name="_radius">圆角的大小。</param>
        /// <returns>建立的路径。</returns>
        public GraphicsPath CreatePath(
            Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            //radius = 3;
            if (radius <= 0)
            {
                path.AddRectangle(rect);
            }
            else
            {
                path.AddLine(new Point(rect.X, rect.Y + radius), new Point(rect.X + radius, rect.Y));
                path.AddLine(new Point(rect.X + radius, rect.Y), new Point(rect.Right - radius, rect.Y));
                path.AddLine(new Point(rect.Right - radius, rect.Y), new Point(rect.Right, rect.Y + radius));
                path.AddLine(new Point(rect.Right, rect.Y + radius), new Point(rect.Right, rect.Bottom - radius));
                path.AddLine(new Point(rect.Right, rect.Bottom - radius), new Point(rect.Right - radius, rect.Bottom));
                path.AddLine(new Point(rect.Right - radius, rect.Bottom), new Point(rect.X + radius, rect.Bottom));
                path.AddLine(new Point(rect.X + radius, rect.Bottom), new Point(rect.X, rect.Bottom - radius));
                path.AddLine(new Point(rect.X, rect.Bottom - radius), new Point(rect.X, rect.Y + radius));
            }
            path.CloseFigure();

            return path;
        }

        protected GraphicsPath CreateLeftRadiusPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(rect);
            }
            else
            {
                path.AddLine(new Point(rect.X, rect.Y + radius), new Point(rect.X + radius, rect.Y));
                path.AddLine(new Point(rect.X + radius, rect.Y), new Point(rect.X + rect.Width, rect.Y));
                path.AddLine(new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                path.AddLine(new Point(rect.X + rect.Width, rect.Y + rect.Height), new Point(rect.X + radius, rect.Y + rect.Height));
                path.AddLine(new Point(rect.X + radius, rect.Y + rect.Height), new Point(rect.X, rect.Y + rect.Height - radius));
                path.AddLine(new Point(rect.X, rect.Y + rect.Height - radius), new Point(rect.X, rect.Y + radius));
            }
            path.CloseFigure();

            return path;
        }

        protected GraphicsPath CreateRightRadiusPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(rect);
            }
            else
            {
                path.AddLine(new Point(rect.X, rect.Y), new Point(rect.X + rect.Width - radius, rect.Y));
                path.AddLine(new Point(rect.X + rect.Width - radius, rect.Y), new Point(rect.X + rect.Width, rect.Y + radius));
                path.AddLine(new Point(rect.X + rect.Width, rect.Y + radius), new Point(rect.X + rect.Width, rect.Y + rect.Height - radius));
                path.AddLine(new Point(rect.X + rect.Width, rect.Y + rect.Height - radius), new Point(rect.X + rect.Width - radius, rect.Y + rect.Height));
                path.AddLine(new Point(rect.X + rect.Width - radius, rect.Y + rect.Height), new Point(rect.X, rect.Y + rect.Height));
                path.AddLine(new Point(rect.X, rect.Y + rect.Height), new Point(rect.X, rect.Y));
            }
            path.CloseFigure();

            return path;
        }

        protected void CreateRegion(Control control, Rectangle bounds)
        {
            CreateRegion(control, bounds, 3);
        }

        protected void DrawSplitButtonSeperatorLine(Graphics g, ToolStripSplitButton item, Rectangle rect)
        {
            //Color Color1 = Color.FromArgb(0, 0, 0, 0);
            //Color Color2 = Color.FromArgb(0, 0, 0);
            //Color Color3 = Color.FromArgb(0, 0, 0, 0);

            if (rect.Width > 0 && rect.Height > 0)
            {

                using (LinearGradientBrush brush = new LinearGradientBrush(
                        rect,
                        _splitButtonSperatorLineColor1,
                        _splitButtonSperatorLineColor3,
                        90))
                {
                    ColorBlend blend = new ColorBlend();

                    Color[] colors = new Color[3]{
                    _splitButtonSperatorLineColor1,_splitButtonSperatorLineColor2,_splitButtonSperatorLineColor3
                    };
                    blend.Colors = colors;
                    blend.Positions = new float[] { 0f, .5f, 1f };
                    brush.InterpolationColors = blend;

                    using (Pen pen = new Pen(brush))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);

                    }
                    colors[0] = Color.FromArgb(0, 255, 255, 255);
                    colors[1] = Color.FromArgb(255, 255, 255);
                    colors[2] = Color.FromArgb(0, 255, 255, 255);
                    blend.Colors = colors;
                    brush.InterpolationColors = blend;
                    using (Pen pen = new Pen(brush))
                    {
                        g.DrawLine(pen, rect.X + 1, rect.Y, rect.X + 1, rect.Bottom);
                    }
                }
            }
        }
    }
}
