using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace WSN.Ants.Controls.WSNTabControl
{
    /// <summary>
    /// WSN控件：
    ///     WSNTabControl
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNTabControl
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNTabControlExamples.cs" region="WSNTabControlExample"/>
    /// </example>
    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    public class WSNTabControl : TabControl
    {
        #region 字段
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private SubClass scUpDown = null;
        private bool bUpDown; // true when the button UpDown is required
        //private ImageList leftRightImages = null;
        private const int nMargin = 5;

        private WSNTabControlRender _render;

        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取WSNTabControl的子页面
        /// </summary>
        [Editor(typeof(TabpageExCollectionEditor), typeof(UITypeEditor))]
        public new TabPageCollection TabPages
        {
            get
            {
                return base.TabPages;
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否可以显示一行以上选项卡
        /// </summary>
        [Browsable(false)]
        new public bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = false; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置WSNTabControl的渲染器
        /// </summary>
        public WSNTabControlRender Render
        {
            get
            {
                if (_render == null)
                {
                    _render = new WSNTabControlRender(this);
                }
                return _render;

            }
            set
            {
                _render = value;
                this.Invalidate();
            }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 创建<c>WSNTabControl</c>
        /// </summary>
        public WSNTabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // double buffering

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            bUpDown = false;

            this.ControlAdded += new ControlEventHandler(WSNTabControl_ControlAdded);
            this.ControlRemoved += new ControlEventHandler(WSNTabControl_ControlRemoved);
            this.SelectedIndexChanged += new EventHandler(WSNTabControl_SelectedIndexChanged);

        }
        #endregion


        #region 重载方法
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                //leftRightImages.Dispose();
            }
            base.Dispose(disposing);
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            FindUpDown();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            base.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            base.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawControl(e.Graphics);
        }

        internal void DrawControl(Graphics g)
        {
            if (!Visible)
                return;

            Rectangle TabControlArea = this.ClientRectangle;
            Rectangle TabArea = this.DisplayRectangle;

            // fill client area
            Render.RenderControlBackground(g, TabControlArea);

            // draw panel border
            //这里必须在外面inflate，否则在函数里做inflate会尺寸不对。
            int nDelta = SystemInformation.Border3DSize.Width;
            TabArea.Inflate(nDelta, nDelta);

            Render.RenderTabPanelBorder(g, TabArea);

            // clip region for drawing tabs
            Region rsaved = g.Clip;
            ClipDrawingTabsRegion(g, TabControlArea, TabArea);


            // draw tabs
            Point cursorPoint = PointToClient(MousePosition);
            for (int i = 0; i < this.TabCount; i++)
            {
                Point cusorPoint = PointToClient(MousePosition);
                Rectangle recBounds = this.GetTabRect(i);
                bool bHovered = recBounds.Contains(cusorPoint);

                Render.DrawTab(g, this.TabPages[i], i, bHovered);
            }

            g.Clip = rsaved;


            Render.DrawCoverBorderAreas(g, TabArea);


        }

        private void ClipDrawingTabsRegion(Graphics g, Rectangle TabControlArea, Rectangle TabArea)
        {

            Rectangle rreg = TabControlArea;

            int nWidth = TabArea.Width + nMargin;
            if (bUpDown)
            {
                // exclude updown control for painting
                if (Win32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rupdown = new Rectangle();
                    Win32.GetWindowRect(scUpDown.Handle, ref rupdown);
                    Rectangle rupdown2 = this.RectangleToClient(rupdown);

                    nWidth = rupdown2.X;
                }
            }
            //rreg = new Rectangle(TabArea.Left, TabControlArea.Top, nWidth - nMargin, TabControlArea.Height);

            switch (Alignment)
            {
                case TabAlignment.Top:
                    rreg = new Rectangle(TabArea.Left, TabControlArea.Top, nWidth - nMargin, TabControlArea.Height);
                    break;
                case TabAlignment.Bottom:
                    rreg = new Rectangle(TabArea.Left, TabControlArea.Top, nWidth - nMargin, TabControlArea.Height);
                    break;
                case TabAlignment.Left:
                    rreg = TabControlArea;
                    break;
                case TabAlignment.Right:
                    rreg = TabControlArea;
                    break;

            }

            g.SetClip(rreg);


        }

        #endregion


        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        private void WSNTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }



        private void WSNTabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void WSNTabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void FindUpDown()
        {
            if (this == null || this.IsDisposed)
                return;

            bool bFound = false;
            
            // find the UpDown control
            IntPtr pWnd = Win32.GetWindow(this.Handle, Win32.GW_CHILD);

            while (pWnd != IntPtr.Zero)
            {
                //----------------------------
                // Get the window class name
                char[] className = new char[33];

                int length = Win32.GetClassName(pWnd, className, 32);

                string s = new string(className, 0, length);
                //----------------------------

                if (s == "msctls_updown32")
                {
                    bFound = true;

                    if (!bUpDown)
                    {
                        //----------------------------
                        // Subclass it
                        this.scUpDown = new SubClass(pWnd, true);
                        this.scUpDown.SubClassedWndProc += new SubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);
                        //----------------------------

                        bUpDown = true;
                    }
                    break;
                }

                pWnd = Win32.GetWindow(pWnd, Win32.GW_HWNDNEXT);
            }

            if ((!bFound) && (bUpDown))
                bUpDown = false;
        }

        private void UpdateUpDown()
        {
            if (bUpDown)
            {
                if (Win32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rect = new Rectangle();

                    Win32.GetClientRect(scUpDown.Handle, ref rect);
                    Win32.InvalidateRect(scUpDown.Handle, ref rect, true);
                }
            }
        }


        #region scUpDown_SubClassedWndProc Event Handler

        private int scUpDown_SubClassedWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_PAINT:
                    {
                        //------------------------
                        // redraw
                        IntPtr hDC = Win32.GetWindowDC(scUpDown.Handle);
                        Graphics g = Graphics.FromHdc(hDC);

                        Render.DrawUpDownButtonIcons(g, scUpDown);

                        g.Dispose();
                        Win32.ReleaseDC(scUpDown.Handle, hDC);
                        //------------------------

                        // return 0 (processed)
                        m.Result = IntPtr.Zero;

                        //------------------------
                        // validate current rect
                        Rectangle rect = new Rectangle();

                        Win32.GetClientRect(scUpDown.Handle, ref rect);
                        Win32.ValidateRect(scUpDown.Handle, ref rect);
                        //------------------------
                    }
                    return 1;
            }

            return 0;
        }

        #endregion

        #region TabpageExCollectionEditor

        internal class TabpageExCollectionEditor : CollectionEditor
        {
            public TabpageExCollectionEditor(System.Type type)
                : base(type)
            {
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(TabPage);
            }
        }

        #endregion

    }
}
