using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using WSN.Ants.Controls.WSNAutoCompleteTextBox;

namespace WSN.Ants.Controls.WSNSearchBox
{
    /// <summary>
    /// WSN控件：
    ///     WSNSearchBox
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNSearchBox
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNSearchBoxExamples.cs" region="WSNSearchBoxExample"/>
    /// </example>
    [DefaultEvent("SearchBtnClick")]
    public class WSNSearchBox : Control
    {
        #region 字段
        private Cursor _oldCursor;
        private WSNAutoCompleteTextBox.WSNAutoCompleteTextBox _textBox;
        private Image _imageNormal;
        private Image _imageHover;
        private Color _borderColor = Color.FromArgb(38, 128, 160);
        private Color _searchBtnHoverBackColor = Color.FromArgb(255, 214, 125);

        private ControlState _state;
        private Size _imageSize = new Size(16, 16);
        private int _imageRightOffset = 10;


        private int _radius = 0;
        private RoundStyle _roundStyle = RoundStyle.All;


        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置输入是否只读
        /// </summary>
        /// <remarks>默认为：false</remarks>
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return _textBox.ReadOnly; }
            set { _textBox.ReadOnly = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置子项集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public System.Windows.Forms.ListBox.ObjectCollection Items
        {
            get { return _textBox.Items; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置图片大小
        /// </summary>
        /// <remarks>默认为："16,16"</remarks>
        [DefaultValue(typeof(Size), "16,16")]
        public Size ImageSize
        {
            get { return _imageSize; }
            set { _imageSize = value; }
        }
        /// <summary>
        /// 控件属性：
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
        /// 控件属性：
        ///     获取或设置bar的圆角半径
        /// </summary>
        /// <remarks>默认为：0</remarks>
        [DefaultValue(0)]
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
        /// <summary>
        /// 控件属性：
        ///     获取图片搜索按钮的显示区域
        /// </summary>
        public Rectangle ImageButtonRect
        {
            get
            {

                Rectangle r = new Rectangle();
                r.X = SearchBtnRectangle.X + (SearchBtnRectangle.Width - ImageSize.Width) / 2;
                r.Y = SearchBtnRectangle.Y + (SearchBtnRectangle.Height - ImageSize.Height) / 2;
                r.Width = ImageSize.Width;
                r.Height = ImageSize.Height;
                return r;
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置搜索框的显示区域
        /// </summary>
        public Rectangle SearchBtnRectangle
        {
            get
            {

                Rectangle r = ClientRectangle;
                r.X = ClientRectangle.Right - ImageSize.Width - _imageRightOffset;
                r.Y = ClientRectangle.Y + 1;
                r.Width = ImageSize.Width + _imageRightOffset - 2;
                r.Height = ClientRectangle.Height - 3;

                return r;

            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置正常显示图片
        /// </summary>
        public Image ImageNormal
        {
            get { return _imageNormal; }
            set { _imageNormal = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置悬停显示图片
        /// </summary>
        public Image ImageHover
        {
            get { return _imageHover; }
            set { _imageHover = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置搜索按钮悬停时的背景色
        /// </summary>
        public Color SearchBtnHoverBackColor
        {
            get { return _searchBtnHoverBackColor; }
            set { _searchBtnHoverBackColor = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置当文本内容为空时显示的文本信息
        /// </summary>
        /// <remarks>默认为："请输入"</remarks>
        [DefaultValue(typeof(String), "请输入")]
        public String EmptyText
        {
            get { return _textBox.EmptyTextTip; }
            set { _textBox.EmptyTextTip = value; _textBox.Invalidate(); }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置搜索输入文本
        /// </summary>
        public new String Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; _textBox.Invalidate(); }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置搜索输入框空格键是否有效
        /// </summary>
        public  bool IsSpaceKeyCodeValid
        {
            get { return _textBox.IsSpaceKeyCodeValid; }
            set { _textBox.IsSpaceKeyCodeValid= value;  }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置搜素输入自动完成下拉列表触发延时。单位：毫秒。范围：0《 InputChangeTriggerTime 《  5000
        /// </summary>
        public int InputChangeTriggerTime
        {
            get { return _textBox.InputChangeTriggerTime; }
            set
            {
                _textBox.InputChangeTriggerTime = value;
                
            }
        }

        internal ControlState ControlState
        {
            get { return _state; }
            set { _state = value; }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 事件：
        ///     搜索按钮点击事件，搜索按钮被点击时触发
        /// </summary>
        public event EventHandler SearchBtnClick;
        /// <summary>
        /// 事件：
        ///     搜索文本改变事件，搜索框内文本改变时触发，EmptyText信息变化不计算在内
        /// </summary>
        public event EventHandler SearchTextChanged;


        /// <summary>
        /// 事件：
        ///     自动完成列表ITEM自绘前事件（这里设置显示的TEXT）
        /// </summary>
        [Description("自动完成列表ITEM自绘前事件（这里设置显示的TEXT）")]
        public event EventHandler<WSN.Ants.Controls.WSNAutoCompleteTextBox.WSNAutoCompleteTextBox.WSNAutoCompleteListBoxDrawingItemEventArgs> ListDrawingItem
        {
            add
            {
                if (_textBox != null)
                {
                    _textBox.ListDrawingItem += value;
                }
            }
            remove
            {
                if (_textBox != null)
                {
                    _textBox.ListDrawingItem -= value;
                }
            }
        }
        /// <summary>
        /// 事件：
        ///     下拉列表选择项填充到自动完成文本事件
        /// </summary>
        [Description("自动完成列表ITEM自绘前事件（这里设置显示的TEXT）")]
        public event EventHandler<SelectItemEventArgs> FillingAutoCompleteTextEvent
        {
            add
            {
                if (_textBox != null)
                {
                    _textBox.FillingAutoCompleteTextEvent += value;
                }
            }
            remove
            {
                if (_textBox != null)
                {
                    _textBox.FillingAutoCompleteTextEvent -= value;
                }
            }
        }
        #endregion

        #region 构造函数


        public WSNSearchBox()
            : base()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.ResizeRedraw, true);
            Init();
            ReadOnly = false;
            
        }

        private void Init()
        {
            _imageNormal = new Bitmap(typeof(WSNSearchBox), "searchN.png");
            _imageHover = new Bitmap(typeof(WSNSearchBox), "searchH.png");

            _textBox = new WSNAutoCompleteTextBox.WSNAutoCompleteTextBox();
            _textBox.BorderStyle = BorderStyle.None;
            _textBox.InputChanged += new EventHandler(_textBox_InputChanged);
            _textBox.EnterKeyDown += new EventHandler(_textBox_EnterKeyDown);
            SetTextBoxBounds();
            _textBox.BackColor = Color.White;

            base.Controls.Add(_textBox);
            base.BackColor = Color.Transparent;
            _oldCursor = this.Cursor;
        }

        void _textBox_EnterKeyDown(object sender, EventArgs e)
        {
            //触发搜索按钮事件
            if (SearchBtnClick != null)
                SearchBtnClick(this, new EventArgs());
        }

        void _textBox_InputChanged(object sender, EventArgs e)
        {
            //触发输入文本值改变事件
            if (SearchTextChanged != null)
                SearchTextChanged(sender, e);
        }
        #endregion

        #region 重载方法
        private void SetTextBoxBounds()
        {
            _textBox.Location = new Point(5, (Height - 2 - _textBox.Height) / 2);
            _textBox.Width = Width - ImageSize.Width - 5 - _imageRightOffset;

        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            SetTextBoxBounds();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetTextBoxBounds();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _oldCursor = this.Cursor;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (SearchBtnRectangle.Contains(e.Location))
            {

                ControlState = ControlState.Hover;
                Cursor = Cursors.Hand;

            }
            else
            {
                ControlState = ControlState.Normal;
                Cursor = _oldCursor;

            }
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (SearchBtnRectangle.Contains(e.Location))
            {

                ControlState = ControlState.Pressed;


            }
            else
            {
                ControlState = ControlState.Normal;
            }
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                if (SearchBtnRectangle.Contains(e.Location))
                {
                    ControlState = ControlState.Hover;


                    //触发搜索按钮事件
                    if (SearchBtnClick != null)
                        SearchBtnClick(this, new EventArgs());
                }
                else
                {
                    ControlState = ControlState.Normal;
                }


            }
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlState = ControlState.Normal;

            Cursor = _oldCursor;

            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Image img = _imageNormal;
            switch (ControlState)
            {
                case ControlState.Hover:
                case ControlState.Pressed:
                    img = _imageHover;
                    break;

            }
            if (img != null)
            {
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;

                e.Graphics.DrawImage(img, ImageButtonRect.X, ImageButtonRect.Y, ImageButtonRect.Width, ImageButtonRect.Height);

            }

        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (Radius == 0)
            {
                using (SolidBrush br = new SolidBrush(BackColor))
                {
                    e.Graphics.FillRectangle(br, ClientRectangle);
                }
                using (Pen p = new Pen(BorderColor))
                {
                    e.Graphics.DrawRectangle(p, new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1));
                }
            }
            else
            {
                using (GraphicsPath path = GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, true))
                {
                    using (SolidBrush br = new SolidBrush(BackColor))
                    {
                        e.Graphics.FillPath(br, path);
                    }
                    using (Pen pen = new Pen(BorderColor))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }

            if (ControlState == ControlState.Hover || ControlState == ControlState.Pressed)
            {
                using (SolidBrush br = new SolidBrush(_searchBtnHoverBackColor))
                {
                    e.Graphics.FillRectangle(br, SearchBtnRectangle);
                }
            }

        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_textBox != null && _textBox.IsDisposed)
                {
                    _textBox.Dispose();
                    _textBox = null;
                }
            }
        }
        #endregion


    }
}
