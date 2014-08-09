using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WSN.Ants.Controls.WSNTipForm
{
    /// <summary>
    /// WSN控件：
    ///     WSNTipForm
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNTipForm
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNTipFormExamples.cs" region="WSNTipFormExample"/>
    /// </example>
    public partial class WSNTipForm : Form
    {
        #region 常量
        const int WM_MOUSEACTIVATE = 0x21;
        const int MA_NOACTIVATE = 3;

        const int WM_NCHITTEST = 0x0084;
        const int HTCAPTION = 2;
        const int HTCLIENT = 1;



        //
        // non client mouse
        const int WM_NCMOUSEMOVE = 0x00A0;
        const int WM_NCLBUTTONDOWN = 0x00A1;
        const int WM_NCLBUTTONUP = 0x00A2;
        const int WM_NCLBUTTONDBLCLK = 0x00A3;
        const int WM_NCRBUTTONDOWN = 0x00A4;
        const int WM_NCRBUTTONUP = 0x00A5;
        const int WM_NCRBUTTONDBLCLK = 0x00A6;
        const int WM_NCMBUTTONDOWN = 0x00A7;
        const int WM_NCMBUTTONUP = 0x00A8;
        const int WM_NCMBUTTONDBLCLK = 0x00A9;
        #endregion

        #region 字段

        private bool closeBox = true;
        private Size closeBoxOffset = new Size(5, 5);

        private int radius = 4;
        private bool autoClose = false;
        private int interval = 1000;

        private Color borderColor = Color.FromArgb(179, 179, 179);
        private BrushParameter backColorSchma = new BrushParameter(Color.FromArgb(255, 255, 255), Color.FromArgb(221, 221, 221), LinearGradientMode.Vertical);

        public BrushParameter BackColorSchma
        {
            get { return backColorSchma; }
            set { backColorSchma = value; }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置时间间隔（以毫秒为单位）。
        /// </summary>
        /// <remarks>默认为：1000</remarks>
        [DefaultValue("1000")]
        public int Interval
        {
            get { return interval; }
            set { interval = value; timer1.Interval = Interval; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否自动关闭
        /// </summary>
        /// <remarks>默认为：false</remarks>
        [DefaultValue(false)]
        public bool AutoClose
        {
            get { return autoClose; }
            set { autoClose = value; }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置窗体边框样式
        /// </summary>
        public new FormBorderStyle FormBorderStyle
        {
            get
            {
                return base.FormBorderStyle;
            }
            set
            {
                base.FormBorderStyle = FormBorderStyle.None;
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置圆角半径
        /// </summary>
        /// <remarks>值大于等于2，小于2的值会自动更改为2</remarks>
        public int Radius
        {
            get { return radius; }
            set
            {
                if (radius != value)
                {
                    radius = value < 2 ? 2 : value;
                    SetReion();
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置关闭按钮的偏移位置
        /// </summary>
        public Size CloseBoxOffset
        {
            get { return closeBoxOffset; }
            set { closeBoxOffset = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否显示关闭按钮
        /// </summary>
        public bool CloseBox
        {
            get { return closeBox; }
            set { closeBox = value; closeButton.Visible = closeBox; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数：
        ///     创建<c>WSNTipForm</c>
        /// </summary>
        public WSNTipForm()
        {
            InitializeComponent();
            SetStyles();
            Init();
        }
        #endregion

        #region 私有方法
        private void WSNTipForm_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = autoClose;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Close();
        }

        private void SetStyles()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ContainerControl, true);



        }
        private void Init()
        {
            this.ShowInTaskbar = false;
            //this.TopMost = true;

            SetCloseButton();


            base.FormBorderStyle = FormBorderStyle.None;
            base.ControlBox = false;
            base.MaximizeBox = false;
            base.MinimizeBox = false;

        }



        protected void SetCloseButton()
        {
            closeButton.Visible = CloseBox;
            closeButton.Width = 26;
            closeButton.Height = 25;
            closeButton.ImageTable.ImageNormal = new Bitmap(typeof(WSNTipForm), "closeN.png");
            closeButton.ImageTable.ImageHover = new Bitmap(typeof(WSNTipForm), "closeH.png");
            closeButton.ImageTable.ImageChecked = closeButton.ImageTable.ImageHover;
            closeButton.ImageTable.ImageDisalbed = closeButton.ImageTable.ImageNormal;

            closeButton.Location = new Point(this.Width - closeButton.Width - CloseBoxOffset.Width - 5, CloseBoxOffset.Height);

        }

        void closeButton_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            this.Close();
        }
        protected void SetReion()
        {

            if (base.Region != null)
            {
                base.Region.Dispose();
            }
            base.Region = CreateRegion();
        }

        protected Region CreateRegion()
        {
            Rectangle rect = ClientRectangle;

            using (GraphicsPath path = CreatePath(
                rect,
                Radius))
            {
                return new Region(path);
            }
        }

        protected GraphicsPath CreatePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(new Point(rect.X, rect.Y + radius), new Point(rect.X + radius, rect.Y));
            path.AddLine(new Point(rect.X + radius, rect.Y), new Point(rect.Right - radius, rect.Y));
            path.AddLine(new Point(rect.Right - radius, rect.Y), new Point(rect.Right, rect.Y + radius));
            path.AddLine(new Point(rect.Right, rect.Y + radius), new Point(rect.Right, rect.Bottom - radius));
            path.AddLine(new Point(rect.Right, rect.Bottom - radius), new Point(rect.Right - radius, rect.Bottom));
            path.AddLine(new Point(rect.Right - radius, rect.Bottom), new Point(rect.X + radius, rect.Bottom));
            path.AddLine(new Point(rect.X + radius, rect.Bottom), new Point(rect.X, rect.Bottom - radius));
            path.AddLine(new Point(rect.X, rect.Bottom - radius), new Point(rect.X, rect.Y + radius));
            path.CloseFigure();

            return path;
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
            closeButton.Location = new Point(this.Width - closeButton.Width - CloseBoxOffset.Width - 5, CloseBoxOffset.Height);
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = ClientRectangle;
            if (ClientRectangle.Height > 0 && ClientRectangle.Width > 0)
            {
                using (LinearGradientBrush br = new LinearGradientBrush(ClientRectangle, backColorSchma.Color1, backColorSchma.Color2, backColorSchma.Mode))
                {
                    g.FillRectangle(br, rect);
                }
            }
            using (Pen pen = new Pen(borderColor))
            {
                Rectangle r = rect;
                r.Width -= 1; r.Height -= 1;
                using (GraphicsPath path = CreatePath(r, Radius))
                {
                    g.DrawPath(pen, path);
                }
            }

        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = new System.IntPtr(MA_NOACTIVATE);
                return;
            }


            
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if (m.Result == (IntPtr)HTCLIENT)
                        m.Result = (IntPtr)HTCAPTION;
                    
                    break;
                case WM_NCLBUTTONDBLCLK:
                case WM_NCRBUTTONDOWN:
                case WM_NCRBUTTONUP:
                case WM_NCRBUTTONDBLCLK:
                case WM_NCMBUTTONDBLCLK:
                    break;
                default:
                    base.WndProc(ref m);
                    break;

            }

        }
        #endregion





    }

}
