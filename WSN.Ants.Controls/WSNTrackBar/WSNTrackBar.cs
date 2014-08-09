using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WSN.Ants.Controls.WSNTrackBar
{
    /// <summary>
    /// WSN控件：
    ///     WSNTrackBar进度条
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNTrackBar
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNTrackBarExamples.cs" region="WSNTrackBarExample"/>
    /// </example>
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent("Scroll")]
    public class WSNTrackBar : Control
    {

        #region 字段
        private System.ComponentModel.IContainer components = null;

        private ControlState _controlState;
        private ControlState _thumbState;



        private RoundStyle _roundStyle = RoundStyle.All;
        private int _radius = 12;


        private FaceStyle _thumbFaceSchema = new FaceStyle();
        private FaceStyle _elapsedBar = new FaceStyle();
        private FaceStyle _barFaceSchema = new FaceStyle();

        private Rectangle thumbRect; //bounding rectangle of thumb area
        private Rectangle barRect; //bounding rectangle of bar area
        private Rectangle elapsedRect; //bounding rectangle of elapsed area

        LinearGradientMode gradientOrientation;
        GraphicsPath thumbPath;
        private int thumbSize = 15;


        private Orientation _orientation = Orientation.Horizontal;


        private int _value = 50;


        private int _minimum = 0;


        private int _maximum = 100;

        private uint smallChange = 1;

        private uint largeChange = 5;







        private int mouseWheelBarPartitions = 10;




        #endregion

        #region 属性

        /// <summary>
        /// 控件属性 ：
        ///     获取或设置bar的圆角样式
        /// </summary>
        /// <remarks>默认为：四个角都是圆角</remarks>
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
        /// 控件属性 ：
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
        /// 控件属性 ：
        ///     获取或设置进度条外观样式
        /// </summary>
        public FaceStyle BarFaceSchema
        {
            get { return _barFaceSchema; }
            set { _barFaceSchema = value; }
        }
        /// <summary>
        /// 控件属性 ：
        ///     获取或设置已完成进度部分外观配置
        /// </summary>
        public FaceStyle ElapsedBarSchema
        {
            get { return _elapsedBar; }
            set { _elapsedBar = value; }
        }
        /// <summary>
        /// 控件属性 ：
        ///     获取或设置thumb外观配置
        /// </summary>
        public FaceStyle ThumbFaceSchema
        {
            get { return _thumbFaceSchema; }
            set { _thumbFaceSchema = value; }
        }

        /// <summary>
        /// 控件属性 ：
        ///     获取或设置thumb区域
        /// </summary>
        [Browsable(false)]
        public Rectangle ThumbRect
        {
            get { return thumbRect; }
        }
        /// <summary>
        /// 控件属性 ：
        ///     获取或设置thumb大小
        /// </summary>
        /// <remarks>默认为：15</remarks>
        [Description("Set Slider thumb size")]
        [Category("ColorSlider")]
        [DefaultValue(15)]
        public int ThumbSize
        {
            get { return thumbSize; }
            set
            {
                if (value > 0 &
                    value < (Orientation == Orientation.Horizontal ? ClientRectangle.Width : ClientRectangle.Height))
                    thumbSize = value;
                else
                    throw new ArgumentOutOfRangeException(
                        "TrackSize has to be greather than zero and lower than half of Slider width");
                Invalidate();
            }
        }
        /// <summary>
        /// 控件属性 ：
        ///     获取或设置控件方向
        /// </summary>
        /// <remarks>默认为：水平方向</remarks>
        [Description("Set Slider orientation")]
        [Category("ColorSlider")]
        [DefaultValue(Orientation.Horizontal)]
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
        /// 控件属性 ：
        ///     获取或设置控件的当前值
        /// </summary>
        /// <remarks>默认为：50</remarks>
        [Description("Set Slider value")]
        [Category("ColorSlider")]
        [DefaultValue(50)]
        public int Value
        {
            get { return _value; }
            set
            {
                if (value >= _minimum & value <= _maximum)
                {
                    _value = value;
                    if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性 ：
        ///     获取或设置进度条的最小值
        /// </summary>
        /// <remarks>默认为：0</remarks>
        [Description("Set Slider minimal point")]
        [Category("ColorSlider")]
        [DefaultValue(0)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (value < _maximum)
                {
                    _minimum = value;
                    if (_value < _minimum)
                    {
                        _value = _minimum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性 ：
        ///     获取或设置进度条的最大值
        /// </summary>
        /// <remarks>默认为：100</remarks>
        [Description("Set Slider maximal point")]
        [Category("ColorSlider")]
        [DefaultValue(100)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (value > _minimum)
                {
                    _maximum = value;
                    if (_value > _maximum)
                    {
                        _value = _maximum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性 ：
        ///     获取或设置进度条一次的最小改变量
        /// </summary>
        /// <remarks>默认为：1</remarks>
        [Description("Set trackbar's small change")]
        [Category("ColorSlider")]
        [DefaultValue(1)]
        public uint SmallChange
        {
            get { return smallChange; }
            set { smallChange = value; }
        }
        /// <summary>
        /// 控件属性 ：
        ///     获取或设置进度条一次的最大改变量
        /// </summary>
        /// <remarks>默认为：5</remarks>
        [Description("Set trackbar's large change")]
        [Category("ColorSlider")]
        [DefaultValue(5)]
        public uint LargeChange
        {
            get { return largeChange; }
            set { largeChange = value; }
        }

        /// <summary>
        /// 控件属性 ：
        ///     获取或设置鼠标滚轮转动单位
        /// </summary>
        /// <exception cref="T:System.ArgumentOutOfRangeException">当值小于0时抛出</exception>
        /// <remarks>默认为：10</remarks>
        [Description("Set to how many parts is bar divided when using mouse wheel")]
        [Category("ColorSlider")]
        [DefaultValue(10)]
        public int MouseWheelBarPartitions
        {
            get { return mouseWheelBarPartitions; }
            set
            {
                if (value > 0)
                    mouseWheelBarPartitions = value;
                else
                    mouseWheelBarPartitions = 1;

            }
        }




        #endregion

        #region 事件


        [Description("Event fires when the Value property changes")]
        [Category("Action")]
        public event EventHandler ValueChanged;


        [Description("Event fires when the Slider position is changed")]
        [Category("Behavior")]
        public event ScrollEventHandler Scroll;

        #endregion


        #region 构造函数

        /// <summary>
        /// 构造函数：
        ///     创建<c>WSNTrackBar</c>
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="value">当前值</param>
        public WSNTrackBar(int min, int max, int value)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | /*ControlStyles.Selectable |*/
                     ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
                     ControlStyles.UserPaint, true);


            Minimum = min;
            Maximum = max;
            Value = value;

            InitColor();
        }

        private void InitColor()
        {
            BackColor = Color.Transparent;

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


        }

        /// <summary>
        /// 默认构造函数：
        ///     创建<c>WSNTrackBar</c>
        /// </summary>
        /// <remarks>默认为：最小值0，最大值100，当前值50</remarks>
        public WSNTrackBar() : this(0, 100, 50) { }

        #endregion


        #region 重载方法
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //计算thumb 矩形范围
            double rate = (double)(_value - _minimum) / (double)(_maximum - _minimum);
            if (Orientation == Orientation.Horizontal)
            {
                double Track = (ClientRectangle.Width - thumbSize) * rate;
                int TrackX = (int)Track;

                thumbRect = new Rectangle(TrackX, 1, thumbSize - 1, ClientRectangle.Height - 3);
            }
            else
            {
                double TrackY = ClientRectangle.Height - thumbSize - (ClientRectangle.Height - thumbSize) * rate;
                //int TrackY = ClientRectangle.Height-(((_value - _minimum) * (ClientRectangle.Height - thumbSize)) / (_maximum - _minimum));
                thumbRect = new Rectangle(1, (int)TrackY, ClientRectangle.Width - 3, thumbSize - 1);
            }

            //计算 bar 矩形范围
            barRect = ClientRectangle;

            if (Orientation == Orientation.Horizontal)
            {
                barRect.Inflate(-1, -barRect.Height / 3);
                gradientOrientation = LinearGradientMode.Horizontal;
                elapsedRect = barRect;
                elapsedRect.Width = thumbRect.Left + thumbSize / 2;
            }
            else
            {
                barRect.Inflate(-barRect.Width / 3, -1);
                gradientOrientation = LinearGradientMode.Vertical;
                elapsedRect = barRect;

                elapsedRect.Y = thumbRect.Top + thumbSize / 2;
                elapsedRect.Height = barRect.Height - thumbRect.Bottom + thumbSize / 2;//改了    ok
            }

            //get thumb shape path 

            thumbPath = CreateRoundRectPath(thumbRect);


            RenderBackGround(e);

            RenderBar(e);

            RenderElapsedBar(e);

            RenderThumb(e);




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
        protected virtual void RenderThumb(PaintEventArgs e)
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
            if (thumbRect.Width > 0 && thumbRect.Height > 0)
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
        }

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
            if (elapsedRect.Width > 0 && elapsedRect.Height > 0)
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
                        RoundStyle r = (Orientation == Orientation.Horizontal) ? RoundStyle.Left : RoundStyle.Bottom;
                        //RoundStyle r = (Orientation == Orientation.Horizontal) ? RoundStyle.Left : RoundStyle.Top;
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
        /// 画bar条
        /// </summary>
        /// <param name="e"></param>
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


            if (barRect.Width > 0 && barRect.Height > 0)
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
                    Rectangle r = new Rectangle();
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
        /// 画背景
        /// </summary>
        /// <param name="e"></param>
        protected virtual void RenderBackGround(PaintEventArgs e)
        {
            using (SolidBrush br = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(br, ClientRectangle);
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ControlState = ControlState.Hover;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlState = ControlState.Normal;
            ThumbState = ControlState.Normal;

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                Capture = true;
                if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, _value));
                if (ValueChanged != null) ValueChanged(this, new EventArgs());
                OnMouseMove(e);
            }
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool bInThumb = IsPointInRect(e.Location, thumbRect);

            ThumbState = bInThumb ? ControlState.Hover : ControlState.Normal;

            if (Capture & e.Button == MouseButtons.Left)
            {
                ScrollEventType set = ScrollEventType.ThumbPosition;
                Point pt = e.Location;
                int p = Orientation == Orientation.Horizontal ? pt.X : ClientSize.Height - pt.Y;//改了OK
                int margin = thumbSize >> 1;
                p -= margin;
                float coef = (float)(_maximum - _minimum) /
                             (float)
                             ((Orientation == Orientation.Horizontal ? ClientSize.Width : ClientSize.Height) -
                              2 * margin);
                _value = (int)(p * coef + _minimum);

                if (_value <= _minimum)
                {
                    _value = _minimum;
                    set = ScrollEventType.First;
                }
                else if (_value >= _maximum)
                {
                    _value = _maximum;
                    set = ScrollEventType.Last;
                }

                if (Scroll != null) Scroll(this, new ScrollEventArgs(set, _value));
                if (ValueChanged != null) ValueChanged(this, new EventArgs());
            }
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Capture = false;
            bool bInThumb = IsPointInRect(e.Location, thumbRect);
            ThumbState = bInThumb ? ControlState.Hover : ControlState.Normal;

            if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.EndScroll, _value));
            if (ValueChanged != null) ValueChanged(this, new EventArgs());
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int v = e.Delta / 120 * (_maximum - _minimum) / mouseWheelBarPartitions;
            SetProperValue(Value - v);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Left:
                    SetProperValue(Value - (int)smallChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.SmallDecrement, Value));
                    break;
                case Keys.Up:
                case Keys.Right:
                    SetProperValue(Value + (int)smallChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.SmallIncrement, Value));
                    break;
                case Keys.Home:
                    Value = _minimum;
                    break;
                case Keys.End:
                    Value = _maximum;
                    break;
                case Keys.PageDown:
                    SetProperValue(Value - (int)largeChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.LargeDecrement, Value));
                    break;
                case Keys.PageUp:
                    SetProperValue(Value + (int)largeChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.LargeIncrement, Value));
                    break;
            }
            if (Scroll != null && Value == _minimum) Scroll(this, new ScrollEventArgs(ScrollEventType.First, Value));
            if (Scroll != null && Value == _maximum) Scroll(this, new ScrollEventArgs(ScrollEventType.Last, Value));
            Point pt = PointToClient(Cursor.Position);
            OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, pt.X, pt.Y, 0));
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab | ModifierKeys == Keys.Shift)
                return base.ProcessDialogKey(keyData);
            else
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
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

            this.Size = new System.Drawing.Size(200, 30);
            this.ResumeLayout(false);

        }
        /// <summary>
        /// Sets the trackbar value so that it wont exceed allowed range.
        /// </summary>
        /// <param name="val">The value.</param>
        private void SetProperValue(int val)
        {
            if (val < _minimum) Value = _minimum;
            else if (val > _maximum) Value = _maximum;
            else Value = val;
        }

        /// <summary>
        /// Determines whether rectangle contains given point.
        /// </summary>
        /// <param name="pt">The point to test.</param>
        /// <param name="rect">The base rectangle.</param>
        /// <returns>
        /// 	<c>true</c> if rectangle contains given point; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPointInRect(Point pt, Rectangle rect)
        {
            if (pt.X > rect.Left & pt.X < rect.Right & pt.Y > rect.Top & pt.Y < rect.Bottom)
                return true;
            else return false;
        }

        #endregion
    }
}
