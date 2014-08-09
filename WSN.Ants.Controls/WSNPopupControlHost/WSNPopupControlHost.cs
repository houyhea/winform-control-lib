using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Security.Permissions;
using System.ComponentModel;

namespace WSN.Ants.Controls.WSNPopupControlHost
{
    /// <summary>
    ///  WSN控件：
    ///     WSNPopupControlHost
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNPopupControlHost
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNPopupControlHostExamples.cs" region="WSNPopupControlHostExample"/>
    /// </example>
    public class WSNPopupControlHost : ToolStripDropDown
    {
        #region 字段

        private ToolStripControlHost _controlHost;
        private Control _popupControl;
        private bool _changeRegion;
        private bool _openFocused;
        private bool _acceptAlt;
        private bool _resizableTop;
        private bool _resizableLeft;
        private bool _canResize = false;
        private WSNPopupControlHost _ownerPopup;
        private WSNPopupControlHost _childPopup;
        private Color _borderColor = Color.FromArgb(32, 46, 62);

        #endregion

        #region 构造函数
        /// <summary>
        /// 创建<c>WSNPopupControlHost</c>
        /// </summary>
        /// <param name="control"></param>
        public WSNPopupControlHost(Control control)
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            AutoSize = false;
            Padding = Padding.Empty;
            Margin = Padding.Empty;
            CreateHost(control);
      
        }

        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否刷新区域
        /// </summary>
        public bool ChangeRegion
        {
            get { return _changeRegion; }
            set { _changeRegion = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否接收焦点
        /// </summary>
        public bool OpenFocused
        {
            get { return _openFocused; }
            set { _openFocused = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否接收Alt键
        /// </summary>
        public bool AcceptAlt
        {
            get { return _acceptAlt; }
            set { _acceptAlt = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置是否可以调整大小
        /// </summary>
        public bool CanResize
        {
            get { return _canResize; }
            set { _canResize = value; }
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

        #endregion

        #region 重载方法

        
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (_acceptAlt && ((keyData & Keys.Alt) == Keys.Alt))
            {
                if ((keyData & Keys.F4) != Keys.F4)
                {
                    return false;
                }
                else
                {
                    Close();
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            if (_popupControl.IsDisposed || _popupControl.Disposing)
            {
                e.Cancel = true;
                base.OnOpening(e);
                return;
            }
            _popupControl.RegionChanged += new EventHandler(PopupControlRegionChanged);
            UpdateRegion();
            base.OnOpening(e);
        }

        protected override void OnOpened(EventArgs e)
        {
            if (_openFocused)
            {
                _popupControl.Focus();
            }

            base.OnOpened(e);
        }

        protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            _popupControl.RegionChanged -= new EventHandler(PopupControlRegionChanged);
            base.OnClosing(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (_controlHost != null)
            {
                _controlHost.Size = new Size(
                    Width - Padding.Horizontal, Height - Padding.Vertical);
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (!ProcessGrip(ref m))
            {
                base.WndProc(ref m);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!_changeRegion)
            {
                ControlPaint.DrawBorder(
                    e.Graphics,
                    ClientRectangle,
                    _borderColor,
                    ButtonBorderStyle.Solid);
            }
        }

        protected void UpdateRegion()
        {
            if (!_changeRegion)
            {
                return;
            }

            if (base.Region != null)
            {
                base.Region.Dispose();
                base.Region = null;
            }
            if (_popupControl.Region != null)
            {
                base.Region = _popupControl.Region.Clone();
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 在父容器中显示显示控件
        /// </summary>
        /// <param name="control">父容器</param>
        public void Show(Control control)
        {
            Show(control, control.ClientRectangle);
        }
        /// <summary>
        /// 在父容器中显示显示控件
        /// </summary>
        /// <param name="control">父容器</param>
        /// <param name="center">是否居中</param>
        public void Show(Control control, bool center)
        {
            Show(control, control.ClientRectangle, center);
        }
        /// <summary>
        /// 在父容器中显示显示控件
        /// </summary>
        /// <param name="control">父容器</param>
        /// <param name="rect">显示区域</param>
        public void Show(Control control, Rectangle rect)
        {
            Show(control, rect, false);
        }
        /// <summary>
        /// 在父容器中显示显示控件
        /// </summary>
        /// <param name="control">父容器</param>
        /// <param name="rect">显示区域</param>
        /// <param name="center">是否居中</param>
        public void Show(Control control, Rectangle rect, bool center)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            SetOwnerItem(control);

            if (_canResize && !_changeRegion)
            {
                Padding = new Padding(3);
            }
            else if (!_changeRegion)
            {
                Padding = new Padding(1);
            }
            else
            {
                Padding = Padding.Empty;
            }

            int width = Padding.Horizontal;
            int height = Padding.Vertical;

            base.Size = new Size(
                   _popupControl.Width + width,
                   _popupControl.Height + height);

            _resizableTop = false;
            _resizableLeft = false;
            Point location = control.PointToScreen(
                new Point(rect.Left, rect.Bottom));
            Rectangle screen = Screen.FromControl(control).WorkingArea;
            if (center)
            {
                if (location.X + (rect.Width + Size.Width) / 2 > screen.Right)
                {
                    location.X = screen.Right - Size.Width;
                    _resizableLeft = true;
                }
                else
                {
                    location.X = location.X - (Size.Width - rect.Width) / 2;
                }
            }
            else
            {
                if (location.X + Size.Width > (screen.Left + screen.Width))
                {
                    _resizableLeft = true;
                    location.X = (screen.Left + screen.Width) - Size.Width;
                }
            }

            if (location.Y + Size.Height > (screen.Top + screen.Height))
            {
                _resizableTop = true;
                location.Y -= Size.Height + rect.Height;
            }

            location = control.PointToClient(location);
            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }

        #endregion

        #region Private Methods

        private void SetOwnerItem(Control control)
        {
            if (control == null)
            {
                return;
            }
            if (control is WSNPopupControlHost)
            {
                WSNPopupControlHost popupControl = control as WSNPopupControlHost;
                _ownerPopup = popupControl;
                _ownerPopup._childPopup = this;
                OwnerItem = popupControl.Items[0];
                return;
            }
            if (control.Parent != null)
            {
                SetOwnerItem(control.Parent);
            }
        }

        private void CreateHost(Control control)
        {
            if (control == null)
            {
                throw new ArgumentException("control");
            }

            _popupControl = control;
            _controlHost = new ToolStripControlHost(control, "popupControlHost");
            _controlHost.AutoSize = false;
            _controlHost.Padding = Padding.Empty;
            _controlHost.Margin = Padding.Empty;
            base.Size = new Size(
                control.Size.Width + Padding.Horizontal,
                control.Size.Height + Padding.Vertical);
            base.Items.Add(_controlHost);
        }

        private void PopupControlRegionChanged(object sender, EventArgs e)
        {
            UpdateRegion();
        }

        [SecurityPermission(SecurityAction.LinkDemand,
           Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool ProcessGrip(ref Message m)
        {
            if (_canResize && !_changeRegion)
            {
                switch (m.Msg)
                {
                    case NativeMethods.WM_NCHITTEST:
                        return OnNcHitTest(ref m);
                    case NativeMethods.WM_GETMINMAXINFO:
                        return OnGetMinMaxInfo(ref m);
                }
            }
            return false;
        }

        [SecurityPermission(SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool OnGetMinMaxInfo(ref Message m)
        {
            Control hostedControl = _popupControl;
            if (hostedControl != null)
            {
                NativeMethods.MINMAXINFO minmax =
                    (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(
                    m.LParam, typeof(NativeMethods.MINMAXINFO));

                if (hostedControl.MaximumSize.Width != 0)
                {
                    minmax.maxTrackSize.Width = hostedControl.MaximumSize.Width;
                }
                if (hostedControl.MaximumSize.Height != 0)
                {
                    minmax.maxTrackSize.Height = hostedControl.MaximumSize.Height;
                }

                minmax.minTrackSize = new Size(100, 100);
                if (hostedControl.MinimumSize.Width > minmax.minTrackSize.Width)
                {
                    minmax.minTrackSize.Width =
                        hostedControl.MinimumSize.Width + Padding.Horizontal;
                }
                if (hostedControl.MinimumSize.Height > minmax.minTrackSize.Height)
                {
                    minmax.minTrackSize.Height =
                        hostedControl.MinimumSize.Height + Padding.Vertical;
                }

                Marshal.StructureToPtr(minmax, m.LParam, false);
            }
            return true;
        }

        private bool OnNcHitTest(ref Message m)
        {
            Point location = PointToClient(new Point(
                NativeMethods.LOWORD(m.LParam), NativeMethods.HIWORD(m.LParam)));
            Rectangle gripRect = Rectangle.Empty;
            if (_canResize && !_changeRegion)
            {
                if (_resizableLeft)
                {
                    if (_resizableTop)
                    {
                        gripRect = new Rectangle(0, 0, 6, 6);
                    }
                    else
                    {
                        gripRect = new Rectangle(
                            0,
                            Height - 6,
                            6,
                            6);
                    }
                }
                else
                {
                    if (_resizableTop)
                    {
                        gripRect = new Rectangle(Width - 6, 0, 6, 6);
                    }
                    else
                    {
                        gripRect = new Rectangle(
                            Width - 6,
                            Height - 6,
                            6,
                            6);
                    }
                }
            }

            if (gripRect.Contains(location))
            {
                if (_resizableLeft)
                {
                    if (_resizableTop)
                    {
                        m.Result = (IntPtr)NativeMethods.HTTOPLEFT;
                        return true;
                    }
                    else
                    {
                        m.Result = (IntPtr)NativeMethods.HTBOTTOMLEFT;
                        return true;
                    }
                }
                else
                {
                    if (_resizableTop)
                    {
                        m.Result = (IntPtr)NativeMethods.HTTOPRIGHT;
                        return true;
                    }
                    else
                    {
                        m.Result = (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                        return true;
                    }
                }
            }
            else
            {
                Rectangle rectClient = ClientRectangle;
                if (location.X > rectClient.Right - 3 &&
                    location.X <= rectClient.Right &&
                    !_resizableLeft)
                {
                    m.Result = (IntPtr)NativeMethods.HTRIGHT;
                    return true;
                }
                else if (location.Y > rectClient.Bottom - 3 &&
                    location.Y <= rectClient.Bottom &&
                    !_resizableTop)
                {
                    m.Result = (IntPtr)NativeMethods.HTBOTTOM;
                    return true;
                }
                else if (location.X > -1 &&
                    location.X < 3 &&
                    _resizableLeft)
                {
                    m.Result = (IntPtr)NativeMethods.HTLEFT;
                    return true;
                }
                else if (location.Y > -1 &&
                    location.Y < 3 &&
                    _resizableTop)
                {
                    m.Result = (IntPtr)NativeMethods.HTTOP;
                    return true;
                }
            }
            return false;
        }

        #endregion
    }

    internal class NativeMethods
    {
        public const int WM_GETMINMAXINFO = 0x0024;
        public const int WM_NCHITTEST = 0x0084;

        public const int HTTRANSPARENT = -1;
        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 16;
        public const int HTBOTTOMRIGHT = 17;

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
        }

        public static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }

        public static int HIWORD(IntPtr n)
        {
            int v = n.ToInt32();
            return HIWORD16(v);
            //return HIWORD(unchecked((int)(long)n));
        }

        public static int LOWORD(int n)
        {
            return n & 0xffff;
        }

        public static int LOWORD(IntPtr n)
        {
            int v = n.ToInt32();
            return LOWORD16(v);
            //return LOWORD(unchecked((int)(long)n));
        }


        /// <summary>
        /// 获取低16位，这里要先转化成INT16型，这样才能防止负数不被转换的问题
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 LOWORD16(int value)
        {

            Int16 v = (Int16)(value & 0xFFFF);
            return v;
        }


        /// <summary>
        /// 获取高16位，这里要先转化成INT16型，这样才能防止负数不被转换的问题
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 HIWORD16(int value)
        {
            Int16 v = (Int16)(value >> 16);
            return v;

        }
    }
}
