using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WSN.Ants.Controls.WsnRangeSlideBar
{
    /// <summary>
    /// WSN控件：
    ///     WsnRangeSlideBar
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WsnRangeSlideBar
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WsnRangeSlideBarExamples.cs" region="WsnRangeSlideBarExample"/>
    /// </example>
    public class WsnRangeSlideBar : Control
    {
        protected enum ThumDirection
        {
            /// <summary>
            /// 左
            /// </summary>
            Left,
            /// <summary>
            /// 右
            /// </summary>
            Right,
        }

        #region 字段
        private System.ComponentModel.IContainer components = null;

        private ControlState _controlState;
        private ControlState _thumbState;



        private RoundStyle _roundStyle = RoundStyle.All;
        private int _radius = 12;

        private FaceStyle _thumbArrowSchema = new FaceStyle();

        private FaceStyle _thumbFaceSchema = new FaceStyle();
        private FaceStyle _elapsedBar = new FaceStyle();
        private FaceStyle _barFaceSchema = new FaceStyle();
        private FaceStyle _selecdthumbFaceSchema = new FaceStyle();

        private Rectangle thumbRectL;
        private Rectangle thumbRectR;

        private Rectangle barRect;
        private Rectangle elapsedRect;

        LinearGradientMode gradientOrientation;
        GraphicsPath thumbPathL;
        GraphicsPath thumbPathR;
        private int thumbSize = 12;

        private Orientation _orientation = Orientation.Horizontal;

        private double _minimum = 0;
        private double _maximum = 100;

        private double _valueMin = 0;
        private double _valueMax = 50;

        private int _barHeight = 10;

        private double _smallChange = 10;
        private double _largeChange = 50;

        private bool MoveLMark = false;
        private bool MoveRMark = false;
        private bool MoveMMark = false;

        private bool _regularRange = false;
        private bool _dynamicMove = true; 
        private bool _moveMiddle = true;

        private int PosL = 0, PosR = 0, PosMd = 0, PosMs = 0;
        private int XPosMin, XPosMax, XPosMin_PosM, PosM_XPosMax;
        #endregion

        #region  属性


        /// <summary>
        /// 控件属性：
        ///     获取或设置bar的圆角半径
        /// </summary>
        /// <remarks>默认为：12</remarks>
        [DefaultValue(12)]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value < 0 ? 0 : value;
                    base.Invalidate();
                }
            }
        }

        internal ControlState ThumbState
        {
            get { return _thumbState; }
            set { _thumbState = value; }
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

        /// <summary>
        /// 控件属性：
        ///     获取或设置bar的圆角样式
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
        ///     获取或设置bar的外观样式
        /// </summary>
        public FaceStyle BarFaceSchema
        {
            get { return _barFaceSchema; }
            set { _barFaceSchema = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置thumb箭头外观样式
        /// </summary>
        public FaceStyle ThumbArrowSchema
        {
            get { return _thumbArrowSchema; }
            set { _thumbArrowSchema = value; }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置选择范围条样式
        /// </summary>
        public FaceStyle ElapsedBarSchema
        {
            get { return _elapsedBar; }
            set { _elapsedBar = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置thumb外观样式
        /// </summary>
        public FaceStyle ThumbFaceSchema
        {
            get { return _thumbFaceSchema; }
            set { _thumbFaceSchema = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置选中的thumb外观样式
        /// </summary>
        public FaceStyle SelecdthumbFaceSchema
        {
            get { return _selecdthumbFaceSchema; }
            set { _selecdthumbFaceSchema = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否固定最大最小范围
        /// </summary>
        /// <remarks>默认为：false</remarks>
        [Description("是否固定最大最小范围")]
        [Category("自定义属性")]
        [DefaultValue(false)]
        public bool RegularRange
        {
            get { return _regularRange; }
            set { _regularRange = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否能动态移动
        /// </summary>
        /// <remarks>默认为：true</remarks>
        [Description("是否能动态移动")]
        [Category("自定义属性")]
        [DefaultValue(true)]
        public bool DynamicMove
        {
            get { return _dynamicMove; }
            set { _dynamicMove = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否能通过中间段进行移动
        /// </summary>
        /// <remarks>默认为：true</remarks>
        [Description("是否能通过中间段进行移动")]
        [Category("自定义属性")]
        [DefaultValue(true)]
        public bool MoveMiddle
        {
            get { return _moveMiddle; }
            set { _moveMiddle = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置拖动条的宽度
        /// </summary>
        /// <remarks>默认为：15</remarks>
        /// <exception cref="ArgumentOutOfRangeException">值小于等于0或大于等于滑块宽度的一半时抛出</exception>
        [Description("设置拖动条的宽度")]
        [Category("自定义属性")]
        [DefaultValue(15)]
        public int ThumbSize
        {
            get { return thumbSize; }
            set
            {
                if (value > 0 &
                    value < (_orientation == Orientation.Horizontal ? ClientRectangle.Width : ClientRectangle.Height))
                    thumbSize = value;
                else
                    throw new ArgumentOutOfRangeException(
                        "TrackSize has to be greather than zero and lower than half of Slider width");
                Invalidate();
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置滑块之间最小值
        /// </summary>
        /// <remarks>默认为：0</remarks>
        /// <exception cref="ArgumentOutOfRangeException">值小于0时抛出</exception>
        [Description("滑块之间最小值")]
        [Category("自定义属性")]
        [DefaultValue(0)]
        public double SmallChange
        {
            get { return _smallChange; }
            set
            {
                if (value >= 0)
                {
                    _smallChange = value;

                    Invalidate();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("不能小于0");
                }
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置滑块之间最大值
        /// </summary>
        /// <remarks>默认为：50</remarks>
        /// <exception cref="ArgumentOutOfRangeException">值小于0时抛出</exception>
        [Description("滑块之间最大值")]
        [Category("自定义属性")]
        [DefaultValue(50)]
        public double LargeChange
        {
            get { return _largeChange; }
            set
            {
                if (value >= 0)
                {
                    _largeChange = value;
                    Invalidate();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("不能小于0");
                }
            }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置滑块最小值
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">值小于0时抛出</exception>
        [Description("设置滑块最小值")]
        [Category("自定义属性")]
        public double ValueMin
        {
            get { return _valueMin; }
            set
            {
                if (value >= 0)
                {
                    ValueMinRangeJudge(value);

                    Range2Pos();
                    Invalidate();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("不能小于0");
                }

            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置滑块最大值
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">值小于0时抛出</exception>
        [Description("设置滑块最大值")]
        [Category("自定义属性")]
        public double ValueMax
        {
            get { return _valueMax; }
            set
            {
                if (value >= 0)
                {
                    if (value >= 0)
                    {
                        ValueMaxRangeJudge(value);
                        Range2Pos();
                        Invalidate();
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("不能小于0");
                }

            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置滑块的高度
        /// </summary>
        /// <remarks>默认为：15</remarks>
        /// <exception cref="ArgumentOutOfRangeException">值小于0时抛出</exception>
        [Description("设置滑块的高度")]
        [Category("自定义属性")]
        [DefaultValue(15)]
        public int BarHeight
        {
            get { return _barHeight; }
            set
            {
                if (value >= 0)
                {
                    _barHeight = value;
                    Invalidate();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("不能小于0");
                }

            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置控件的方向
        /// </summary>
        /// <remarks>默认为：水平方向</remarks>
        [Description("设置控件的方向")]
        [Category("自定义属性")]
        [DefaultValue(Orientation.Horizontal)]
        [Browsable(true)]
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                if (_orientation != value)
                {
                    _orientation = value;
                    int temp = Width;
                    Width = Height;
                    Height = temp;

                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置进度条最小值
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">值小于0时抛出</exception>
        [Description("设置最小值")]
        [Category("自定义属性")]
        public double Minimum
        {
            get { return _minimum; }
            set
            {
                if (value >= 0)
                {
                    MinimumRangeJudge(value);
                    Invalidate();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("不能小于0");
                }

            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置进度条最大值
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">值小于0时抛出</exception>
        [Description("设置最大值")]
        [Category("自定义属性")]
        public double Maximum
        {
            get { return _maximum; }
            set
            {
                if (value >= 0)
                {
                    MaximumRangeJudge(value);
                    Invalidate();

                }
                else
                {
                    throw new ArgumentOutOfRangeException("不能小于0");
                }

            }
        }


        #endregion

        #region 事件

        public delegate void RangeChangedEventHandler(object sender, RangeEventArgs e);

        public delegate void RangeChangingEventHandler(object sender, RangeEventArgs e);

        public event RangeChangedEventHandler RangeChanged;

        public event RangeChangingEventHandler RangeChanging;

        public virtual void OnRangeChanged(RangeEventArgs e)
        {
            if (RangeChanged != null)
                RangeChanged(this, e);
        }

        public virtual void OnRangeChanging(RangeEventArgs e)
        {
            if (RangeChanging != null)
                RangeChanging(this, e);
        }

        #endregion

        #region 重载方法

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int MarkHeight;
            // range 左边托控制条移动判断
            int h = this.Height;
            int w = this.Width;

            MarkHeight = ClientRectangle.Height - 3;


            XPosMin = ThumbSize;
            if (_orientation == Orientation.Horizontal)
                XPosMax = w - ThumbSize - 2;
            else
                XPosMax = h - ThumbSize - 2;

            Range2Pos();

            //计算 bar 矩形范围
            barRect = ClientRectangle;

            //计算thumb 矩形范围
            if (_orientation == Orientation.Horizontal)
            {
                barRect = new Rectangle(0, (ClientRectangle.Height) / 3, w - 2, BarHeight);
                gradientOrientation = LinearGradientMode.Horizontal;

                elapsedRect = new Rectangle(PosL - thumbSize / 2, barRect.Y, PosR - PosL + thumbSize / 2, barRect.Height);


                thumbRectL = new Rectangle(PosL - thumbSize, 1, thumbSize, h - 3);

                thumbRectR = new Rectangle(PosR, 1, thumbSize, h - 3);

            }
            else
            {
                barRect = new Rectangle((ClientRectangle.Width) / 3, 0, BarHeight, h - 2);
                gradientOrientation = LinearGradientMode.Horizontal;

                elapsedRect = new Rectangle(barRect.X, PosL - thumbSize / 2, barRect.Width, PosR - PosL + thumbSize / 2);

                thumbRectL = new Rectangle(1, PosL - thumbSize, w - 3, thumbSize);

                thumbRectR = new Rectangle(1, PosR, w - 3, thumbSize);

            }

            thumbPathL = CreateRoundRectPath(thumbRectL);
            thumbPathR = CreateRoundRectPath(thumbRectR);

            RenderBackGround(e);

            RenderBar(e);

            RenderElapsedBar(e);

            RenderThumb(e, thumbPathL, thumbRectL, ThumDirection.Left);

            RenderThumb(e, thumbPathR, thumbRectR, ThumDirection.Right);

        }

        protected virtual GraphicsPath CreateRoundRectPath(Rectangle rect)
        {
            GraphicsPath gp = new GraphicsPath();
            int rad = 8;

            gp.AddLine(rect.Left + rad, rect.Top, rect.Right - rad, rect.Top);
            gp.AddArc(rect.Right - rad, rect.Top, rad, rad, 270, 90);

            gp.AddLine(rect.Right, rect.Top + rad, rect.Right, rect.Bottom - rad);
            gp.AddArc(rect.Right - rad, rect.Bottom - rad, rad, rad, 0, 90);

            gp.AddLine(rect.Right - rad, rect.Bottom, rect.Left + rad, rect.Bottom);
            gp.AddArc(rect.Left, rect.Bottom - rad, rad, rad, 90, 90);

            gp.AddLine(rect.Left, rect.Bottom - rad, rect.Left, rect.Top + rad);
            gp.AddArc(rect.Left, rect.Top, rad, rad, 180, 90);

            return gp;
        }

        /// <summary>
        /// 绘制拖动条
        /// </summary>
        protected virtual void RenderThumb(PaintEventArgs e, GraphicsPath thumbPath, Rectangle thumbRect, ThumDirection dir)
        {
            Color color1 = ThumbFaceSchema.BackNormalStyle.Color1;
            Color color2 = ThumbFaceSchema.BackNormalStyle.Color2;
            Color borderColor = ThumbFaceSchema.BorderNormalStyle.Color1;

            if (Enabled)
            {
                switch (ThumbState)
                {
                    case ControlState.Hover:
                    case ControlState.Pressed:
                        color1 = ThumbFaceSchema.BackHoverStyle.Color1;
                        color2 = ThumbFaceSchema.BackHoverStyle.Color2;
                        borderColor = ThumbFaceSchema.BorderHoverStyle.Color1;
                        break;
                }
            }
            else
            {
                color1 = ThumbFaceSchema.BackDisabledStyle.Color1;
                color2 = ThumbFaceSchema.BackDisabledStyle.Color2;
                borderColor = ThumbFaceSchema.BorderDisabledStyle.Color1;
            }



            if (thumbRectL.Width != 0 && thumbRect.Height != 0)
            {
                using (
                    LinearGradientBrush lgbThumb =
                        new LinearGradientBrush(thumbRect, color1, color2, gradientOrientation))
                {
                    //lgbThumb.WrapMode = WrapMode.TileFlipXY;

                    e.Graphics.FillPath(lgbThumb, thumbPath);

                    using (Pen p = new Pen(borderColor))
                    {
                        e.Graphics.DrawPath(p, thumbPath);
                    }
                }
            }
            RenderThumArrow(e, thumbRect, dir);

        }

        /// <summary>
        /// 绘制选中条
        /// </summary>
        protected virtual void RenderElapsedBar(PaintEventArgs e)
        {
            Color color1 = ElapsedBarSchema.BackNormalStyle.Color1;
            Color color2 = ElapsedBarSchema.BackNormalStyle.Color2;
            Color borderColor = ElapsedBarSchema.BorderNormalStyle.Color1;

            if (Enabled)
            {
                switch (ControlState)
                {
                    case ControlState.Hover:
                    case ControlState.Pressed:
                        color1 = ElapsedBarSchema.BackHoverStyle.Color1;
                        color2 = ElapsedBarSchema.BackHoverStyle.Color2;
                        borderColor = ElapsedBarSchema.BorderHoverStyle.Color1;
                        break;
                }
            }
            else
            {
                color1 = ElapsedBarSchema.BackDisabledStyle.Color1;
                color2 = ElapsedBarSchema.BackDisabledStyle.Color2;
                borderColor = ElapsedBarSchema.BorderDisabledStyle.Color1;
            }
            if (elapsedRect.Width != 0 && elapsedRect.Height != 0)
            {
                using (
                    LinearGradientBrush lgbElapsed =
                        new LinearGradientBrush(elapsedRect, color1, color2, gradientOrientation))
                {
                    //lgbElapsed.WrapMode = WrapMode.TileFlipXY;

                    if (Radius == 0)
                    {
                        e.Graphics.FillRectangle(lgbElapsed, elapsedRect);

                        using (Pen p = new Pen(borderColor))
                        {
                            e.Graphics.DrawRectangle(p, barRect);
                        }

                    }
                    else
                    {
                        RoundStyle r = (Orientation == Orientation.Horizontal) ? RoundStyle.Left : RoundStyle.Top;
                        using (GraphicsPath path = GraphicsPathHelper.CreatePath(elapsedRect, Radius, r, false))
                        {

                            e.Graphics.FillPath(lgbElapsed, path);
                            using (Pen p = new Pen(borderColor))
                            {
                                e.Graphics.DrawPath(p, path);
                            }
                        }
                    }
                }
            }


        }

        /// <summary>
        /// 绘制背景条
        /// </summary>
        /// <param name="rect">背景条坐标</param>
        /// <returns></returns>
        protected virtual void RenderBar(PaintEventArgs e)
        {
            Color color1 = BarFaceSchema.BackNormalStyle.Color1;
            Color color2 = BarFaceSchema.BackNormalStyle.Color2;
            Color borderColor = BarFaceSchema.BorderNormalStyle.Color1;
            if (Enabled)
            {
                switch (ControlState)
                {
                    case ControlState.Hover:
                    case ControlState.Pressed:
                        color1 = BarFaceSchema.BackHoverStyle.Color1;
                        color2 = BarFaceSchema.BackHoverStyle.Color2;

                        borderColor = BarFaceSchema.BorderHoverStyle.Color1;
                        break;
                }
            }
            else
            {
                color1 = BarFaceSchema.BackDisabledStyle.Color1;
                color2 = BarFaceSchema.BackDisabledStyle.Color2;
                borderColor = BarFaceSchema.BorderDisabledStyle.Color1;
            }


            if (barRect.Width != 0 && barRect.Height != 0)
            {
                //draw bar
                using (
                    LinearGradientBrush lgbBar = new LinearGradientBrush(
                        barRect
                        , color1
                        , color2
                        , gradientOrientation)
                    )
                {
                    //Rectangle r = new Rectangle();
                    //lgbBar.WrapMode = WrapMode.TileFlipXY;
                    if (Radius == 0)
                    {
                        e.Graphics.FillRectangle(lgbBar, barRect);
                        using (Pen p = new Pen(borderColor))
                        {
                            e.Graphics.DrawRectangle(p, barRect);
                        }
                    }
                    else
                    {
                        using (GraphicsPath path = GraphicsPathHelper.CreatePath(barRect, Radius, RoundStyle, false))
                        {

                            e.Graphics.FillPath(lgbBar, path);

                            using (Pen p = new Pen(borderColor))
                            {
                                e.Graphics.DrawPath(p, path);
                            }
                        }

                    }


                }
            }

        }

        /// <summary>
        /// 绘制拖动条箭头
        /// </summary>
        /// <param name="rect">拖动条箭头坐标</param>
        /// <returns></returns>
        private void RenderThumArrow(PaintEventArgs e, Rectangle thumbRect, ThumDirection dir)
        {
            Color thumbArrowColor = ThumbArrowSchema.BackNormalStyle.Color1;

            if (Enabled)
            {
                switch (ThumbState)
                {
                    case ControlState.Hover:
                    case ControlState.Pressed:
                        thumbArrowColor = ThumbArrowSchema.BackHoverStyle.Color1;
                        break;
                    case ControlState.Normal:
                        thumbArrowColor = ThumbArrowSchema.BackNormalStyle.Color1;
                        break;
                }
            }
            else
            {
                thumbArrowColor = ThumbArrowSchema.BackDisabledStyle.Color1;
            }
            switch (dir)
            {
                case ThumDirection.Left:
                    {
                        using (GraphicsPath gp = new GraphicsPath())
                        {
                            if (_orientation == Orientation.Horizontal)
                            {
                                gp.AddLine(thumbRect.Left + thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2,
                                thumbRect.Left + thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2 - 4);
                                gp.AddLine(thumbRect.Left + thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2 - 4,
                                    thumbRect.Left + thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2 + 4);
                                gp.AddLine(thumbRect.Left + thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2 + 4,
                                    thumbRect.Left + thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2);

                            }
                            else
                            {
                                gp.AddLine(thumbRect.Left + thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2,
                                thumbRect.Left + thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2 - 4);
                                gp.AddLine(thumbRect.Left + thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2 - 4,
                                    thumbRect.Left + thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2 + 4);
                                gp.AddLine(thumbRect.Left + thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2 + 4,
                                    thumbRect.Left + thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2);
                            }
                            gp.CloseFigure();
                            using (SolidBrush br = new SolidBrush(thumbArrowColor))
                            {
                                e.Graphics.FillPath(br, gp);
                            }
                        }
                    }
                    break;
                case ThumDirection.Right:
                    {
                        using (GraphicsPath gp = new GraphicsPath())
                        {
                            if (_orientation == Orientation.Horizontal)
                            {
                                gp.AddLine(thumbRect.Right - thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2,
                                thumbRect.Right - thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2 - 4);
                                gp.AddLine(thumbRect.Right - thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2 - 4,
                                    thumbRect.Right - thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2 + 4);
                                gp.AddLine(thumbRect.Right - thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2 + 4,
                                    thumbRect.Right - thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2);

                            }
                            else
                            {
                                gp.AddLine(thumbRect.Right - thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2,
                               thumbRect.Right - thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2 - 4);
                                gp.AddLine(thumbRect.Right - thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2 - 4,
                                    thumbRect.Right - thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2 + 4);
                                gp.AddLine(thumbRect.Right - thumbRect.Width / 2 - 2, thumbRect.Top + thumbRect.Height / 2 + 4,
                                    thumbRect.Right - thumbRect.Width / 2 + 2, thumbRect.Top + thumbRect.Height / 2);
                            }
                            gp.CloseFigure();
                            using (SolidBrush br = new SolidBrush(thumbArrowColor))
                            {
                                e.Graphics.FillPath(br, gp);
                            }
                        }
                    }
                    break;
            }
        }

        protected virtual void RenderBackGround(PaintEventArgs e)
        {
            using (SolidBrush br = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(br, ClientRectangle);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.Enabled)
            {
                base.OnMouseDown(e);

                if (thumbRectL.Contains(e.X, e.Y))
                {
                    this.Capture = true;
                    MoveLMark = true;
                    MoveRMark = false;
                    MoveMMark = false;
                    Invalidate(true);
                }
                else if (thumbRectR.Contains(e.X, e.Y))
                {
                    this.Capture = true;
                    MoveLMark = false;
                    MoveRMark = true;
                    MoveMMark = false;
                    Invalidate(true);
                }
                else if (elapsedRect.Contains(e.X, e.Y))
                {
                    this.Capture = true;
                    MoveLMark = false;
                    MoveRMark = false;
                    if (_moveMiddle)
                    {
                        MoveMMark = true;

                        if (_orientation == Orientation.Horizontal)
                            PosMs = e.X;
                        else
                            PosMs = e.Y;

                        XPosMin_PosM = PosMs - PosL;
                        PosM_XPosMax = PosR - PosMs;

                        Invalidate(true);
                    }
                    else
                    {
                        MoveMMark = false;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.Enabled)
            {
                base.OnMouseMove(e);
                RangeEventArgs eEventArgs = new RangeEventArgs();

                if (thumbRectL.Contains(e.X, e.Y) || thumbRectR.Contains(e.X, e.Y))
                {
                    if (this.Orientation == Orientation.Horizontal)
                        this.Cursor = Cursors.Hand;
                    else
                        this.Cursor = Cursors.Hand;
                }
                else if (elapsedRect.Contains(e.X, e.Y))
                {
                    if (this.Orientation == Orientation.Horizontal)
                        this.Cursor = Cursors.Hand;
                    else
                        this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                }

                if (e.Button == MouseButtons.Left)
                {
                    if (MoveLMark)
                    {
                        if (_orientation == Orientation.Horizontal)
                            PosL = e.X;
                        else
                            PosL = e.Y;
                        if (PosL < XPosMin)
                            PosL = XPosMin;
                        if (PosL > XPosMax)
                            PosL = XPosMax;
                        if (PosR < PosL)
                            PosR = PosL;
                        Pos2Range();
                        Invalidate(true);

                    }
                    else if (MoveRMark)
                    {
                        if (_orientation == Orientation.Horizontal)
                            PosR = e.X;
                        else
                            PosR = e.Y;
                        if (PosR > XPosMax)
                            PosR = XPosMax;
                        if (PosR < XPosMin)
                            PosR = XPosMin;
                        if (PosL > PosR)
                            PosL = PosR;
                        Pos2Range();
                        Invalidate(true);
                    }
                    else if (MoveMMark)
                    {
                        if (_dynamicMove)
                        {
                            if (_orientation == Orientation.Horizontal)
                                PosMd = e.X;
                            else
                                PosMd = e.Y;


                            if (PosMd - XPosMin_PosM < XPosMin)
                            {
                                PosMd = XPosMin + XPosMin_PosM;
                                PosL = XPosMin;
                                PosR = XPosMin + XPosMin_PosM;
                            }
                            if (PosMd + PosM_XPosMax > XPosMax)
                            {
                                PosMd = XPosMax - PosM_XPosMax;
                                PosR = XPosMax;
                                PosL = XPosMax - PosM_XPosMax;
                            }

                            Pos2Range();
                            Invalidate(true);
                        }
                    }

                    eEventArgs.ValueMin = _valueMin;
                    eEventArgs.ValueMax = _valueMax;

                    OnRangeChanging(eEventArgs);
                }
            }

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.Enabled)
            {
                base.OnMouseUp(e);

                RangeEventArgs eEventArgs = new RangeEventArgs();
                this.Capture = false;

                MoveLMark = false;
                MoveRMark = false;
                MoveMMark = false;

                Invalidate();

                eEventArgs.ValueMin = _valueMin;
                eEventArgs.ValueMax = _valueMax;

                OnRangeChanged(eEventArgs);
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
        }

        /// <summary>
        ///  由定位坐标转换为实际数值
        /// </summary>
        private void Pos2Range()
        {
            int w;
            int posw;

            if (_orientation == Orientation.Horizontal)
                w = this.Width;
            else
                w = this.Height;
            posw = w - 2 * thumbSize - 2;

            double dMin = Minimum + (double)Math.Round((Maximum - Minimum) * (PosL - XPosMin) / (double)posw);
            double dMax = Minimum + (double)Math.Round((Maximum - Minimum) * (PosR - XPosMin) / (double)posw);

            if (MoveLMark)
            {
                ValueMinRangeJudge(dMin);
            }
            else if (MoveRMark)
            {
                ValueMaxRangeJudge(dMax);
            }
            #region
            else if (MoveMMark)
            {
                PosL = PosMd - XPosMin_PosM;
                PosR = PosMd + PosM_XPosMax;

                _valueMin = Minimum + (double)Math.Round((Maximum - Minimum) * (PosL - XPosMin) / (double)posw);
                _valueMax = Minimum + (double)Math.Round((Maximum - Minimum) * (PosR - XPosMin) / (double)posw);
            }
            #endregion

        }

        /// <summary>
        ///  由实际数值转换为定位坐标
        /// </summary>
        private void Range2Pos()
        {
            int w;
            int posw;

            if (_orientation == Orientation.Horizontal)
                w = this.Width;
            else
                w = this.Height;
            posw = w - 2 * thumbSize - 2;

            PosL = XPosMin + (int)Math.Round((double)posw * (double)(_valueMin - Minimum) / (double)(Maximum - Minimum));
            PosR = XPosMin + (int)Math.Round((double)posw * (double)(_valueMax - Minimum) / (double)(Maximum - Minimum));
        }

        /// <summary>
        /// 最小值边界检测
        /// </summary>
        /// <param name="minimum"></param>
        private void MinimumRangeJudge(double num)
        {
            if (num < 0)
            {
                _minimum = 0;
            }
            else if (num > _valueMin)
            {
                _minimum = num;
            }
            else
            {
                _minimum = num;
            }

            ValueMin = _minimum;
        }

        /// <summary>
        ///  最大值边界检测
        /// </summary>
        /// <param name="minimum"></param>
        private void MaximumRangeJudge(double num)
        {

            if (num < _valueMin)
            {
                _maximum = num;
            }
            else
            {
                _maximum = num;
            }

            ValueMax = _maximum;
        }

        /// <summary>
        /// 左边拖动条边界检测
        /// </summary>
        /// <param name="valueMin">num</param>
        private void ValueMinRangeJudge(double num)
        {
            if (_dynamicMove && _regularRange)
            {
                //a)	当最小值小于（最大值 - 滑动条之间最大值）时， 则最大值等于（最小值 + 滑动条之间最大值），且如果最小值小于最小范围时，则最小值等于最小范围，最大值等于（最小值 + 滑动条之间最大值）。
                //b)	当最小值大于（最大值 - 滑动条之间最小值）时，则最大值等于（最小值 + 滑动条之间最小值），且如果最大值大于最大范围时，则最大值等于最大范围，最小值等于（最大值 -滑动条之间最小值）。
                //c)	其他条件最小值 等于 当前赋值。

                #region 动态移动 & 固定值范围
                if (num < (_valueMax - _largeChange))
                {
                    if (num < _minimum)
                    {
                        num = _minimum;
                    }
                    _valueMin = num;
                    _valueMax = _valueMin + _largeChange;

                }
                else if (num > (_valueMax - _smallChange))
                {
                    if ((num + _smallChange) > _maximum)
                    {
                        _valueMax = _maximum;
                        _valueMin = _valueMax - _smallChange;
                    }
                    else
                    {
                        _valueMin = num;
                        _valueMax = _valueMin + _smallChange;
                    }
                }
                else if (num < _minimum)
                {
                    _valueMin = _minimum;
                }
                else
                {
                    _valueMin = num;
                }
                #endregion
            }
            else if (_dynamicMove && !_regularRange)
            {
                //a)	当最小值大于最大值时， 则最大值等于最小值，直到两者等于最大范围。
                //b)	当最小值小于最小范围时， 则最小值等于最小范围。
                //c)	其他条件最小值 等于 当前赋值， 如果最小值小于最小范围， 则最小值等于最小范围
                #region 动态移动 & 不固定值范围
                if (num > _valueMax)
                {
                    if (num > _maximum)
                    {
                        _valueMax = _valueMin = _maximum;
                    }
                    else
                    {
                        _valueMin = num;
                        _valueMax = _valueMin;
                    }

                }
                else if (num < _minimum)
                {
                    _valueMin = _minimum;
                }
                else
                {
                    _valueMin = num;
                }
                #endregion
            }
            else if (!_dynamicMove && _regularRange)
            {
                //e)	当最小值小于（最大值 - 滑动条之间最大值）时， 则最小值等于（最大值 – 滑动条之间最大值）,如果这时最小值小于最小范围，则等于最小范围。
                //f)	当最小值大于（最大值 - 滑动条之间最小值）时， 则最小值等于（最大值 – 滑动条之间最小值）。
                //c)	其他条件最小值 等于 当前赋值， 如果最小值小于最小范围， 则最小值等于最小范围
                #region 不动态移动 & 固定值范围
                if (num < (_valueMax - _largeChange))
                {
                    if (num < _minimum)
                    {
                        num = _minimum;
                        _valueMin = num;
                    }
                    else
                    {
                        _valueMin = _valueMax - _largeChange;
                    }
                }
                else if (num > (_valueMax - _smallChange))
                {
                    _valueMin = _valueMax - _smallChange;
                }
                else if (num < _minimum)
                {
                    _valueMin = _minimum;
                }
                else
                {
                    _valueMin = num;
                }
                #endregion
            }
            else if (!_dynamicMove && !_regularRange)
            {
                //a)	当最小值大于最大值时， 则最小值等于最大值。
                //b)	当最小值小于最小范围时， 则最小值等于最小范围。
                //c)	其他条件最小值 等于 当前赋值， 如果最小值小于最小范围， 则最小值等于最小范围
                #region 不动态移动 & 不固定值范围
                if (num > _valueMax)
                {
                    _valueMin = _valueMax;
                }
                else if (num < _minimum)
                {
                    _valueMin = _minimum;
                }
                else
                {
                    _valueMin = num;
                }
                #endregion
            }
        }

        /// <summary>
        ///右边拖动条边界检测
        /// </summary>
        /// <param name="valueMax">num</param>
        private void ValueMaxRangeJudge(double num)
        {
            if (_dynamicMove && _regularRange)
            {
                //a)	当最大值大于（最小值 + 滑动条之间最大值）时， 则最小值等于（最大值 - 滑动条之间最大值），且如果最大值大于最大范围时，则最大值等于最大范围，最小值等于（最大值 - 滑动条之间最大值）。
                //b)	当最大值小于（最小值 + 滑动条之间最小值）时，则最小值等于（最大值 - 滑动条之间最小值），且如果最小值小于最小范围时，则最小值等于最小范围，最大值等于（最小值 + 滑动条之间最小值）。
                //c)	其他条件最大值 等于 当前赋值， 如果最大值大于最大范围， 则最大值等于最大范围。

                #region 动态移动 & 固定值范围
                if (num > (_valueMin + _largeChange))
                {
                    if (num > _maximum)
                    {
                        num = _maximum;
                    }
                    _valueMax = num;
                    _valueMin = _valueMax - _largeChange;

                }
                else if (num < (_valueMin + _smallChange))
                {
                    if ((num - _smallChange) < _minimum)
                    {
                        _valueMin = _minimum;
                        _valueMax = _valueMin + _smallChange;
                    }
                    else
                    {
                        _valueMax = num;
                        _valueMin = num - _smallChange;
                    }

                }
                else if (num > _maximum)
                {
                    _valueMax = _maximum;
                }
                else
                {
                    _valueMax = num;
                }
                #endregion
            }
            else if (_dynamicMove && !_regularRange)
            {
                //a)	当最大值小于最小值时， 则最小值等于最大值，直到两者等于最小范围。
                //b)	当最大值大于最大范围时， 则最大值等于最大范围。
                //c)	其他条件最大值 等于 当前赋值， 如果最大值大于最大范围， 则最大值等于最大范围

                #region 动态移动 & 不固定值范围
                if (num < _valueMin)
                {
                    if (num < _minimum)
                    {
                        _valueMin = _valueMax = _minimum;
                    }
                    else
                    {
                        _valueMax = num;
                        _valueMin = _valueMax;
                    }

                }
                else if (num > _maximum)
                {
                    _valueMax = _maximum;
                }
                else
                {
                    _valueMax = num;
                }
                #endregion
            }
            else if (!_dynamicMove && _regularRange)
            {
                //a)	当最大值大于（最小值 + 滑动条之间最大值）时， 则最大值等于（最小值 + 滑动条之间最大值）,如果这时最大值大于最大范围，则等于最大范围。
                //b)	当最大值小于（最小值 + 滑动条之间最小值）时， 则最大值等于（最小值 + 滑动条之间最小值）。
                //c)	其他条件最大值 等于 当前赋值， 如果最大值大于最大范围， 则最大值等于最大范围

                #region 不动态移动 & 固定值范围
                if (num < (_valueMin + _largeChange))
                {
                    if (num > _maximum)
                    {
                        num = _maximum;
                        _valueMax = num;
                    }
                    else
                    {
                        _valueMax = _valueMin + _largeChange;
                    }
                }
                else if (num < (_valueMin + _smallChange))
                {
                    _valueMax = _valueMin + _smallChange;
                }
                else if (num > _maximum)
                {
                    _valueMax = _maximum;
                }
                else
                {
                    _valueMax = num;
                }
                #endregion
            }
            else if (!_dynamicMove && !_regularRange)
            {
                //a)	当最大值小于最小值时， 则最大值等于最小值。
                //b)	当最大值大于最大范围时， 则最大值等于最大范围。
                //c)	其他条件最大值 等于 当前赋值， 如果最大值大于最大范围， 则最大值等于最大范围

                #region 不动态移动 & 不固定值范围
                if (num < _valueMin)
                {
                    _valueMax = _valueMin;
                }
                else if (num > _maximum)
                {
                    _valueMax = _maximum;
                }
                else
                {
                    _valueMax = num;
                }
                #endregion
            }
        }
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        ///     创建<c>WsnRangeSlideBar</c>
        /// </summary>
        /// <param name="min">范围最小值</param>
        /// <param name="max">范围最大值</param>
        public WsnRangeSlideBar(int min, int max)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | /*ControlStyles.Selectable |*/
                     ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
                     ControlStyles.UserPaint, true);
            InitColor();
        }

        /// <summary>
        /// 默认构造函数
        ///     创建<c>WsnRangeSlideBar</c>
        /// </summary>
        /// <remarks>默认为：最小值是30，最大值是300</remarks>
        public WsnRangeSlideBar() : this(30, 300) { }

        /// <summary>
        /// 初始化颜色参数
        /// </summary>
        private void InitColor()
        {
            BackColor = Color.Transparent;


            //箭头
            ThumbArrowSchema.BackNormalStyle.Color1 = Color.FromArgb(86, 86, 86);
            ThumbArrowSchema.BackNormalStyle.Color2 = Color.FromArgb(86, 86, 86);
            ThumbArrowSchema.BackHoverStyle.Color1 = Color.FromArgb(255, 168, 0);
            ThumbArrowSchema.BackHoverStyle.Color2 = Color.FromArgb(255, 168, 0);
            ThumbArrowSchema.BackDisabledStyle.Color1 = Color.FromArgb(100, 100, 100);

            //bar
            BarFaceSchema.BackNormalStyle.Color1 = Color.FromArgb(86, 86, 86);
            BarFaceSchema.BackNormalStyle.Color2 = Color.FromArgb(86, 86, 86);
            BarFaceSchema.BackHoverStyle = BarFaceSchema.BackCheckedStyle = BarFaceSchema.BackNormalStyle;

            BarFaceSchema.BorderNormalStyle.Color1 =
                BarFaceSchema.BorderHoverStyle.Color1 =
                BarFaceSchema.BorderCheckedStyle.Color1 =
                BarFaceSchema.BorderDisabledStyle.Color1 = Color.FromArgb(179, 179, 179);


            //elapse bar
            ElapsedBarSchema.BackNormalStyle.Color1 = Color.FromArgb(255, 168, 0);
            ElapsedBarSchema.BackNormalStyle.Color2 = Color.FromArgb(255, 168, 0);
            ElapsedBarSchema.BackHoverStyle = ElapsedBarSchema.BackCheckedStyle = ElapsedBarSchema.BackNormalStyle;

            ElapsedBarSchema.BorderNormalStyle.Color1 =
                ElapsedBarSchema.BorderHoverStyle.Color1 =
                ElapsedBarSchema.BorderCheckedStyle.Color1 =
                ElapsedBarSchema.BorderDisabledStyle.Color1 = Color.FromArgb(179, 179, 179);


            //thumb
            ThumbFaceSchema.BackNormalStyle.Color1 = Color.FromArgb(255, 255, 255);
            ThumbFaceSchema.BackNormalStyle.Color2 = Color.FromArgb(255, 255, 255);
            ThumbFaceSchema.BackHoverStyle = ThumbFaceSchema.BackCheckedStyle = ThumbFaceSchema.BackNormalStyle;

            ThumbFaceSchema.BorderNormalStyle.Color1 =
                ThumbFaceSchema.BorderHoverStyle.Color1 =
                ThumbFaceSchema.BorderCheckedStyle.Color1 =
                ThumbFaceSchema.BorderDisabledStyle.Color1 = Color.FromArgb(179, 179, 179);


            //selecdthumb
            SelecdthumbFaceSchema.BackNormalStyle.Color1 = Color.FromArgb(50, 50, 50);
            SelecdthumbFaceSchema.BackNormalStyle.Color2 = Color.FromArgb(50, 50, 50);
            SelecdthumbFaceSchema.BackHoverStyle = BarFaceSchema.BackCheckedStyle = BarFaceSchema.BackNormalStyle;

            SelecdthumbFaceSchema.BorderNormalStyle.Color1 =
                SelecdthumbFaceSchema.BorderHoverStyle.Color1 =
                SelecdthumbFaceSchema.BorderCheckedStyle.Color1 =
                SelecdthumbFaceSchema.BorderDisabledStyle.Color1 = Color.FromArgb(179, 179, 179);

        }

        #endregion

        #region 私有方法

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Size = new System.Drawing.Size(300, 30);
            this.ResumeLayout(false);

        }
        #endregion

    }

    /// <summary>
    /// 自定义数据数据类
    /// </summary>
    public class RangeEventArgs : EventArgs
    {
        #region 字段

        private double _valueMin;
        private double _valueMax;

        #endregion

        #region 字段

        /// <summary>
        ///当前最小值
        /// </summary>
        public double ValueMin
        {
            get
            {
                return _valueMin;
            }
            set
            {
                _valueMin = value;
            }
        }

        /// <summary>
        /// 当前最大值
        /// </summary>
        public double ValueMax
        {
            get
            {
                return _valueMax;
            }
            set
            {
                _valueMax = value;
            }
        }

        #endregion
    }
}

