using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using WSN.Ants.Controls.WSNSpliterContainer;

namespace WSN.Ants.Controls.WSNCaptionPanel
{
    /// <summary>
    /// WSN控件：
    ///     WSNCaptionPanel标题面板
    /// </summary>
    public class WSNCaptionPanel : Panel
    {
        #region 字段
        private FaceStyle controlSchema = new FaceStyle();
        private FaceStyle captionSchema = new FaceStyle();

        private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;


        private RoundStyle roundStyle = RoundStyle.None;
        private int radius = 8;
        private int _captionHeight = 30;

        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置面板圆角半径
        /// </summary>
        /// <remarks>默认为：8</remarks>
        [DefaultValue(8)]
        public int Radius
        {
            get { return radius; }
            set
            {
                if (radius != value)
                {
                    radius = value < 4 ? 4 : value;
                    SetReion();
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置面板的圆角样式
        /// </summary>
        /// <remarks>默认为：四个角都不是圆角</remarks>
        [DefaultValue(typeof(RoundStyle), "0")]
        public RoundStyle RoundStyle
        {
            get { return roundStyle; }
            set
            {
                if (roundStyle != value)
                {
                    roundStyle = value;
                    SetReion();
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置面板的标题外观样式相关
        /// </summary>
        public FaceStyle CaptionSchema
        {
            get { return captionSchema; }
            set { captionSchema = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置面板的控件样式
        /// </summary>
        public FaceStyle ControlSchema
        {
            get { return controlSchema; }
            set { controlSchema = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置文本对齐方式
        /// </summary>
        /// <remarks>默认为：内容在垂直方向上中间对齐，在水平方向上左边对齐</remarks>
        [DefaultValue(typeof(ContentAlignment), "16")]
        public virtual ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set
            {
                if (_textAlign != value)
                {
                    _textAlign = value;
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置标题文本
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置标题高度
        /// </summary>
        /// <remarks>默认为：30</remarks>
        [DefaultValue(30)]
        public virtual int CaptionHeight
        {
            get { return _captionHeight; }
            set
            {
                if (_captionHeight != value)
                {
                    _captionHeight = value < 8 ? 8 : value;
                    base.Invalidate();
                }
            }
        }
        internal protected Rectangle CaptionRect
        {
            get
            {
                Rectangle captionRect = base.ClientRectangle;
                captionRect.Height = _captionHeight;
                return captionRect;
            }
        }

        /// <summary>
        /// 控件属性：
        ///     获取显示区域
        /// </summary>
        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = base.DisplayRectangle;
                rect.Y += _captionHeight;
                rect.Height -= _captionHeight;
                return rect;
            }
        }
        #endregion


        #region  构造函数
        /// <summary>
        /// 创建标题面板控件<c>WSNCaptionPanel</c>
        /// </summary>
        public WSNCaptionPanel()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); 
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            InitColor();
        }
        #endregion

        #region 私有方法

        private void InitColor()
        {
            controlSchema.BackNormalStyle.Color1 = Color.FromArgb(255, 255, 255);
            controlSchema.BackNormalStyle.Color2 = Color.FromArgb(215, 215, 215);
            controlSchema.BackNormalStyle.Mode = LinearGradientMode.Vertical;

            controlSchema.BorderNormalStyle.Color1 = Color.FromArgb(179, 179, 179);
            controlSchema.BorderNormalStyle.Color2 = Color.Empty;

            //caption
            captionSchema.BackNormalStyle.Color1 = Color.FromArgb(255, 255, 255);
            captionSchema.BackNormalStyle.Color2 = Color.FromArgb(215, 215, 215);
            captionSchema.BackNormalStyle.Mode = LinearGradientMode.Vertical;

            captionSchema.BorderNormalStyle.Color1 = Color.FromArgb(179, 179, 179);
            captionSchema.BorderNormalStyle.Color2 = Color.FromArgb(255, 255, 255);

        }
        private void SetReion()
        {

            if (base.Region != null)
            {
                base.Region.Dispose();
            }
            using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, false))
            {
                base.Region = new Region(path);
            }

        }

        #endregion


        #region 重载方法
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SetReion();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetReion();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            Graphics g = e.Graphics;


            #region 画客户区


            using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, false))
            {
                g.SetClip(path);

                if (controlSchema.BackNormalStyle.Color2 != Color.Empty
                    && controlSchema.BackNormalStyle.Color1 != controlSchema.BackNormalStyle.Color2)
                {
                    if (DisplayRectangle.Width > 0 && DisplayRectangle.Height > 0)
                    {
                        using (LinearGradientBrush br = new LinearGradientBrush(DisplayRectangle, controlSchema.BackNormalStyle.Color1, controlSchema.BackNormalStyle.Color2, controlSchema.BackNormalStyle.Mode))
                        {
                            g.FillPath(br, path);
                            //g.FillRectangle(br, DisplayRectangle);

                        }
                    }
                }
                else
                {
                    using (SolidBrush br = new SolidBrush(controlSchema.BackNormalStyle.Color1))
                    {
                        g.FillPath(br, path);
                        //g.FillRectangle(br, DisplayRectangle);

                    }

                }

            }




            #region 画背景图片
            if (BackgroundImage != null)
            {
                switch (BackgroundImageLayout)
                {
                    case ImageLayout.None:
                        g.DrawImageUnscaled(BackgroundImage,
                            DisplayRectangle.X,
                            DisplayRectangle.Y,
                            BackgroundImage.Width,
                            BackgroundImage.Height);
                        break;
                    case ImageLayout.Tile:
                        using (TextureBrush Txbrus = new TextureBrush(BackgroundImage))
                        {
                            Txbrus.WrapMode = WrapMode.Tile;

                            g.FillRectangle(Txbrus, new Rectangle(0, 0, DisplayRectangle.Width - 1, DisplayRectangle.Height - 1));

                        }
                        break;
                    case ImageLayout.Center:

                        int xx = (DisplayRectangle.Width - BackgroundImage.Width) / 2;
                        int yy = (DisplayRectangle.Height - BackgroundImage.Height) / 2;
                        g.DrawImage(BackgroundImage, new Rectangle(xx, yy, BackgroundImage.Width, BackgroundImage.Height), new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height), GraphicsUnit.Pixel);



                        break;
                    case ImageLayout.Stretch:

                        g.DrawImage(BackgroundImage, new Rectangle(0, 0, DisplayRectangle.Width, DisplayRectangle.Height), new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height), GraphicsUnit.Pixel);


                        break;

                    case ImageLayout.Zoom:
                        {
                            double tm = 0.0;
                            int W = BackgroundImage.Width;
                            int H = BackgroundImage.Height;
                            if (W > DisplayRectangle.Width)
                            {
                                tm = DisplayRectangle.Width / BackgroundImage.Width;
                                W = (int)(W * tm);
                                H = (int)(H * tm);
                            }
                            if (H > DisplayRectangle.Height)
                            {
                                tm = DisplayRectangle.Height / H;
                                W = (int)(W * tm);
                                H = (int)(H * tm);
                            }
                            using (Bitmap tmpBP = new Bitmap(W, H))
                            {
                                using (Graphics G2 = Graphics.FromImage(tmpBP))
                                {
                                    G2.DrawImage(BackgroundImage, new Rectangle(0, 0, W, H), new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height), GraphicsUnit.Pixel);

                                    int xxx = (DisplayRectangle.Width - W) / 2;
                                    int yyy = (DisplayRectangle.Height - H) / 2;
                                    g.DrawImage(tmpBP, new Rectangle(xxx, yyy, W, H), new Rectangle(0, 0, W, H), GraphicsUnit.Pixel);

                                }
                            }
                        }
                        break;

                }

            }

            #endregion


            #endregion

            #region 画标题
            OnPaintCaption(e);
            #endregion

            
            #region 画border
            using (SmoothingModeGraphics sgr = new SmoothingModeGraphics(g))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle r = ClientRectangle;
                //r.Width -= 1;
                //r.Height -= 1;
                using (GraphicsPath path =
                GraphicsPathHelper.CreatePath(r, Radius, RoundStyle, true))
                {
                    using (Pen p = new Pen(controlSchema.BorderNormalStyle.Color1))
                    {

                        g.DrawPath(p, path);
                    }
                }

                if (controlSchema.BorderNormalStyle.Color2 != Color.Empty
                    && controlSchema.BorderNormalStyle.Color2 != controlSchema.BorderNormalStyle.Color1)
                {
                    r.Inflate(-1, -1);
                    using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                    r, radius, roundStyle, true))
                    {
                        using (Pen pen = new Pen(controlSchema.BorderNormalStyle.Color2))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
            #endregion



        }

        protected virtual void OnPaintCaption(PaintEventArgs e)
        {


            Graphics g = e.Graphics;
            Rectangle captionRect = CaptionRect;

            if (captionSchema.BackNormalStyle.Color2 != Color.Empty
                && (captionSchema.BackNormalStyle.Color2 != captionSchema.BackNormalStyle.Color1))
            {
                if (CaptionRect.Width > 0 && CaptionRect.Height > 0)
                {
                    using (LinearGradientBrush br = new LinearGradientBrush(CaptionRect, captionSchema.BackNormalStyle.Color1, captionSchema.BackNormalStyle.Color2, captionSchema.BackNormalStyle.Mode))
                    {
                        g.FillRectangle(br, CaptionRect);
                    }
                }
            }
            else
            {
                using (SolidBrush br = new SolidBrush(captionSchema.BackNormalStyle.Color1))
                {
                    g.FillRectangle(br, CaptionRect);
                }
            }

            #region 画底线
            using (Pen pen = new Pen(captionSchema.BorderNormalStyle.Color2,1.0f))
            {
                g.DrawLine(pen, new Point(captionRect.X, captionRect.Height ), new Point(captionRect.X + captionRect.Width, captionRect.Height ));
            }
            if (captionSchema.BorderNormalStyle.Color2 != Color.Empty)
            {
                using (Pen pen = new Pen(captionSchema.BorderNormalStyle.Color1,1.0f))
                {
                    g.DrawLine(pen, new Point(captionRect.X, captionRect.Height - 1), new Point(captionRect.X + captionRect.Width, captionRect.Height - 1));
                }
            }
            #endregion

            RenderText(g, CaptionRect);

        }

        private void RenderText(Graphics g, Rectangle captionRect)
        {
            Rectangle textRect = Rectangle.Empty;
            StringFormat sf = new StringFormat();
            sf.FormatFlags = StringFormatFlags.NoWrap;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            bool rightToLeft = base.RightToLeft == RightToLeft.Yes;

            if (rightToLeft)
            {
                sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }

            textRect = new Rectangle(
                        captionRect.X + 3,
                        captionRect.Y,
                        captionRect.Width - 6,
                        captionRect.Height);

            if (rightToLeft)
            {
                textRect.X = captionRect.Right - textRect.Right;
            }

            sf.LineAlignment = StringAlignment.Center;

            switch (_textAlign)
            {
                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    break;
            }
            if (!string.IsNullOrEmpty(base.Text))
            {
                using (Brush brush = new SolidBrush(ForeColor))
                {
                    g.DrawString(
                        base.Text,
                        Font,
                        brush,
                        textRect,
                        sf);
                }
            }
        }
        #endregion
    }
}
