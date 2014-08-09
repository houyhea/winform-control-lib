using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace WSN.Ants.Controls.WSNToolStrip
{
    /// <summary>
    /// 状态栏效果的toolstrip Render
    /// </summary>
    public class WSNStatusToolStipRender : WSNToolStripRender
    {
        //protected override void OnRenderToolStripBackground(
        //    ToolStripRenderEventArgs e)
        //{
        //    ToolStrip toolStrip = e.ToolStrip;
        //    Graphics g = e.Graphics;
        //    Rectangle bounds = e.AffectedBounds;

        //    if (toolStrip is ToolStripDropDown)
        //    {
        //        CreateRegion(e.ToolStrip, e.AffectedBounds);

        //        Rectangle rect = e.AffectedBounds;

        //        using (GraphicsPath path = CreatePath(
        //            rect, 3))
        //        {
        //            if (e.AffectedBounds.Height > 0 && e.AffectedBounds.Width > 0)
        //            {
        //                using (LinearGradientBrush brush = new LinearGradientBrush(e.AffectedBounds, Color.FromArgb(216, 216, 216), Color.FromArgb(255, 255, 255), -90))
        //                {
        //                    //g.FillPath(brush, path);
        //                    g.FillRectangle(brush, e.AffectedBounds);
        //                }
        //            }
        //            using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
        //            {
        //                //g.DrawPath(pen, path);

        //                g.DrawRectangle(pen, e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);


        //            }


        //        }
        //    }
        //    else
        //    {
        //        if (bounds.Height > 0 && bounds.Width > 0)
        //        {
        //            //画背景
        //            using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(32, 46, 64), Color.FromArgb(65, 87, 117), 90))
        //            {
        //                Color[] colrs = new Color[3];
        //                colrs[0] = Color.FromArgb(65, 87, 117);
        //                colrs[1] = Color.FromArgb(32, 46, 64);
        //                colrs[2] = Color.FromArgb(32, 46, 64);
        //                ColorBlend blend = new ColorBlend();
        //                blend.Positions = new float[] { 0f, .5f, 1f };
        //                blend.Colors = colrs;
        //                br.InterpolationColors = blend;

        //                g.FillRectangle(br, bounds);
        //            }
        //        }
        //        //画顶部线
        //        using (Pen pen = new Pen(Color.FromArgb(10, 22, 38)))
        //        {
        //            g.DrawLine(pen, new Point(bounds.X, bounds.Y), new Point(bounds.Right, bounds.Y));

        //        }
        //        using (Pen pen = new Pen(Color.FromArgb(104, 122, 146)))
        //        {
        //            g.DrawLine(pen, new Point(bounds.X, bounds.Y + 1), new Point(bounds.Right, bounds.Y + 1));

        //        }

        //    }
        //}

        protected override void DrawToolStripBackground(Graphics g, Rectangle bounds)
        {
            //base.DrawToolStripBackground(g, bounds);
            if (bounds.Height > 0 && bounds.Width > 0)
            {
                //画背景
                using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(32, 46, 64), Color.FromArgb(65, 87, 117), 90))
                {
                    Color[] colrs = new Color[3];
                    colrs[0] = Color.FromArgb(65, 87, 117);
                    colrs[1] = Color.FromArgb(32, 46, 64);
                    colrs[2] = Color.FromArgb(32, 46, 64);
                    ColorBlend blend = new ColorBlend();
                    blend.Positions = new float[] { 0f, .5f, 1f };
                    blend.Colors = colrs;
                    br.InterpolationColors = blend;

                    g.FillRectangle(br, bounds);
                }
            }
            //画顶部线
            using (Pen pen = new Pen(Color.FromArgb(10, 22, 38)))
            {
                g.DrawLine(pen, new Point(bounds.X, bounds.Y), new Point(bounds.Right, bounds.Y));

            }
            using (Pen pen = new Pen(Color.FromArgb(104, 122, 146)))
            {
                g.DrawLine(pen, new Point(bounds.X, bounds.Y + 1), new Point(bounds.Right, bounds.Y + 1));
            }
        }

//        /// <summary>
//        /// 画分割线，包括dropdown
//        /// </summary>
//        /// <param name="e"></param>
//        protected override void OnRenderSeparator(
//           ToolStripSeparatorRenderEventArgs e)
//        {

//            ToolStrip toolStrip = e.ToolStrip;
//            Rectangle rect = e.Item.ContentRectangle;
//            Graphics g = e.Graphics;

//            Color baseColor = Color.FromArgb(255, 255, 255);
//            RenderSeparatorLine(
//               g,
//               rect,
//               baseColor,
//                //               ColorTable.BackNormal,
//Color.Red,
//               Color.Snow,
//               e.Vertical);
//        }

        //internal void RenderSeparatorLine(
        //   Graphics g,
        //   Rectangle rect,
        //   Color baseColor,
        //   Color backColor,
        //   Color shadowColor,
        //   bool vertical)
        //{

        //    if (vertical)
        //    {
        //        //画工具栏上的分割线

        //        Color Color1 = Color.FromArgb(0, 29, 35, 44);
        //        Color Color2 = Color.FromArgb(29, 35, 44);
        //        Color Color3 = Color.FromArgb(0, 29, 35, 44);

        //        if (rect.Width > 0 && rect.Height > 0)
        //        {

        //            using (LinearGradientBrush brush = new LinearGradientBrush(
        //                    rect,
        //                    Color1,
        //                    Color3,
        //                    90))
        //            {
        //                ColorBlend blend = new ColorBlend();

        //                Color[] colors = new Color[3]{
        //            Color1,Color2,Color3
        //            };
        //                blend.Colors = colors;
        //                blend.Positions = new float[] { 0f, .5f, 1f };
        //                brush.InterpolationColors = blend;

        //                using (Pen pen = new Pen(brush))
        //                {
        //                    g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);

        //                }
        //                colors[0] = Color.FromArgb(0, 89, 116, 145);
        //                colors[1] = Color.FromArgb(89, 116, 145);
        //                colors[2] = Color.FromArgb(0, 89, 116, 145);
        //                blend.Colors = colors;
        //                brush.InterpolationColors = blend;
        //                using (Pen pen = new Pen(brush))
        //                {
        //                    g.DrawLine(pen, rect.X + 1, rect.Y, rect.X + 1, rect.Bottom);
        //                }
        //            }
        //        }

        //    }
        //    else
        //    {
        //        //画弹出菜单的分割线

        //        using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
        //        {
        //            g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Top);
        //        }
        //        using (Pen pen = new Pen(Color.FromArgb(255, 255, 255)))
        //        {
        //            g.DrawLine(pen, rect.X, rect.Y + 1, rect.Right, rect.Top + 1);
        //        }

        //    }
        //}

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripDropDownButton item = e.Item as ToolStripDropDownButton;

            if (item != null)
            {
                Graphics g = e.Graphics;
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);

                if (item.Pressed)
                {
                    GraphicsPath path = CreatePath(bounds, 2);

                    if (bounds.Height > 0 && bounds.Width > 0)
                    {
                        //画背景
                        using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(23, 37, 60), Color.FromArgb(66, 103, 148), 90))
                        {
                            //Color[] colrs = new Color[3];
                            //colrs[0] = Color.FromArgb(19, 37, 56);
                            //colrs[1] = Color.FromArgb(225, 225, 225);
                            //colrs[2] = Color.FromArgb(73, 105, 144);
                            //ColorBlend blend = new ColorBlend();
                            //blend.Positions = new float[] { 0f, .5f, 1f };
                            //blend.Colors = colrs;
                            //br.InterpolationColors = blend;

                            g.FillPath(br, path);
                        }
                    }
                    GraphicsPath pathLine = CreatePath(new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), 2);
                    using (Pen pen = new Pen(Color.FromArgb(91, 117, 152)))
                    {
                        g.DrawPath(pen, pathLine);
                    }

                    return;
                }
                else if (item.Selected)
                {
                    GraphicsPath path = CreatePath(bounds, 2);

                    if (bounds.Width > 0 && bounds.Height > 0)
                    {
                        //画背景
                        using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(32, 46, 64), Color.FromArgb(102, 113, 127), 90))
                        {
                            Color[] colrs = new Color[3];
                            //colrs[0] = Color.FromArgb(65, 87, 117);
                            colrs[0] = Color.FromArgb(102, 113, 127);
                            colrs[1] = Color.FromArgb(32, 46, 64);
                            colrs[2] = Color.FromArgb(32, 46, 64);
                            ColorBlend blend = new ColorBlend();
                            blend.Positions = new float[] { 0f, .5f, 1f };
                            blend.Colors = colrs;
                            br.InterpolationColors = blend;

                            g.FillPath(br, path);
                        }
                    }
                    GraphicsPath pathLine = CreatePath(new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), 2);
                    using (Pen pen = new Pen(Color.FromArgb(150, 158, 168)))
                    {
                        g.DrawPath(pen, pathLine);
                    }
                    return;
                }
                else 
                {
                    if (item.Tag != null)
                    {
                        bool bchk = (bool)item.Tag;
                        if (bchk)
                        {
                            GraphicsPath path = CreatePath(bounds, 2);

                            if (bounds.Width > 0 && bounds.Height > 0)
                            {
                                ////画背景
                                ////using (LinearGradientBrush br = new LinearGradientBrush(bounds,  Color.FromArgb(255,0,0), Color.FromArgb(143,12,0),90))
                                //using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(236, 138, 17), Color.FromArgb(225,123,12), 90))
                                //{
                                //    g.FillPath(br, path);
                                //}

                                //画背景
                                using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(124, 38, 0), Color.FromArgb(206, 127 ,74), 90))
                                {
                                    Color[] colrs = new Color[3];
                                    //colrs[0] = Color.FromArgb(65, 87, 117);
                                    colrs[0] = Color.FromArgb(206, 127, 74);
                                    colrs[1] = Color.FromArgb(180, 78, 13);
                                    colrs[2] = Color.FromArgb(124, 38, 0);
                                    ColorBlend blend = new ColorBlend();
                                    blend.Positions = new float[] { 0f, .5f, 1f };
                                    blend.Colors = colrs;
                                    br.InterpolationColors = blend;

                                    g.FillPath(br, path);
                                }
                            }
                            GraphicsPath pathLine = CreatePath(new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), 2);
                            using (Pen pen = new Pen(Color.FromArgb(163 ,103, 77)))
                            {
                                g.DrawPath(pen, pathLine);
                            }
                        }
                    }
                }

                //base.DrawArrow(
                //   new ToolStripArrowRenderEventArgs(
                //   g,
                //   item,
                //   item.Bounds,
                //   Color.White,
                //   ArrowDirection.Down));
                return;
            }

            base.OnRenderDropDownButtonBackground(e);


        }

        protected override void OnRenderSplitButtonBackground(
    ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripSplitButton item = e.Item as ToolStripSplitButton;

            if (item != null)
            {
                Graphics g = e.Graphics;
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);



                if (item.Pressed || item.Selected || item.ButtonPressed || item.DropDownButtonSelected)
                {

                    GraphicsPath path = CreatePath(bounds, 2);

                    if (bounds.Width > 0 && bounds.Height > 0)
                    {
                        //画背景
                        using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(32, 46, 64), Color.FromArgb(102, 113, 127), 90))
                        {
                            Color[] colrs = new Color[3];
                            //colrs[0] = Color.FromArgb(65, 87, 117);
                            colrs[0] = Color.FromArgb(102, 113, 127);
                            colrs[1] = Color.FromArgb(32, 46, 64);
                            colrs[2] = Color.FromArgb(32, 46, 64);
                            ColorBlend blend = new ColorBlend();
                            blend.Positions = new float[] { 0f, .5f, 1f };
                            blend.Colors = colrs;
                            br.InterpolationColors = blend;

                            g.FillPath(br, path);
                        }
                    }
                    GraphicsPath pathLine = CreatePath(new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), 2);
                    using (Pen pen = new Pen(Color.FromArgb(150, 158, 168)))
                    {
                        //g.DrawRectangle(pen, bounds.X,bounds.Y,bounds.Width-1,bounds.Height-1);
                        g.DrawPath(pen, pathLine);
                        //g.DrawLine(
                        //   pen,
                        //   item.SplitterBounds.Left,
                        //   item.SplitterBounds.Top,
                        //   item.SplitterBounds.Left,
                        //   item.SplitterBounds.Bottom);
                    }

                    DrawSplitButtonSeperatorLine(g, item, item.SplitterBounds);

                    base.DrawArrow(
                        new ToolStripArrowRenderEventArgs(
                        g,
                        item,
                        item.DropDownButtonBounds,
                        Color.White,
                        ArrowDirection.Down));
                    return;
                }

                base.DrawArrow(
                   new ToolStripArrowRenderEventArgs(
                   g,
                   item,
                   item.DropDownButtonBounds,
                   Color.White,
                   ArrowDirection.Down));
                return;
            }

            base.OnRenderSplitButtonBackground(e);
        }

        //protected override void OnRenderMenuItemBackground(
        //  ToolStripItemRenderEventArgs e)
        //{
        //    ToolStrip toolStrip = e.ToolStrip;
        //    ToolStripItem item = e.Item;

        //    if (!item.Enabled)
        //    {
        //        return;
        //    }

        //    Graphics g = e.Graphics;
        //    Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

        //    if (toolStrip is ToolStripDropDown)
        //    {

        //        g.SmoothingMode = SmoothingMode.AntiAlias;


        //        if (e.Item.Selected)
        //        {
        //            using (SolidBrush br = new SolidBrush(Color.FromArgb(27, 40, 55)))
        //            {
        //                g.FillRectangle(br, new Rectangle(Point.Empty, e.Item.Size));
        //            }

        //        }
        //    }
        //    else
        //    {
        //        base.OnRenderMenuItemBackground(e);
        //    }
        //}

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderButtonBackground(e);

            ToolStrip toolStrip = e.ToolStrip;
            ToolStripButton item = e.Item as ToolStripButton;
            Graphics g = e.Graphics;

            if (item != null)
            {
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);

                if (item.Pressed)
                {
                    GraphicsPath path = CreatePath(bounds, 2);

                    if (bounds.Height > 0 && bounds.Width > 0)
                    {
                        ////画背景
                        //using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(237, 242, 248), Color.FromArgb(245, 247, 246), 90))
                        //{
                        //    Color[] colrs = new Color[3];
                        //    colrs[0] = Color.FromArgb(245, 247, 246);
                        //    colrs[1] = Color.FromArgb(225, 225, 225);
                        //    colrs[2] = Color.FromArgb(237, 242, 248);
                        //    ColorBlend blend = new ColorBlend();
                        //    blend.Positions = new float[] { 0f, .5f, 1f };
                        //    blend.Colors = colrs;
                        //    br.InterpolationColors = blend;

                        //    g.FillPath(br, path);
                        //}

                        //画背景
                        using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(23, 37, 60), Color.FromArgb(66, 103, 148), 90))
                        {
                            g.FillPath(br, path);
                        }
                    }
                    GraphicsPath pathLine = CreatePath(new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), 2);
                    using (Pen pen = new Pen(Color.FromArgb(91, 117, 152)))
                    {
                        g.DrawPath(pen, pathLine);
                    }

                    return;
                }
                else if (item.Checked)
                {
                    GraphicsPath path = CreatePath(bounds, 2);

                    if (bounds.Width > 0 && bounds.Height > 0)
                    {
                        //画背景
                        using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(255, 122, 122), Color.FromArgb(255, 0, 0), 90))
                        {
                            g.FillPath(br, path);
                        }

                        //画背景
                        //using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(124, 38, 0), Color.FromArgb(206, 127, 74), 90))
                        //{
                        //    Color[] colrs = new Color[3];
                        //    //colrs[0] = Color.FromArgb(65, 87, 117);
                        //    colrs[0] = Color.FromArgb(206, 127, 74);
                        //    colrs[1] = Color.FromArgb(180, 78, 13);
                        //    colrs[2] = Color.FromArgb(124, 38, 0);
                        //    ColorBlend blend = new ColorBlend();
                        //    blend.Positions = new float[] { 0f, .5f, 1f };
                        //    blend.Colors = colrs;
                        //    br.InterpolationColors = blend;

                        //    g.FillPath(br, path);
                        //}
                    }
                    GraphicsPath pathLine = CreatePath(new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), 2);
                    using (Pen pen = new Pen(Color.FromArgb(255, 106, 106)))
                    {
                        g.DrawPath(pen, pathLine);
                    }
                } 
                else if (item.Selected)
                {
                    GraphicsPath path = CreatePath(bounds, 2);

                    if (bounds.Height > 0 && bounds.Width > 0)
                    {
                        //画背景
                        using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(32, 46, 64), Color.FromArgb(102, 113, 127), 90))
                        {
                            Color[] colrs = new Color[3];
                            colrs[0] = Color.FromArgb(102, 113, 127);
                            colrs[1] = Color.FromArgb(32, 46, 64);
                            colrs[2] = Color.FromArgb(32, 46, 64);
                            ColorBlend blend = new ColorBlend();
                            blend.Positions = new float[] { 0f, .5f, 1f };
                            blend.Colors = colrs;
                            br.InterpolationColors = blend;

                            g.FillPath(br, path);
                        }
                    }
                    GraphicsPath pathLine = CreatePath(new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), 2);
                    using (Pen pen = new Pen(Color.FromArgb(150, 158, 168)))
                    {
                        g.DrawPath(pen, pathLine);
                    }

                    return;
                }
            }


        }

        #region 私有方法
        //private void DrawSplitButtonSeperatorLine(Graphics g, ToolStripSplitButton item, Rectangle rect)
        //{
        //    Color Color1 = Color.FromArgb(0, 0, 0, 0);
        //    Color Color2 = Color.FromArgb(0, 0, 0);
        //    Color Color3 = Color.FromArgb(0, 0, 0, 0);

        //    if (rect.Width > 0 && rect.Height > 0)
        //    {

        //        using (LinearGradientBrush brush = new LinearGradientBrush(
        //                rect,
        //                Color1,
        //                Color3,
        //                90))
        //        {
        //            ColorBlend blend = new ColorBlend();

        //            Color[] colors = new Color[3]{
        //            Color1,Color2,Color3
        //            };
        //            blend.Colors = colors;
        //            blend.Positions = new float[] { 0f, .5f, 1f };
        //            brush.InterpolationColors = blend;

        //            using (Pen pen = new Pen(brush))
        //            {
        //                g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);

        //            }
        //            colors[0] = Color.FromArgb(0, 255, 255, 255);
        //            colors[1] = Color.FromArgb(255, 255, 255);
        //            colors[2] = Color.FromArgb(0, 255, 255, 255);
        //            blend.Colors = colors;
        //            brush.InterpolationColors = blend;
        //            using (Pen pen = new Pen(brush))
        //            {
        //                g.DrawLine(pen, rect.X + 1, rect.Y, rect.X + 1, rect.Bottom);
        //            }
        //        }
        //    }
        //}

        //private void CreateRegion(Control control,
        //    Rectangle bounds)
        //{
        //    CreateRegion(control, bounds, 3);
        //}
        //public void CreateRegion(
        //    Control control,
        //    Rectangle bounds,
        //    int radius)
        //{
        //    using (GraphicsPath path =
        //        CreatePath(
        //        bounds, radius))
        //    {
        //        Region region = new Region(path);
        //        path.Widen(Pens.White);
        //        region.Union(path);
        //        if (control.Region != null)
        //        {
        //            control.Region.Dispose();
        //        }
        //        control.Region = region;
        //    }
        //}
        ///// <summary>
        ///// 建立带有圆角样式的路径。
        ///// </summary>
        ///// <param name="rect">用来建立路径的矩形。</param>
        ///// <param name="_radius">圆角的大小。</param>
        ///// <returns>建立的路径。</returns>
        //public GraphicsPath CreatePath(
        //    Rectangle rect, int radius)
        //{
        //    GraphicsPath path = new GraphicsPath();

        //    //radius = 3;
        //    if (radius <= 0)
        //    {

        //        path.AddRectangle(rect);
        //    }
        //    else
        //    {

        //        path.AddLine(new Point(rect.X, rect.Y + radius), new Point(rect.X + radius, rect.Y));
        //        path.AddLine(new Point(rect.X + radius, rect.Y), new Point(rect.Right - radius, rect.Y));
        //        path.AddLine(new Point(rect.Right - radius, rect.Y), new Point(rect.Right, rect.Y + radius));
        //        path.AddLine(new Point(rect.Right, rect.Y + radius), new Point(rect.Right, rect.Bottom - radius));
        //        path.AddLine(new Point(rect.Right, rect.Bottom - radius), new Point(rect.Right - radius, rect.Bottom));
        //        path.AddLine(new Point(rect.Right - radius, rect.Bottom), new Point(rect.X + radius, rect.Bottom));
        //        path.AddLine(new Point(rect.X + radius, rect.Bottom), new Point(rect.X, rect.Bottom - radius));
        //        path.AddLine(new Point(rect.X, rect.Bottom - radius), new Point(rect.X, rect.Y + radius));


        //        //以下是圆弧的圆角
        //        //path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
        //        //path.AddArc(
        //        //    rect.Right - radius - radiusCorrection,
        //        //    rect.Y,
        //        //    radius,
        //        //    radius,
        //        //    270,
        //        //    90);
        //        //path.AddArc(
        //        //    rect.Right - radius ,
        //        //    rect.Bottom - radius ,
        //        //    radius,
        //        //    radius, 0, 90);
        //        //path.AddArc(
        //        //    rect.X,
        //        //    rect.Bottom - radius ,
        //        //    radius,
        //        //    radius,
        //        //    90,
        //        //    90);

        //    }
        //    path.CloseFigure();

        //    return path;
        //}
        #endregion

    }
}
