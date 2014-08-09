using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace WSN.Ants.Controls.WSNButton
{
    /// <summary>
    /// WSN控件：
    ///     自定义按钮<c>WSNButton</c>
    /// </summary>
    /// <remarks>自定义按钮，可以实现显示图片，显示文本，显示文本加图片,可以设置各状态颜色.</remarks>
    /// <example>
    /// 以下示例展示如何使用WSNButton
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNButtonExamples.cs" region="WSNButtonExample"/>
    /// </example>
    public class WSNButton : Button
    {
        #region 字段
        private ControlState _controlState;
        private RoundStyle _roundStyle = RoundStyle.All;
        private int _radius = 8;
        private int _imageWidth = 18;
        private bool _imageMode = false;
        private bool _checked = false;
        private WSNButtonImageTable _imageTable = new WSNButtonImageTable();
        private FaceStyle _controlFaceSchema;
        private bool _glassEnable = true;





        private ToolTip _toolTip;
        private string _toolTipText = string.Empty;

        #endregion

        #region 构造函数
        /// <summary>
        /// 创建按钮
        /// </summary>
        public WSNButton()
            : base()
        {

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            _toolTip = new ToolTip();


            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;

            this.BackgroundImageLayout = ImageLayout.Center;
            //使该控件不可接收焦点
            SetStyle(ControlStyles.Selectable, false);

            //初始化长按事件参数
            InitLongClick();

            ResetControlFaceSchema();


        }
        #endregion


        #region 长按事件相关
        private bool allowLongClick;
        private Timer longClickTimer;
        private bool isLongClickTrigger;
        [Description("长按事件")]
        /// <summary>
        /// 长按事件
        /// </summary>
        public event EventHandler LongClick;

        [Description("长按事件触发时长")]
        [DefaultValue(500)]
        /// <summary>
        /// 长按事件触发时长
        /// </summary>
        public int LongClickInterval
        {
            get
            {
                if (longClickTimer == null) return 500;
                return longClickTimer.Interval;
            }
            set
            {
                if (value > 1000 * 5)
                    value = 1000 * 5;
                if (value <= 200)
                    value = 200;
                longClickTimer.Interval = value;
            }
        }
        [Description("是否允许长按事件触发。默认：不允许")]
        [DefaultValue(false)]
        /// <summary>
        /// 是否允许长按事件触发。默认：不允许
        /// </summary>
        public bool AllowLongClick
        {
            get { return allowLongClick; }
            set { allowLongClick = value; }
        }
        void longClickTimer_Tick(object sender, EventArgs e)
        {
            longClickTimer.Enabled = false;
            if (LongClick != null)
                LongClick(this, new EventArgs());
            isLongClickTrigger = true;
        }
        private void InitLongClick()
        {
            allowLongClick = false;

            longClickTimer = new Timer();
            LongClickInterval = 500;
            longClickTimer.Enabled = false;
            longClickTimer.Tick += new EventHandler(longClickTimer_Tick);
            isLongClickTrigger = false;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否启用玻璃效果，默认是TRUE
        /// </summary>
        public bool GlassEnable
        {
            get { return _glassEnable; }
            set { _glassEnable = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置外观相关样式
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public FaceStyle ControlFaceSchema
        {
            get
            {

                return _controlFaceSchema;
            }
            set { _controlFaceSchema = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置提示文本
        /// </summary>
        public string ToolTipText
        {
            get { return _toolTipText; }
            set { _toolTipText = value; }
        }



        /// <summary>
        /// 控件属性：
        ///     获取或设置是否开启图片显示模式
        /// </summary>
        /// <remarks>如果启用，则按钮只显示图片，不显示TEXT</remarks>
        [Category("Appearance"), DefaultValue(false), Description("设置按钮显示是否是图片模式。")]
        public bool ImageMode
        {
            get { return _imageMode; }
            set
            {
                _imageMode = value;
                this.Refresh();

            }
        }
        internal ToolTip ToolTip
        {
            get { return _toolTip; }
        }
        /// <summary>
        /// 控件属性：
        ///     按钮使用的图片列表
        /// </summary>
        /// <remarks>当imagemode为true的时候，从该属性取各状态的图片绘制控件。</remarks>
        public WSNButtonImageTable ImageTable
        {
            get { return _imageTable; }
            set { _imageTable = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     是否被选中
        /// </summary>
        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; base.Invalidate(); }
        }
        /// <summary>
        /// 控件属性：
        ///     按钮上图片宽度
        /// </summary>
        /// <remarks>默认为：18</remarks>
        [DefaultValue(18)]
        public int ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                if (value != _imageWidth)
                {

                    _imageWidth = value < 12 ? 12 : value;
                    base.Invalidate();
                }
            }
        }

        /// <summary>
        /// 控件属性：
        ///     按钮的圆角样式
        /// </summary>
        /// <remarks>默认为：四个角都为圆角</remarks>
        [DefaultValue(typeof(RoundStyle), "1")]
        public RoundStyle RoundStyle
        {
            get { return _roundStyle; }
            set
            {
                if (_roundStyle != value)
                {
                    _roundStyle = value;
                    base.Invalidate();
                }
            }
        }

        /// <summary>
        /// 控件属性：
        ///     圆角半径
        /// </summary>
        /// <remarks>默认为：8</remarks>
        [DefaultValue(8)]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value < 4 ? 4 : value;
                    base.Invalidate();
                }
            }
        }

        internal ControlState ControlState
        {
            get { return _controlState; }
            set
            {
                if (_controlState != value)
                {
                    _controlState = value;
                    base.Invalidate();
                }
            }
        }
        #endregion

        #region 设计时属性串行化方法
        /// <summary>
        /// 重设按钮的外观样式
        /// </summary>
        public void ResetControlFaceSchema()
        {

            _controlFaceSchema = new FaceStyle();
            //初始化颜色
            //back,color1是上面，color2是下面。
            ControlFaceSchema.BackNormalStyle.Color2 = Color.FromArgb(245, 245, 245);
            ControlFaceSchema.BackNormalStyle.Color1 = Color.FromArgb(226, 226, 226);

            ControlFaceSchema.BackHoverStyle.Color2 = Color.FromArgb(255, 241, 211);
            ControlFaceSchema.BackHoverStyle.Color1 = Color.FromArgb(255, 214, 125);

            ControlFaceSchema.BackDisabledStyle.Color1 = Color.FromArgb(241, 241, 241);

            ControlFaceSchema.BackCheckedStyle.Color2 = Color.FromArgb(255, 211, 115);
            ControlFaceSchema.BackCheckedStyle.Color1 = Color.FromArgb(255, 211, 115);

            //border
            ControlFaceSchema.BorderNormalStyle.Color1 = Color.FromArgb(179, 179, 179);
            ControlFaceSchema.BorderHoverStyle.Color1 = Color.FromArgb(224, 153, 0);
            ControlFaceSchema.BorderDisabledStyle.Color1 = Color.FromArgb(220, 220, 220);
            ControlFaceSchema.BorderCheckedStyle.Color1 = Color.FromArgb(224, 153, 0);


            //text
            ControlFaceSchema.TextNormalStyle.Color1 = Color.FromArgb(72, 72, 72);
            ControlFaceSchema.TextHoverStyle.Color1 = Color.FromArgb(156, 88, 7);
            ControlFaceSchema.TextDisabledStyle.Color1 = Color.FromArgb(179, 179, 179);
            ControlFaceSchema.TextCheckedStyle.Color1 = Color.FromArgb(156, 88, 7);

        }
        /// <summary>
        ///  判断是否需要序列化按钮的外观样式
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeControlFaceSchema()
        {
            return (ControlFaceSchema.BackNormalStyle.Color2 != Color.FromArgb(245, 245, 245))
                || (ControlFaceSchema.BackNormalStyle.Color1 != Color.FromArgb(226, 226, 226))
             || (ControlFaceSchema.BackHoverStyle.Color2 != Color.FromArgb(255, 241, 211))
             || (ControlFaceSchema.BackHoverStyle.Color1 != Color.FromArgb(255, 214, 125))
             || (ControlFaceSchema.BackDisabledStyle.Color1 != Color.FromArgb(241, 241, 241))
             || (ControlFaceSchema.BackCheckedStyle.Color2 != Color.FromArgb(255, 211, 115))
             || (ControlFaceSchema.BackCheckedStyle.Color1 != Color.FromArgb(255, 211, 115))
                //border
             || (ControlFaceSchema.BorderNormalStyle.Color1 != Color.FromArgb(179, 179, 179))
             || (ControlFaceSchema.BorderHoverStyle.Color1 != Color.FromArgb(224, 153, 0))
             || (ControlFaceSchema.BorderDisabledStyle.Color1 != Color.FromArgb(220, 220, 220))
             || (ControlFaceSchema.BorderCheckedStyle.Color1 != Color.FromArgb(224, 153, 0))
                //text
             || (ControlFaceSchema.TextNormalStyle.Color1 != Color.FromArgb(72, 72, 72))
             || (ControlFaceSchema.TextHoverStyle.Color1 != Color.FromArgb(156, 88, 7))
             || (ControlFaceSchema.TextDisabledStyle.Color1 != Color.FromArgb(179, 179, 179))
             || (ControlFaceSchema.TextCheckedStyle.Color1 != Color.FromArgb(156, 88, 7));
        }

        #endregion

        #region 重载方法
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ControlState = ControlState.Hover;
            if (ToolTipText != string.Empty)
            {
                HideToolTip();
                ShowToolTip(ToolTipText);
            }
        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlState = ControlState.Normal;
            HideToolTip();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                ControlState = ControlState.Pressed;
            }
            //长按事件相关
            if (AllowLongClick)
                longClickTimer.Enabled = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            //长按事件相关
            longClickTimer.Enabled = false;
            if (!isLongClickTrigger)
            {
                base.OnMouseUp(e);
            }

            if (isLongClickTrigger)
            {
                isLongClickTrigger = false;
            }


            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (ClientRectangle.Contains(e.Location))
                {
                    ControlState = ControlState.Hover;
                }
                else
                {
                    ControlState = ControlState.Normal;
                }
            }
            ControlState = ControlState.Normal;
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            ControlState = ControlState.Normal;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.OnPaintBackground(e);



            Graphics g = e.Graphics;
            Rectangle imageRect;
            Rectangle textRect;

            CalculateRect(out imageRect, out textRect);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color textColor = ControlFaceSchema.TextNormalStyle.Color1;

            if (!ImageMode)//文本模式
            {
                if (Enabled)
                {
                    if (Checked)
                    {
                        switch (ControlState)
                        {
                            case ControlState.Hover:
                                RenderBackgroundInternal(g
                                    , ClientRectangle
                                    , ControlFaceSchema.BackHoverStyle
                                    , ControlFaceSchema.BorderHoverStyle
                                    , RoundStyle
                                    , Radius);
                                textColor = ControlFaceSchema.TextHoverStyle.Color1;
                                break;
                            default:
                                RenderBackgroundInternal(g
                                            , ClientRectangle
                                            , ControlFaceSchema.BackCheckedStyle
                                            , ControlFaceSchema.BorderCheckedStyle
                                            , RoundStyle
                                            , Radius);
                                textColor = ControlFaceSchema.TextCheckedStyle.Color1;
                                break;
                        }
                    }
                    else
                    {
                        switch (ControlState)
                        {
                            case ControlState.Hover:
                                RenderBackgroundInternal(g
                                    , ClientRectangle
                                    , ControlFaceSchema.BackHoverStyle
                                    , ControlFaceSchema.BorderHoverStyle
                                    , RoundStyle
                                    , Radius);
                                textColor = ControlFaceSchema.TextHoverStyle.Color1;
                                break;
                            case ControlState.Pressed:
                                RenderBackgroundInternal(g
                                    , ClientRectangle
                                    , ControlFaceSchema.BackCheckedStyle
                                    , ControlFaceSchema.BorderCheckedStyle
                                    , RoundStyle
                                    , Radius);
                                textColor = ControlFaceSchema.TextCheckedStyle.Color1;
                                break;
                            default:
                                RenderBackgroundInternal(g
                                    , ClientRectangle
                                    , ControlFaceSchema.BackNormalStyle
                                    , ControlFaceSchema.BorderNormalStyle
                                    , RoundStyle
                                    , Radius);
                                textColor = ControlFaceSchema.TextNormalStyle.Color1;
                                break;
                        }
                    }
                }
                else
                {
                    RenderBackgroundInternal(g
                                , ClientRectangle
                                , ControlFaceSchema.BackDisabledStyle
                                , ControlFaceSchema.BorderDisabledStyle
                                , RoundStyle
                                , Radius);
                    textColor = ControlFaceSchema.TextDisabledStyle.Color1;
                }



                if (Image != null)
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    g.DrawImage(
                        Image,
                        imageRect,
                        0,
                        0,
                        Image.Width,
                        Image.Height,
                        GraphicsUnit.Pixel);
                }
                //画玻璃效果
                if (GlassEnable && Enabled)
                {
                    Rectangle r = ClientRectangle;
                    r.Height = r.Height / 2;
                    if (r.Height > 0 && r.Width > 0)
                    {

                        using (GraphicsPath path =
                        GraphicsPathHelper.CreatePathHalf(ClientRectangle, Radius, RoundStyle, true))
                        {
                            //255:表示完全不透明，color1是上面。
                            using (LinearGradientBrush br = new LinearGradientBrush(r, Color.FromArgb(255, Color.White), Color.FromArgb(70, Color.White), 90))
                            {
                                g.FillPath(br, path);
                            }
                        }
                    }
                }
                //TextRenderer.DrawText(
                //    g,
                //    Text,
                //    Font,
                //    textRect,
                //    textColor,
                //    GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));

                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Trimming = StringTrimming.EllipsisCharacter;

                    using (SolidBrush br = new SolidBrush(textColor))
                    {
                        g.DrawString(Text, Font, br, textRect, sf);
                    }

                }
            }
            else//图片模式，直接根据状态贴图即可
            {
                Image img = ImageTable.ImageNormal;
                if (Checked)
                {
                    img = ImageTable.ImageChecked;
                }
                else
                {
                    if (Enabled)
                    {
                        switch (ControlState)
                        {
                            case Ants.Controls.ControlState.Hover:
                                img = ImageTable.ImageHover;
                                break;
                            case Ants.Controls.ControlState.Normal:
                                img = ImageTable.ImageNormal;
                                break;
                            case Ants.Controls.ControlState.Pressed:
                                img = ImageTable.ImageChecked;
                                break;


                        }
                    }
                    else
                    {
                        img = ImageTable.ImageDisalbed;
                    }
                }
                if (img != null)
                {
                    this.Text = string.Empty;
                    this.BackgroundImage = img;
                    base.OnPaint(e);


                    //this.Width = img.Width;
                    //this.Height = img.Height;
                    //g.DrawImage(
                    //    img,
                    //    e.ClipRectangle,
                    //    0,
                    //    0,
                    //    img.Width,
                    //    img.Height,
                    //    GraphicsUnit.Pixel);
                }

                if (this.DesignMode)
                {
                    using (Pen pen = new Pen(Color.Gray))
                    {
                        Rectangle r = ClientRectangle;
                        r.Height -= 1;
                        r.Width -= 1;
                        g.DrawRectangle(pen, r);
                    }
                }

            }
        }



        #endregion

        #region 内部方法

        private void HideToolTip()
        {
            ToolTip.Active = false;
        }

        private void ShowToolTip(string toolTipText)
        {
            ToolTip.Active = true;
            ToolTip.SetToolTip(this, toolTipText);
        }
        private void RenderBackgroundInternal(Graphics g
            , Rectangle ClientRectangle
            , BrushParameter backStyle
            , BrushParameter bordStyle
            , Ants.Controls.RoundStyle RoundStyle
            , int Radius)
        {
            #region 画背景
            if (backStyle.Color2 == Color.Empty || backStyle.Color2 == backStyle.Color1)
            {
                using (SolidBrush brush = new SolidBrush(backStyle.Color1))
                {
                    using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, true))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }
            else
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                 ClientRectangle, backStyle.Color1, backStyle.Color2, backStyle.Mode))
                {
                    using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, true))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }
            #endregion


            #region 画边框
            using (GraphicsPath path =
                            GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, true))
            {
                using (Pen pen = new Pen(bordStyle.Color1))
                {
                    g.DrawPath(pen, path);
                }
            }
            if (bordStyle.Color2 != Color.Empty)
            {
                ClientRectangle.Inflate(-1, -1);
                Rectangle r = ClientRectangle;
                r.X += 1;
                r.Y += 1;
                r.Width -= 2;
                r.Height -= 2;

                using (GraphicsPath path =
                    //GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, false))
                    GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, true))
                {
                    using (Pen pen = new Pen(bordStyle.Color2))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }

            #endregion


        }


        private void CalculateRect(
           out Rectangle imageRect, out Rectangle textRect)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            if (Image == null)
            {
                textRect = new Rectangle(
                   2,
                   0,
                   Width - 4,
                   Height);
                return;
            }
            switch (TextImageRelation)
            {
                case TextImageRelation.Overlay:
                    imageRect = new Rectangle(
                        5,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        5,
                        0,
                        Width - 4,
                        Height);
                    break;
                case TextImageRelation.ImageAboveText:
                    imageRect = new Rectangle(
                        (Width - ImageWidth) / 2,
                        5,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        5,
                        imageRect.Bottom,
                        Width,
                        Height - imageRect.Bottom - 2);
                    break;
                case TextImageRelation.ImageBeforeText:
                    imageRect = new Rectangle(
                        5,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        imageRect.Right + 2,
                        0,
                        Width - imageRect.Right - 4,
                        Height);
                    break;
                case TextImageRelation.TextAboveImage:
                    imageRect = new Rectangle(
                        (Width - ImageWidth) / 2,
                        Height - ImageWidth - 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        0,
                        2,
                        Width,
                        Height - imageRect.Y - 2);
                    break;
                case TextImageRelation.TextBeforeImage:
                    imageRect = new Rectangle(
                        Width - ImageWidth - 2,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        2,
                        0,
                        imageRect.X - 2,
                        Height);
                    break;
            }

            if (RightToLeft == RightToLeft.Yes)
            {
                imageRect.X = Width - imageRect.Right;
                textRect.X = Width - textRect.Right;
            }
        }
        internal static TextFormatFlags GetTextFormatFlags(
            ContentAlignment alignment,
            bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 该方法用于提供form的acceptbutton,cancelbutton属性调用，
        /// 要想两个属性生效，button必须是可选（ControlStyles.Selectable），所以这里提供一个设置可选的接口。
        /// </summary>
        /// <param name="canSelect">参数：能否被选中 true-能，false-不能</param>
        public void SetSelect(bool canSelect)
        {
            SetStyle(ControlStyles.Selectable, canSelect);

        }
        #endregion

    }
}
