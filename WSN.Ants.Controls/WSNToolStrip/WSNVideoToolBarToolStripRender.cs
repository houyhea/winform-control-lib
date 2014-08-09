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
    /// WSN视频工具栏渲染器类
    /// </summary>
    public class WSNVideoToolBarToolStripRender : WSNToolStripRender
    {
        public WSNVideoToolBarToolStripRender()
        {
            this._splitButtonSperatorLineColor2 = Color.FromArgb(179, 179, 179); //重新设置SplitButtonSeperatorLine的中间颜色
            this._splitButtonSeperatorArrowColor = Color.FromArgb(100, 100, 100);
        }

        //private Color overflowBackgroundColor = Color.White;

  //      protected override void OnRenderToolStripBackground(
  //ToolStripRenderEventArgs e)
  //      {
  //          ToolStrip toolStrip = e.ToolStrip;
  //          Graphics g = e.Graphics;
  //          Rectangle bounds = e.AffectedBounds;

  //          if (toolStrip is ToolStripDropDown)
  //          {
  //              //画弹出菜单背景
  //              CreateRegion(e.ToolStrip, e.AffectedBounds);

  //              Rectangle rect = e.AffectedBounds;

  //              using (GraphicsPath path = CreatePath(
  //                  rect, 3))
  //              {
  //                  if (e.AffectedBounds.Height > 0 && e.AffectedBounds.Width > 0)
  //                  {
  //                      using (LinearGradientBrush brush = new LinearGradientBrush(e.AffectedBounds, Color.FromArgb(216, 216, 216), Color.FromArgb(255, 255, 255), -90))
  //                      {
  //                          //g.FillPath(brush, path);
  //                          g.FillRectangle(brush, e.AffectedBounds);
  //                      }
  //                  }
  //                  using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
  //                  {
  //                      //g.DrawPath(pen, path);

  //                      g.DrawRectangle(pen, e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);


  //                  }


  //              }
  //          }
  //          else
  //          {



  //              if (bounds.Width > 0 && bounds.Height > 0)
  //              {
  //                  //画背景
  //                  using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(123, 123, 123), Color.FromArgb(149, 149, 149), -90))
  //                  {
  //                      g.FillRectangle(br, bounds);
  //                  }
  //              }
  //              //using (SolidBrush br = new SolidBrush(Color.FromArgb(56, 56, 56)))
  //              //{
  //              //    g.FillRectangle(br, bounds);
  //              //}
  //              //using (Pen pen = new Pen(Color.FromArgb(68, 91, 120)))
  //              //{
  //              //    g.DrawRectangle(pen, bounds);
  //              //}

  //              //画底部线
  //              //using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
  //              //{
  //              //    g.DrawLine(pen, new Point(bounds.X, bounds.Bottom - 1), new Point(bounds.Right, bounds.Bottom - 1));

  //              //}
  //              //using (Pen pen = new Pen(Color.FromArgb(255, 255, 255)))
  //              //{
  //              //    g.DrawLine(pen, new Point(bounds.X, bounds.Bottom), new Point(bounds.Right, bounds.Bottom));

  //              //}

  //          }
  //      }

        protected override void DrawToolStripBackground(Graphics g, Rectangle bounds)
        {
            //base.DrawToolStripBackground(g, bounds);
            if (bounds.Width > 0 && bounds.Height > 0)
            {
                //画背景
                using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(123, 123, 123), Color.FromArgb(149, 149, 149), -90))
                {
                    g.FillRectangle(br, bounds);
                }
            }
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


                    //画背景                    
                    Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
                    if (rect.Width > 0 && rect.Height > 0)
                    {

                        using (LinearGradientBrush br = new LinearGradientBrush(rect, Color.FromArgb(225, 225, 225), Color.FromArgb(255, 255, 255), 90))
                        {
                            GraphicsPath path1 = CreatePath(rect, 2);
                            g.FillPath(br, path1);
                        }
                    }
                    using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
                    {
                        Rectangle r = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                        GraphicsPath path1 = CreatePath(r, 2);
                        g.DrawPath(pen, path1);
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
                        Color.FromArgb(100, 100, 100),
                        ArrowDirection.Down));
                    return;
                }
                base.DrawArrow(
                   new ToolStripArrowRenderEventArgs(
                   g,
                   item,
                   item.DropDownButtonBounds,
                   Color.FromArgb(100, 100, 100),
                   ArrowDirection.Down));
                return;
            }

            base.OnRenderSplitButtonBackground(e);
        }

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
                        using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(237, 242, 248), Color.FromArgb(245, 247, 246), 90))
                        {
                            Color[] colrs = new Color[3];
                            colrs[0] = Color.FromArgb(245, 247, 246);
                            colrs[1] = Color.FromArgb(225, 225, 225);
                            colrs[2] = Color.FromArgb(237, 242, 248);
                            ColorBlend blend = new ColorBlend();
                            blend.Positions = new float[] { 0f, .5f, 1f };
                            blend.Colors = colrs;
                            br.InterpolationColors = blend;

                            g.FillPath(br, path);
                        }
                    }
                    GraphicsPath pathLine = CreatePath(new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), 2);
                    using (Pen pen = new Pen(Color.FromArgb(172, 180, 191)))
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
                        //using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(32, 46, 64), Color.FromArgb(102, 113, 127), 90))
                        //{
                        //    Color[] colrs = new Color[3];
                        //    //colrs[0] = Color.FromArgb(65, 87, 117);
                        //    colrs[0] = Color.FromArgb(102, 113, 127);
                        //    colrs[1] = Color.FromArgb(32, 46, 64);
                        //    colrs[2] = Color.FromArgb(32, 46, 64);
                        //    ColorBlend blend = new ColorBlend();
                        //    blend.Positions = new float[] { 0f, .5f, 1f };
                        //    blend.Colors = colrs;
                        //    br.InterpolationColors = blend;

                        //    g.FillPath(br, path);
                        //}
                        using (LinearGradientBrush br = new LinearGradientBrush(bounds, Color.FromArgb(123, 123, 123), Color.FromArgb(149, 149, 149), -90))
                        {
                            g.FillPath(br, path);
                        }
                    }
                    GraphicsPath pathLine = CreatePath(new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1), 2);
                    using (Pen pen = new Pen(Color.FromArgb(255, 255, 255)))
                    {
                        g.DrawPath(pen, pathLine);
                    }
                    return;
                }
            }

            base.OnRenderDropDownButtonBackground(e);
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

        //protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        //{
        //    //base.OnRenderOverflowButtonBackground(e);
        //    ToolStripItem item = e.Item;
        //    ToolStrip toolstrip = e.ToolStrip;
        //    Graphics g = e.Graphics;
        //    g.SmoothingMode = SmoothingMode.AntiAlias;
        //    bool rightToLeft = item.RightToLeft == RightToLeft.Yes;

        //    bool bHorizontal = toolstrip.Orientation == Orientation.Horizontal;
        //    Rectangle rect = Rectangle.Empty;

        //    if (rightToLeft)
        //    {
        //        rect = new Rectangle(0, item.Height - 8, 10, 5);
        //    }
        //    else
        //    {
        //        rect = new Rectangle(item.Width - 12, item.Height - 8, 10, 5);
        //    }

        //    ArrowDirection direction = bHorizontal ? ArrowDirection.Down : ArrowDirection.Right;
        //    int x = (rightToLeft && bHorizontal) ? -1 : 1;
        //    rect.Offset(x, 1);

        //    Color color;

        //    if (item.Pressed)
        //    {
        //        color = Color.Orange;
        //    }
        //    else if (item.Selected)
        //    {
        //        color = Color.FromArgb(255, 211, 115);
        //    }
        //    else
        //    {
        //        color = overflowBackgroundColor;
        //    }

        //    using (Brush brush = new SolidBrush(color))
        //    {
        //        RenderArrowInternal(g, rect, direction, brush);
        //    }

        //    if (bHorizontal)
        //    {
        //        using (Pen pen = new Pen(color))
        //        {
        //            g.DrawLine(pen, rect.Right - 8, rect.Y - 2, rect.Right - 2, rect.Y - 2);
        //            g.DrawLine(pen, rect.Right - 8, rect.Y - 1, rect.Right - 2, rect.Y - 1);
        //        }

        //        Rectangle rect2 = new Rectangle(rect.Right - 8, 0, 6, item.Height - 11);
        //        using (Brush bbrush = new SolidBrush(color))
        //        {
        //            g.FillRectangle(bbrush, rect2);
        //        }
        //    }
        //    else
        //    {
        //        using (Pen pen = new Pen(overflowBackgroundColor))
        //        {
        //            g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom - 1);
        //            g.DrawLine(pen, rect.X, rect.Y + 1, rect.X, rect.Bottom);
        //        }
        //    }
        //}

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

        //        Color Color1 = Color.FromArgb(0, 179, 179, 179);
        //        Color Color2 = Color.FromArgb(179, 179, 179);
        //        Color Color3 = Color.FromArgb(0, 179, 179, 179);
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
        //                colors[0] = Color.FromArgb(0, 255, 255, 255);
        //                colors[1] = Color.FromArgb(255, 255, 255);
        //                colors[2] = Color.FromArgb(0, 255, 255, 255);
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

        //internal void RenderArrowInternal(Graphics g, Rectangle dropDownRect, ArrowDirection direction, Brush brush)
        //{
        //    Point point = new Point(
        //        dropDownRect.Left + (dropDownRect.Width / 2),
        //        dropDownRect.Top + (dropDownRect.Height / 2));
        //    Point[] points = null;
        //    switch (direction)
        //    {
        //        case ArrowDirection.Left:
        //            points = new Point[] { 
        //                new Point(point.X + 2, point.Y - 3), 
        //                new Point(point.X + 2, point.Y + 3), 
        //                new Point(point.X - 1, point.Y) };
        //            break;

        //        case ArrowDirection.Up:
        //            points = new Point[] { 
        //                new Point(point.X - 3, point.Y + 2), 
        //                new Point(point.X + 3, point.Y + 2), 
        //                new Point(point.X, point.Y - 2) };
        //            break;

        //        case ArrowDirection.Right:
        //            points = new Point[] {
        //                new Point(point.X - 2, point.Y - 3), 
        //                new Point(point.X - 2, point.Y + 3), 
        //                new Point(point.X + 1, point.Y) };
        //            break;

        //        default:
        //            points = new Point[] {
        //                new Point(point.X - 3, point.Y - 1), 
        //                new Point(point.X + 3, point.Y - 1), 
        //                new Point(point.X, point.Y + 2) };
        //            break;
        //    }
        //    g.FillPolygon(brush, points);
        //}

        //private void DrawSplitButtonSeperatorLine(Graphics g, ToolStripSplitButton item, Rectangle rect)
        //{
        //    Color Color1 = Color.FromArgb(0, 0, 0, 0);
        //    Color Color2 = Color.FromArgb(179, 179, 179);
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
        //protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        //{
        //    ToolStripButton button = e.Item as ToolStripButton;

        //    Graphics g = e.Graphics;
        //    Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
        //    if (button.Selected || button.Checked)
        //    {
        //        using (LinearGradientBrush br = new LinearGradientBrush(rect, Color.FromArgb(225, 225, 225), Color.FromArgb(255, 255, 255), 90))
        //        {
        //            GraphicsPath path = CreatePath(rect, 2);
        //            g.FillPath(br, path);
        //        }
        //        using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
        //        {
        //            Rectangle r = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        //            GraphicsPath path = CreatePath(r, 2);
        //            g.DrawPath(pen, path);
        //        }
        //    }

        //    base.OnRenderButtonBackground(e);
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

        //    }
        //    path.CloseFigure();

        //    return path;
        //}
    }
}
