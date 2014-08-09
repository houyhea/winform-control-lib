using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WSN.Ants.Controls.WSNContextMenuStrip
{
    /// <summary>
    /// contextmenu 渲染类
    /// </summary>
    public class ContextMenuRender : ToolStripRenderer
    {
        //private Color _BackColor1 = Color.FromArgb(216, 216, 216);
        //private Color _BackColor2 = Color.FromArgb(255, 255, 255);
        private Color _BackColor1 = Color.FromArgb(241, 241, 241);//弹出菜单栏背景色
        private Color _BackColor2 = Color.FromArgb(241, 241, 241);

        private Color _ImageSideBarBackColor1 = Color.FromArgb(228, 228, 228);//弹出菜单栏图片区域背景色
        private Color _ImageSideBarBackColor2 = Color.FromArgb(228, 228, 228);

        private Color _ForeColorNormal = Color.FromArgb(0, 0, 0);
        private Color _ForeColorHover = Color.FromArgb(255, 255, 255);
        private Color _ItemBackColorNormal = Color.Empty;
        private Color _ItemBackColorHover = Color.FromArgb(27, 40, 55);
        private Color _BorderColor = Color.FromArgb(179, 179, 179);

        private Color _SeperatorLineColor1 = Color.FromArgb(179, 179, 179);
        private Color _SeperatorLineColor2 = Color.FromArgb(255, 255, 255);
        private int _Radius = 3;

        private Dictionary<String, Image> _itemImageHoverList;
        private Image _checkedItemImage;

        private int imageBlockWidth = 26;

        public Image CheckedItemImage
        {
            get { return _checkedItemImage; }
            set { _checkedItemImage = value; }
        }

        /// <summary>
        /// 每一项的HOVER状态图片，以item.text作为KEY
        /// </summary>
        public Dictionary<String, Image> ItemImageHoverList
        {
            get
            {
                if (_itemImageHoverList == null)
                    _itemImageHoverList = new Dictionary<String, Image>();
                return _itemImageHoverList;
            }
            set { _itemImageHoverList = value; }
        }

        /// <summary>
        /// 分割线颜色底部
        /// </summary>
        public Color SeperatorLineColor2
        {
            get { return _SeperatorLineColor2; }
            set { _SeperatorLineColor2 = value; }
        }
        /// <summary>
        /// 分割线颜色顶部
        /// </summary>
        public Color SeperatorLineColor1
        {
            get { return _SeperatorLineColor1; }
            set { _SeperatorLineColor1 = value; }
        }
        /// <summary>
        /// 子项背景色(Hover状态)
        /// </summary>
        public Color ItemBackColorHover
        {
            get { return _ItemBackColorHover; }
            set { _ItemBackColorHover = value; }
        }
        /// <summary>
        /// 子项背景色(Normal状态)
        /// </summary>
        public Color ItemBackColorNormal
        {
            get { return _ItemBackColorNormal; }
            set { _ItemBackColorNormal = value; }
        }
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; }
        }
        /// <summary>
        /// 文字颜色(Hover状态)
        /// </summary>
        public Color ForeColorHover
        {
            get { return _ForeColorHover; }
            set { _ForeColorHover = value; }
        }
        /// <summary>
        /// 文字颜色(Normal状态)
        /// </summary>
        public Color ForeColorNormal
        {
            get { return _ForeColorNormal; }
            set { _ForeColorNormal = value; }
        }

        public Color BackColor2
        {
            get { return _BackColor2; }
            set { _BackColor2 = value; }
        }

        public Color BackColor1
        {
            get { return _BackColor1; }
            set { _BackColor1 = value; }
        }

        public Color ImageSideBarBackColor1
        {
            get { return _ImageSideBarBackColor1; }
            set { _ImageSideBarBackColor1 = value; }
        }

        public Color ImageSideBarBackColor2
        {
            get { return _ImageSideBarBackColor2; }
            set { _ImageSideBarBackColor2 = value; }
        }

        public int Radius
        {
            get { return _Radius; }
            set { _Radius = value; }
        }

        public ContextMenuRender()
            : base()
        {

        }

        /// <summary>
        /// 画整个菜单的背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderToolStripBackground(
            ToolStripRenderEventArgs e)
        {
            //Color baseColor = ColorTable.BackColorNormal;
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (toolStrip is ToolStripDropDown)
            {
                CreateRegion(e.ToolStrip, e.AffectedBounds);

                Rectangle rect = e.AffectedBounds;

                Rectangle imageRect = new Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, imageBlockWidth, e.AffectedBounds.Height);
                Rectangle contentRect = new Rectangle(e.AffectedBounds.X + imageBlockWidth, e.AffectedBounds.Y, e.AffectedBounds.Width - imageBlockWidth, e.AffectedBounds.Height);

                using (GraphicsPath imagePath = CreateLeftRadiusPath(imageRect, 2))
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

                using (GraphicsPath contentPath = CreateRightRadiusPath(contentRect, 2))
                {
                    if (e.AffectedBounds.Width > 00 && e.AffectedBounds.Height > 0)
                    {
                        using (LinearGradientBrush br = new LinearGradientBrush(contentRect, this._BackColor1, this._BackColor2, LinearGradientMode.Vertical))
                        {
                            g.FillPath(br, contentPath);
                        }
                    }
                }

                //using (GraphicsPath path = CreatePath(
                //    rect, 2))
                //{
                //    if (e.AffectedBounds.Width > 0 && e.AffectedBounds.Height > 0)
                //    {
                //        using (LinearGradientBrush brush = new LinearGradientBrush(e.AffectedBounds, BackColor1, BackColor2, -90))
                //        {
                //            //g.FillPath(brush, path);
                //            g.FillRectangle(brush, e.AffectedBounds);
                //        }
                //    }
                using (Pen pen = new Pen(BorderColor))
                {
                    //g.DrawPath(pen, path);
                    g.DrawRectangle(pen, e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);
                }
                //}
            }
        }

        /// <summary>
        /// 画分割线
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderSeparator(
            ToolStripSeparatorRenderEventArgs e)
        {
            Rectangle rect = e.Item.ContentRectangle;

            using (Pen pen = new Pen(SeperatorLineColor1))
            {
                e.Graphics.DrawLine(pen, rect.X + this.imageBlockWidth, rect.Y, rect.Right - 2, rect.Top);
            }
            using (Pen pen = new Pen(SeperatorLineColor2))
            {
                e.Graphics.DrawLine(pen, rect.X + this.imageBlockWidth, rect.Y + 1, rect.Right - 2, rect.Top + 1);
            }
        }

        /// <summary>
        /// 画menuitem的背景，如果不画，则没有hover，selecte的衬底背景效果。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderMenuItemBackground(
            ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Enabled)
            {
                return;
            }

            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
            g.SmoothingMode = SmoothingMode.AntiAlias;

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
        ///// <summary>
        ///// 画item图片
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnRenderItemImage(
        //    ToolStripItemImageRenderEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    g.InterpolationMode = InterpolationMode.HighQualityBilinear;

        //    if (e.Item is ToolStripMenuItem)
        //    {
        //        ToolStripMenuItem item = (ToolStripMenuItem)e.Item;
        //        if (item.Checked)
        //        {
        //            return;
        //        }

        //        Rectangle rect = e.ImageRectangle;

        //        //if (e.Item.Enabled && e.Item.Selected )
        //        //{
        //        //    //Image img = new Bitmap(this.GetType(), "uphover.png");
        //        //    //如果有设置了hover的图片，则显示之
        //        //    Image img = item.Image;
        //        //    try
        //        //    {
        //        //        img = ItemImageHoverList[e.Item.Text];
        //        //    }
        //        //    catch (KeyNotFoundException er)
        //        //    {
        //        //        base.OnRenderItemImage(e);
        //        //        return;
        //        //    }

        //        //    if (img != null)
        //        //    {
        //        //        e.Graphics.DrawImageUnscaled(img, e.ImageRectangle.X, e.ImageRectangle.Y);
        //        //        return;
        //        //    }
        //        //}

        //        ToolStripItemImageRenderEventArgs ne =
        //            new ToolStripItemImageRenderEventArgs(
        //            e.Graphics, e.Item, e.Image, rect);
        //        base.OnRenderItemImage(ne);
        //        return;
        //    }

        //    base.OnRenderItemImage(e);
        //}

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            if (CheckedItemImage == null)
                base.OnRenderItemCheck(e);
            else
            {
                e.Graphics.DrawImageUnscaled(CheckedItemImage, e.ImageRectangle.X, e.ImageRectangle.Y);
    
            }
            
            //base.OnRenderItemCheck(e);
        }


        protected override void OnRenderItemText(
            ToolStripItemTextRenderEventArgs e)
        {
            //e.TextColor = e.Item.Selected ? ForeColorHover : ForeColorNormal;
            //e.TextColor = ForeColorNormal;

            //if (!(e.ToolStrip is MenuStrip) && (e.Item is ToolStripMenuItem))
            //{
            //    Rectangle rect = e.TextRectangle;
            //    e.TextRectangle = rect;
            //}
            if(e.Item.Selected && e.Item is ToolStripMenuItem)
                e.TextColor = ForeColorNormal;
            base.OnRenderItemText(e);
        }

        public void CreateRegion(Control control, Rectangle rect)
        {
            using (GraphicsPath path =
                CreatePath(rect, Radius))
            {
                if (control.Region != null)
                {
                    control.Region.Dispose();
                }
                control.Region = new Region(path);
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

                //以下是圆弧的圆角
                //path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                //path.AddArc(
                //    rect.Right - radius - radiusCorrection,
                //    rect.Y,
                //    radius,
                //    radius,
                //    270,
                //    90);
                //path.AddArc(
                //    rect.Right - radius ,
                //    rect.Bottom - radius ,
                //    radius,
                //    radius, 0, 90);
                //path.AddArc(
                //    rect.X,
                //    rect.Bottom - radius ,
                //    radius,
                //    radius,
                //    90,
                //    90);

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
    }
}
