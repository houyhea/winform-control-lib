using System;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

#region Author/About
/************************************************************************************
*  ToolTip      ToolTip Extension Class                                             *
*                                                                                   *
*  Created:     November 22, 2008                                                   *
*  Purpose:     ToolTip Extender (vhGrid member class)                              *
*  Revision:    1.1                                                                 *
*  IDE:         C# 2005 SP1                                                         *
*  Referenced:  Control Class NSP                                                   *
*  Author:      John Underhill (Steppenwolfe)                                       *
*                                                                                   *
*************************************************************************************

You can not:
Sell or redistribute this code or the binary as a usercontrol for profit.
Use this in spyware, malware, or any generally acknowledged form of malicious software.
Remove or alter the above author accreditation, or this disclaimer.

You can:
Use this code in your applications in any way you like.
You can use this in a published program, (a credit to nspowertools.com would be nice)

I will not:
Except any responsibility for this code whatsoever. 
There is no guarantee of fitness, nor should you have any expectation of support. 
I further renounce any and all responsibilities for this code, in every way conceivable, 
now, and for the rest of time. (clear enough?)

If you desperately need some customization, look me up on rentacoder, otherwise.. do it yourself.

Cheers,
John
steppenwolfe_2000@yahoo.com

***Revision History***
-> 1.1
Changed incorrect field in RightToLeft property
Fixed gdi leaks in: drawGraphic, calcTextSize, and tipCapture
There is a leak when using a custom icon in default mode in Vista that can not be fixed
There looks to be a 1 leak per show when using graphic, but hdc is deleted, probably requires gc op
Completed finalizer
Changed mirror edge gradients to solid brushes
 * 
URL: http://www.codeproject.com/KB/miscctrl/ToolTip_Class.aspx
*/
#endregion

namespace WSN.Ants.Controls.WSNToolTips
{
    #region Enums
    /// <summary>
    /// Tip类型
    /// </summary>
    public enum TipStyle
    {
        Default = 0,
        Solid,
        Gradient,
        Mirror,
        Glass,
        Graphical,
        OwnerDrawn
    }
    /// <summary>
    /// 渐变样式
    /// </summary>
    public enum GradientStyle
    {
        LinearVertical = 0,
        LinearHorizontal,
        BackwardDiagonal,
        ForwardDiagonal,
        HorizontalTube,
        VerticalTube,
        FloatingBoxed,
        BlendTriangular,
        SigmaBellShape
    }
    /// <summary>
    /// 淡入淡出样式
    /// </summary>
    public enum FadeStyle
    {
        Default,
        Phaser,
        Pulse
    }
    /// <summary>
    /// 系统图标
    /// </summary>
    public enum OsIcon
    {
        None = 0,
        SecurityLarge = 32518,
        FileLarge = 32512,
        InfoLarge = 32516,
        WarningLarge = 32515,
        ErrorLarge = 32513,
        HelpLarge = 32514
    }
    /// <summary>
    /// Tip显示位置
    /// </summary>
    public enum TipPosition
    {
        AutoPosition = 0,
        TopLeft,
        TopCenter,
        TopRight,
        RightCenter,
        BottomRight,
        BottomCenter,
        BottomLeft,
        LeftCenter
    }

    internal enum TRACKMOUSEEVENT_FLAGS : uint
    {
        TME_HOVER = 1,
        TME_LEAVE = 2,
        TME_QUERY = 0x40000000,
        TME_CANCEL = 0x80000000
    }
    #endregion

    /// <summary>
    /// WSN控件：
    ///     WSNToolTip
    /// </summary>
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class WSNToolTip : NativeWindow
    {
        #region Constants
        // misc
        private const int ICC_TAB_CLASSES = 0x00000008;
        private const int NM_FIRST = 0;
        private const int NM_CUSTOMDRAW = (NM_FIRST - 12);
        private const int CDDS_PREPAINT = 0x1;
        private const int CDDS_POSTPAINT = 0x2;
        private const int CDDS_PREERASE = 0x3;
        private const int CDDS_POSTERASE = 0x4;
        private const int CDDS_ITEM = 0x10000;
        private const int CDDS_ITEMPREPAINT = (CDDS_ITEM | CDDS_PREPAINT);
        private const int CDDS_ITEMPOSTPAINT = (CDDS_ITEM | CDDS_POSTPAINT);
        private const int CDDS_ITEMPREERASE = (CDDS_ITEM | CDDS_PREERASE);
        private const int CDDS_ITEMPOSTERASE = (CDDS_ITEM | CDDS_POSTERASE);
        private const int CDDS_SUBITEM = 0x20000;

        private const int CDRF_DODEFAULT = 0x0;
        private const int CDRF_NEWFONT = 0x2;
        private const int CDRF_SKIPDEFAULT = 0x4;
        private const int CDRF_NOTIFYPOSTPAINT = 0x10;
        private const int CDRF_NOTIFYITEMDRAW = 0x20;
        private const int CDRF_NOTIFYSUBITEMDRAW = 0x20;
        private const int CDRF_NOTIFYPOSTERASE = 0x40;
        private const int CDRF_NOTIFYITEMERASE = 0x80;
        static readonly IntPtr LPSTR_TEXTCALLBACK = new IntPtr(-1);
        static readonly IntPtr RETURN_TRUE = new IntPtr(1);
        static readonly IntPtr RETURN_FALSE = new IntPtr(0);
        private const int HOVER_SIGNALED = 3;
        private const int FADER_SIGNALED = 6;
        private const int TIMER_FADE = 77;
        private const byte AC_SRC_OVER = 0x00;
        private const byte AC_SRC_ALPHA = 0x01;
        // icon
        private const int TTI_NONE = 0;
        private const int TTI_INFO = (32512);
        private const int TTI_WARNING = 2;
        private const int TTI_ERROR = 3;
        private const int TTI_INFO_LARGE = 4;
        private const int TTI_WARNING_LARGE = 5;
        private const int TTI_ERROR_LARGE = 6;
        // setwindowpos
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        // size/move
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOREDRAW = 0x0008;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_HIDEWINDOW = 0x0080;
        private const uint SWP_NOCOPYBITS = 0x0100;
        private const uint SWP_NOOWNERZORDER = 0x0200;
        private const uint SWP_NOSENDCHANGING = 0x0400;
        // class
        private const string TOOLTIPS_CLASS = "tooltips_class32";
        // format
        private const int TTF_IDISHWND = 0x0001;
        private const int TTF_CENTERTIP = 0x0002;
        private const int TTF_RTLREADING = 0x0004;
        private const int TTF_SUBCLASS = 0x0010;
        private const int TTF_TRACK = 0x0020;
        private const int TTF_ABSOLUTE = 0x0080;
        private const int TTF_TRANSPARENT = 0x0100;
        private const int TTF_DI_SETITEM = 0x8000;
        private const int TTF_PARSELINKS = 0x1000;
        private const int TTDT_AUTOMATIC = 0;
        private const int TTDT_RESHOW = 1;
        private const int TTDT_AUTOPOP = 2;
        private const int TTDT_INITIAL = 3;
        // messages
        private const int WM_USER = 0x400;
        private const int TTM_ACTIVATE = (WM_USER + 1);
        private const int TTM_SETDELAYTIME = (WM_USER + 3);
        private const int TTM_RELAYEVENT = (WM_USER + 7);
        private const int TTM_GETTOOLCOUNT = (WM_USER + 13);
        private const int TTM_WINDOWFROMPOINT = (WM_USER + 16);
        private const int TTM_TRACKACTIVATE = (WM_USER + 17);
        private const int TTM_TRACKPOSITION = (WM_USER + 18);
        private const int TTM_SETTIPBKCOLOR = (WM_USER + 19);
        private const int TTM_SETTIPTEXTCOLOR = (WM_USER + 20);
        private const int TTM_GETDELAYTIME = (WM_USER + 21);
        private const int TTM_GETTIPBKCOLOR = (WM_USER + 22);
        private const int TTM_GETTIPTEXTCOLOR = (WM_USER + 23);
        private const int TTM_SETMAXTIPWIDTH = (WM_USER + 24);
        private const int TTM_GETMAXTIPWIDTH = (WM_USER + 25);
        private const int TTM_SETMARGIN = (WM_USER + 26);
        private const int TTM_GETMARGIN = (WM_USER + 27);
        private const int TTM_POP = (WM_USER + 28);
        private const int TTM_UPDATE = (WM_USER + 29);
        private const int TTM_POPUP = (WM_USER + 34);
        private const int TTM_ADJUSTRECT = (WM_USER + 31);
        // ansi
        private const int TTM_ADDTOOLA = (WM_USER + 4);
        private const int TTM_DELTOOLA = (WM_USER + 5);
        private const int TTM_NEWTOOLRECTA = (WM_USER + 6);
        private const int TTM_GETTOOLINFOA = (WM_USER + 8);
        private const int TTM_SETTOOLINFOA = (WM_USER + 9);
        private const int TTM_HITTESTA = (WM_USER + 10);
        private const int TTM_GETTEXTA = (WM_USER + 11);
        private const int TTM_UPDATETIPTEXTA = (WM_USER + 12);
        private const int TTM_GETCURRENTTOOLA = (WM_USER + 15);
        private const int TTM_ENUMTOOLSA = (WM_USER + 14);
        private const int TTM_SETTITLEA = (WM_USER + 32);
        // unicode
        private const int TTM_ADDTOOLW = (WM_USER + 50);
        private const int TTM_DELTOOLW = (WM_USER + 51);
        private const int TTM_NEWTOOLRECTW = (WM_USER + 52);
        private const int TTM_GETTOOLINFOW = (WM_USER + 53);
        private const int TTM_SETTOOLINFOW = (WM_USER + 54);
        private const int TTM_HITTESTW = (WM_USER + 55);
        private const int TTM_GETTEXTW = (WM_USER + 56);
        private const int TTM_UPDATETIPTEXTW = (WM_USER + 57);
        private const int TTM_ENUMTOOLSW = (WM_USER + 58);
        private const int TTM_GETCURRENTTOOLW = (WM_USER + 59);
        private const int TTM_SETTITLEW = (WM_USER + 33);
        // derived
        private int TTM_ADDTOOL;
        private int TTM_DELTOOL;
        private int TTM_NEWTOOLRECT;
        private int TTM_GETTOOLINFO;
        private int TTM_SETTOOLINFO;
        private int TTM_HITTEST;
        private int TTM_GETTEXT;
        private int TTM_UPDATETIPTEXT;
        private int TTM_ENUMTOOLS;
        private int TTM_GETCURRENTTOOL;
        private int TTM_SETTITLE;
        // ownerdraw
        private const int TTN_FIRST = (-520);
        private const int TTN_GETDISPINFOA = (TTN_FIRST - 0);
        private const int TTN_GETDISPINFOW = (TTN_FIRST - 10);
        private const int TTN_SHOW = (TTN_FIRST - 1);
        private const int TTN_POP = (TTN_FIRST - 2);
        private const int TTN_LAST = (-549);
        // styles
        private const int TTS_ALWAYSTIP = 0x01;
        private const int TTS_NOPREFIX = 0x02;
        private const int TTS_NOANIMATE = 0x10;
        private const int TTS_NOFADE = 0x20;
        private const int TTS_BALLOON = 0x40;
        private const int TTS_CLOSE = 0x80;
        private const int TTS_USEVISUALSTYLE = 0x100;
        // window messages
        private const int WM_NOTIFY = 0x4E;
        private const int WM_REFLECT = 0x2000;
        private const int WM_PAINT = 0xF;
        private const int WM_SIZE = 0x5;
        private const int WM_MOVE = 0x3;
        private const int WM_SETFONT = 0x30;
        private const int WM_GETFONT = 0x31;
        private const int WM_SHOWWINDOW = 0x18;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_MOUSELEAVE = 0x2A3;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_MOUSEWHEEL = 0x20A;
        private const int WM_TIMER = 0x113;
        private const int WM_NCPAINT = 0x85;
        private const int WM_DESTROY = 0x2;
        private const int WM_SETFOCUS = 0x7;
        private const int WM_KILLFOCUS = 0x8;
        private const int WM_IME_NOTIFY = 0x282;
        private const int WM_IME_SETCONTEXT = 0x281;
        private const int WM_ACTIVATE = 0x6;
        private const int WM_NCACTIVATE = 0x86;
        private const int WM_STYLECHANGED = 0x7d;
        private const int WM_STYLECHANGING = 0x7c;
        private const int WM_WINDOWPOSCHANGING = 0x46;
        private const int WM_WINDOWPOSCHANGED = 0x47;
        private const int WM_NCCALCSIZE = 0x83;
        private const int WM_CTLCOLOR = 0x3d8d610;
        // window styles
        private const int GWL_STYLE = (-16);
        private const int GWL_EXSTYLE = (-20);
        private const int WM_ACTIVATEAPP = 0x001C;
        private const int WS_TABSTOP = 0x10000;
        private const int WS_THICKFRAME = 0x40000;
        private const int WS_HSCROLL = 0x100000;
        private const int WS_VSCROLL = 0x200000;
        private const int WS_BORDER = 0x800000;
        private const int WS_CLIPCHILDREN = 0x2000000;
        private const int WS_CLIPSIBLINGS = 0x4000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_CHILD = 0x40000000;
        private const int WS_POPUP = -2147483648;
        // window extended styles
        private const int WS_EX_LTRREADING = 0x0;
        private const int WS_EX_LEFT = 0x0;
        private const int WS_EX_RIGHTSCROLLBAR = 0x0;
        private const int WS_EX_DLGMODALFRAME = 0x1;
        private const int WS_EX_NOPARENTNOTIFY = 0x4;
        private const int WS_EX_TOPMOST = 0x8;
        private const int WS_EX_ACCEPTFILES = 0x10;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int WS_EX_MDICHILD = 0x40;
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int WS_EX_WINDOWEDGE = 0x100;
        private const int WS_EX_CLIENTEDGE = 0x200;
        private const int WS_EX_CONTEXTHELP = 0x400;
        private const int WS_EX_RIGHT = 0x1000;
        private const int WS_EX_RTLREADING = 0x2000;
        private const int WS_EX_LEFTSCROLLBAR = 0x4000;
        private const int WS_EX_CONTROLPARENT = 0x10000;
        private const int WS_EX_STATICEDGE = 0x20000;
        private const int WS_EX_APPWINDOW = 0x40000;
        private const int WS_EX_NOACTIVATE = 0x8000000;
        private const int WS_EX_LAYERED = 0x80000;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            internal int X;
            internal int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            internal RECT(int X, int Y, int Width, int Height)
            {
                this.Left = X;
                this.Top = Y;
                this.Right = Width;
                this.Bottom = Height;
            }
            internal int Left;
            internal int Top;
            internal int Right;
            internal int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TOOLINFO
        {
            internal TOOLINFO(int flags)
            {
                this.cbSize = Marshal.SizeOf(typeof(TOOLINFO));
                this.uFlags = flags;
                this.hwnd = IntPtr.Zero;
                this.uId = IntPtr.Zero;
                this.rect = new RECT(0, 0, 0, 0);
                this.hinst = IntPtr.Zero;
                this.lpszText = IntPtr.Zero;
                this.lParam = IntPtr.Zero;
            }
            internal int cbSize;
            internal int uFlags;
            internal IntPtr hwnd;
            internal IntPtr uId;
            internal RECT rect;
            internal IntPtr hinst;
            internal IntPtr lpszText;
            internal IntPtr lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            internal NMHDR(int flag)
            {
                this.hwndFrom = IntPtr.Zero;
                this.idFrom = 0;
                this.code = 0;
            }
            internal IntPtr hwndFrom;
            internal int idFrom;
            internal int code;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NMTTDISPINFO
        {
            internal NMTTDISPINFO(int flags)
            {
                this.hdr = new NMHDR(0);
                this.lpszText = IntPtr.Zero;
                this.szText = IntPtr.Zero;
                this.hinst = IntPtr.Zero;
                this.uFlags = 0;
                this.lParam = IntPtr.Zero;
            }
            internal NMHDR hdr;
            internal IntPtr lpszText;
            internal IntPtr szText;
            internal IntPtr hinst;
            internal int uFlags;
            internal IntPtr lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NMCUSTOMDRAW
        {
            NMHDR hdr;
            uint dwDrawStage;
            IntPtr hdc;
            RECT rc;
            IntPtr dwItemSpec;
            uint uItemState;
            IntPtr lItemlParam;
        }

        private struct NMTTCUSTOMDRAW
        {
            NMCUSTOMDRAW nmcd;
            uint uDrawFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TT_HITTESTINFO
        {
            internal IntPtr hwnd;
            internal POINT pt;
            internal TOOLINFO ti;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            internal IntPtr hdc;
            internal int fErase;
            internal RECT rcPaint;
            internal int fRestore;
            internal int fIncUpdate;
            internal int Reserved1;
            internal int Reserved2;
            internal int Reserved3;
            internal int Reserved4;
            internal int Reserved5;
            internal int Reserved6;
            internal int Reserved7;
            internal int Reserved8;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct tagINITCOMMONCONTROLSEX
        {
            internal tagINITCOMMONCONTROLSEX(int flags)
            {
                this.dwSize = Marshal.SizeOf(typeof(tagINITCOMMONCONTROLSEX));
                this.dwICC = flags;
            }
            internal int dwSize;
            internal int dwICC;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TRACKMOUSEEVENT
        {
            internal uint cbSize;
            internal TRACKMOUSEEVENT_FLAGS dwFlags;
            internal IntPtr hwndTrack;
            internal uint dwHoverTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct BLENDFUNCTION
        {
            byte BlendOp;
            byte BlendFlags;
            byte SourceConstantAlpha;
            byte AlphaFormat;

            internal BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
            {
                BlendOp = op;
                BlendFlags = flags;
                SourceConstantAlpha = alpha;
                AlphaFormat = format;
            }
        }
        #endregion

        #region API
        [DllImport("user32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, BestFitMapping = false)]
        private static extern IntPtr CreateWindowEx(int exstyle, string lpClassName,
        string lpWindowName, int dwStyle, int x, int y, int nWidth,
        int nHeight, IntPtr hwndParent, IntPtr Menu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("comctl32.dll", EntryPoint = "InitCommonControlsEx", CallingConvention = CallingConvention.StdCall)]
        private static extern bool InitCommonControlsEx(ref tagINITCOMMONCONTROLSEX iccex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private extern static int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private extern static int SendMessage(IntPtr hWnd, int msg, int wParam, ref TOOLINFO lParam);
        [DllImport("user32.dll")]
        private extern static int SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private extern static int SendMessage(IntPtr hWnd, int msg, int wParam, ref RECT lParam);
        [DllImport("user32.dll")]
        private extern static int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPTStr)]string lParam);
        [DllImport("user32.dll")]
        private extern static int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref NMHDR lParam);
        [DllImport("user32.dll")]
        private extern static int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        [DllImport("kernel32.dll")]
        private extern static int RtlMoveMemory(ref NMHDR Destination, IntPtr Source, int Length);
        [DllImport("kernel32.dll")]
        private extern static int RtlMoveMemory(ref NMTTDISPINFO Destination, IntPtr Source, int Length);
        [DllImport("kernel32.dll")]
        private extern static int RtlMoveMemory(IntPtr Destination, ref NMTTDISPINFO Source, int Length);
        [DllImport("kernel32.dll")]
        private extern static int RtlMoveMemory(ref POINT Destination, ref RECT Source, int Length);
        [DllImport("kernel32.dll")]
        private extern static int RtlMoveMemory(ref NMTTCUSTOMDRAW Destination, IntPtr Source, int Length);
        [DllImport("kernel32.dll")]
        private extern static int RtlMoveMemory(ref NMCUSTOMDRAW Destination, IntPtr Source, int Length);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadIcon(IntPtr hInstance, int lpIconName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int x, int y, int cx, int cy, uint flags);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, ref RECT r);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr handle, IntPtr hdc);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        private static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PtInRect(ref RECT lprc, Point pt);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern IntPtr SetTimer(IntPtr hWnd, int nIDEvent, uint uElapse, IntPtr lpTimerFunc);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool KillTimer(IntPtr hWnd, uint uIDEvent);

        [DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
        private static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest,
        IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, BLENDFUNCTION blendFunction);

        [DllImport("user32.dll")]
        private static extern int SetFocus(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool StretchBlt(IntPtr hDest, int X, int Y, int nWidth, int nHeight, IntPtr hdcSrc,
        int sX, int sY, int nWidthSrc, int nHeightSrc, int dwRop);
        #endregion

        #region Fields
        private bool _bActive = false;
        private bool _bMultiLine = false;
        private bool _bShowAlways = false;
        private bool _bUseAnimation = false;
        private bool _bUseFading = true;
        private bool _bPainting = false;
        private bool _bBallonStyle = false;
        private bool _bNoPrefix = false;
        private bool _bCloseButton = false;
        private bool _bUseVisualStyle = false;
        private bool _bRightToLeft = false;
        private bool _bCenterTip = false;
        private bool _bClickable = false;
        private bool _bTimerActive = false;
        private bool _bCanFocus = false;
        private int _iAutomaticDelay = 500;
        private int _iAutoPopDelay = 5000;
        private int _iInitialDelay = 500;
        private int _iFadeDelay = 350;
        private int _iReShowDelay = 500;
        private int _iMaxWidth = 150;
        private int _iSafeTimer = 0;
        private float _fOpacity = .6f;
        private Color _oBackColor = Color.FromKnownColor(KnownColor.Info);
        private Color _oForeColor = Color.FromKnownColor(KnownColor.ControlText);
        private Color _oTitleColor = Color.FromKnownColor(KnownColor.ControlText);
        private Color _oGradientEndColor = Color.LightGray;
        private Color _oGradientStartColor = Color.White;
        private Color _oBorderColor = Color.DimGray;
        private Color _oPulseColor = Color.SteelBlue;
        private Size _oSize = new Size(0, 0);
        private Font _oTipFont = new Font("Tahoma", 8, FontStyle.Regular);
        private Font _oTitleFont = new Font("Tahoma", 9, FontStyle.Bold);
        private IntPtr _hTipWnd = IntPtr.Zero;
        private IntPtr _hParentWnd = IntPtr.Zero;
        private IntPtr _hInstance = IntPtr.Zero;
        private TipPosition _ePosition = TipPosition.AutoPosition;
        private TipStyle _eCustomStyle = TipStyle.Default;
        private FadeStyle _eFadeStyle = FadeStyle.Default;
        private Bitmap _bmGraphic;
        private cStoreDc _cBufferDc = new cStoreDc();
        private cStoreDc _cFaderDc = new cStoreDc();
        private cStoreDc _cBgDc = new cStoreDc();
        private cStoreDc _cImage = new cStoreDc();
        private cStoreDc _cDraw = new cStoreDc();
        private GradientStyle _eGradientStyle = GradientStyle.VerticalTube;
        private ImageList _oIcons = new ImageList();
        private ArrayList _aHandles = new ArrayList();
        private Dictionary<IntPtr, string> _dCaption = new Dictionary<IntPtr, string>();
        private Dictionary<IntPtr, string> _dTitle = new Dictionary<IntPtr, string>();
        private Dictionary<IntPtr, Size> _dSize = new Dictionary<IntPtr, Size>();
        #endregion

        #region Delegates
        /// <summary>
        /// 委托：
        ///     画图委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void DrawDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     正在处理委托
        /// </summary>
        public delegate void DisposingDelegate();
        /// <summary>
        /// 委托：
        ///     已经处理完成委托
        /// </summary>
        public delegate void DisposedDelegate();
        /// <summary>
        /// 委托：
        ///     Pop委托
        /// </summary>
        public delegate void PopDelegate();
        /// <summary>
        /// 委托：
        ///     显示委托
        /// </summary>
        public delegate void ShowDelegate();
        /// <summary>
        /// 委托：
        ///     鼠标移动委托
        /// </summary>
        /// <param name="pos">目标位置</param>
        public delegate void MouseMoveDelegate(Point pos);
        /// <summary>
        /// 委托：
        ///     鼠标左键按下委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void LeftButtonDownDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     鼠标左键弹起来委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void LeftButtonUpDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     鼠标左键双击委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void LeftButtonDblClickDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     鼠标中键按下委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void MiddleButtonDownDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     鼠标中键按弹起来委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void MiddleButtonUpDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     鼠标中键双击委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void MiddleButtonDblClickDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     鼠标右键按下委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void RightButtonDownDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     鼠标右键弹起委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void RightButtonUpDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     鼠标右键双击委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void RightButtonDblClickDelegate(Object sender, DrawEventArgs e);
        /// <summary>
        /// 委托：
        ///     鼠标滚轮滚动委托
        /// </summary>
        /// <param name="sender">委托源</param>
        /// <param name="e">数据</param>
        public delegate void MouseWheelDelegate();
        #endregion

        #region Events
        /// <summary>
        /// 事件：
        ///     鼠标移动事件
        /// </summary>
        [Description("Raised when the mouse moves over the ToolTip.")]
        public event MouseMoveDelegate MouseMove;
        /// <summary>
        /// 事件：
        ///     鼠标左键按下事件
        /// </summary>
        [Description("Raised when the mouse left button is depressed over the ToolTip.")]
        public event LeftButtonDownDelegate LeftButtonDown;
        /// <summary>
        /// 事件：
        ///     鼠标左键弹起事件
        /// </summary>
        [Description("Raised when the mouse left button is released over the ToolTip.")]
        public event LeftButtonUpDelegate LeftButtonUp;
        /// <summary>
        /// 事件：
        ///     鼠标左键双击事件
        /// </summary>
        [Description("Raised when the mouse left button is released over the ToolTip.")]
        public event LeftButtonDblClickDelegate LeftButtonDblClick;
        /// <summary>
        /// 事件：
        ///     鼠标中键按下事件
        /// </summary>
        [Description("Raised when the mouse middle button is depressed over the ToolTip.")]
        public event MiddleButtonDownDelegate MiddleButtonDown;
        /// <summary>
        /// 事件：
        ///     鼠标中键弹起事件
        /// </summary>
        [Description("Raised when the mouse left button is released over the ToolTip.")]
        public event MiddleButtonUpDelegate MiddleButtonUp;
        /// <summary>
        /// 事件：
        ///     鼠标中键双击事件
        /// </summary>
        [Description("Raised when the mouse left button is released over the ToolTip.")]
        public event MiddleButtonDblClickDelegate MiddleButtonDblClick;
        /// <summary>
        /// 事件：
        ///     鼠标右键按下事件
        /// </summary>
        [Description("Raised when the mouse right button is depressed over the ToolTip.")]
        public event RightButtonDownDelegate RightButtonDown;
        /// <summary>
        /// 事件：
        ///     鼠标右键弹起事件
        /// </summary>
        [Description("Raised when the mouse right button is released over the ToolTip.")]
        public event RightButtonUpDelegate RightButtonUp;
        /// <summary>
        /// 事件：
        ///     鼠标右键双击事件
        /// </summary>
        [Description("Raised when the mouse right button is released over the ToolTip.")]
        public event RightButtonDblClickDelegate RightButtonDblClick;
        /// <summary>
        /// 事件：
        ///     鼠标滚轮滚动事件
        /// </summary>
        [Description("Raised when the mouse wheel is scrolled over the ToolTip.")]
        public event MouseWheelDelegate MouseWheel;
        /// <summary>
        /// 事件：
        ///     控件绘制事件
        /// </summary>
        [Description("Interface for custom drawing the tooltip, used when OwnerDrawn style is used.")]
        public event DrawDelegate Draw;
        /// <summary>
        /// 事件：
        ///     正在销毁事件
        /// </summary>
        [Description("Raised when the ToolTip window is about to be destroyed.")]
        public event DisposingDelegate Disposing;
        /// <summary>
        /// 事件：
        ///     已经销毁完成事件
        /// </summary>
        [Description("Raised when the ToolTip window has been destroyed.")]
        public event DisposedDelegate Disposed;
        /// <summary>
        /// 事件：
        ///     Pop事件，ToolTip窗体被隐藏时触发
        /// </summary>
        [Description("Raised when the ToolTip window has been hidden.")]
        public event PopDelegate Pop;
        /// <summary>
        /// 事件：
        ///     PopUp事件，ToolTip窗体将要呈现时触发
        /// </summary>
        [Description("Raised when the ToolTip window is about to be shown.")]
        public event ShowDelegate PopUp;
        #endregion

        #region Constructors
        public WSNToolTip()
        {
            // initialize class
            tagINITCOMMONCONTROLSEX tg = new tagINITCOMMONCONTROLSEX(ICC_TAB_CLASSES);
            InitCommonControlsEx(ref tg);
            // get app instance
            Type t = typeof(WSNToolTip);
            Module m = t.Module;
            _hInstance = Marshal.GetHINSTANCE(m);
            // create window
            _hTipWnd = CreateWindowEx(WS_EX_TOPMOST | WS_EX_TOOLWINDOW/* | WS_EX_TRANSPARENT*/,
                TOOLTIPS_CLASS, "",
                WS_POPUP | TTS_NOPREFIX | TTS_ALWAYSTIP,
                0, 0,
                0, 0,
                IntPtr.Zero,
                IntPtr.Zero, _hInstance, IntPtr.Zero);
            // set position
            SetWindowPos(_hTipWnd, HWND_TOP,
                0, 0,
                0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_NOOWNERZORDER);
            SendMessage(_hTipWnd, WM_SETFONT, _oTipFont.ToHfont(), 0);
            windowStyle(_hTipWnd, GWL_STYLE, 0, WS_BORDER);
            useUnicode(IsUnicode);
            timerDefaults();
            base.AssignHandle(_hTipWnd);
        }

        private bool IsUnicode
        {
            get { return (Marshal.SystemDefaultCharSize != 1); }
        }

        private void timerDefaults()
        {
            _iAutomaticDelay = SendMessage(_hTipWnd, TTM_GETDELAYTIME, TTDT_AUTOMATIC, 0);
            _iAutoPopDelay = SendMessage(_hTipWnd, TTM_GETDELAYTIME, TTDT_AUTOPOP, 0);
            _iInitialDelay = SendMessage(_hTipWnd, TTM_GETDELAYTIME, TTDT_INITIAL, 0);
            _iReShowDelay = SendMessage(_hTipWnd, TTM_GETDELAYTIME, TTDT_RESHOW, 0);
        }

        private void useUnicode(bool unicode)
        {
            if (unicode)
            {
                TTM_ADDTOOL = TTM_ADDTOOLW;
                TTM_DELTOOL = TTM_DELTOOLW;
                TTM_NEWTOOLRECT = TTM_NEWTOOLRECTW;
                TTM_GETTOOLINFO = TTM_GETTOOLINFOW;
                TTM_SETTOOLINFO = TTM_SETTOOLINFOW;
                TTM_HITTEST = TTM_HITTESTW;
                TTM_GETTEXT = TTM_GETTEXTW;
                TTM_UPDATETIPTEXT = TTM_UPDATETIPTEXTW;
                TTM_GETCURRENTTOOL = TTM_GETCURRENTTOOLW;
                TTM_ENUMTOOLS = TTM_ENUMTOOLSW;
                TTM_GETCURRENTTOOL = TTM_GETCURRENTTOOLW;
                TTM_SETTITLE = TTM_SETTITLEW;
            }
            else
            {
                TTM_ADDTOOL = TTM_ADDTOOLA;
                TTM_DELTOOL = TTM_DELTOOLA;
                TTM_NEWTOOLRECT = TTM_NEWTOOLRECTA;
                TTM_GETTOOLINFO = TTM_GETTOOLINFOA;
                TTM_SETTOOLINFO = TTM_SETTOOLINFOA;
                TTM_HITTEST = TTM_HITTESTA;
                TTM_GETTEXT = TTM_GETTEXTA;
                TTM_UPDATETIPTEXT = TTM_UPDATETIPTEXTA;
                TTM_GETCURRENTTOOL = TTM_GETCURRENTTOOLA;
                TTM_ENUMTOOLS = TTM_ENUMTOOLSA;
                TTM_GETCURRENTTOOL = TTM_GETCURRENTTOOLA;
                TTM_SETTITLE = TTM_SETTITLEA;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// bool ToolTip.Active Gets/Sets a value indicating wether the ToolTip is currently active.
        /// </summary>
        public bool Active
        {
            get { return _bActive; }
            set
            {
                _bActive = value;
                SendMessage(_hTipWnd, TTM_ACTIVATE, value ? 1 : 0, 0);
            }
        }

        /// <summary>
        /// int ToolTip.AutomaticDelay Gets/Sets the automatic delay for the ToolTip.
        /// </summary>
        public int AutomaticDelay
        {
            get { return _iAutomaticDelay; }
            set
            {
                _iAutomaticDelay = value;
                SendMessage(_hTipWnd, TTM_SETDELAYTIME, TTDT_AUTOMATIC, _iAutomaticDelay);
            }
        }

        /// <summary>
        /// int ToolTip.AutoPopDelay Gets/Sets the period of time the ToolTip 
        /// remains visible if the pointer is stationary over the control.
        /// </summary>
        public int AutoPopDelay
        {
            get { return _iAutoPopDelay; }
            set
            {
                _iAutoPopDelay = value;
                SendMessage(_hTipWnd, TTM_SETDELAYTIME, TTDT_AUTOPOP, _iAutoPopDelay);
            }
        }

        /// <summary>
        /// Color ToolTip.BackColor Gets/Sets the ToolTip BackColor. If visual styles are 
        /// enabled, this will only have effect if using one of the customized modes.
        /// </summary>
        public Color BackColor
        {
            get { return _oBackColor; }
            set
            {
                _oBackColor = value;
                SendMessage(_hTipWnd, TTM_SETTIPBKCOLOR, ColorTranslator.ToWin32(_oBackColor), 0);
            }
        }

        /// <summary>
        /// Image ToolTip.BackGroundImage Gets/Sets the ToolTip background image. This method requires 
        /// the Graphical custom mode.
        /// </summary>
        public Bitmap BackGroundImage
        {
            get { return _bmGraphic; }
            set
            {
                _bmGraphic = value;
                _cImage.Height = _bmGraphic.Height;
                _cImage.Width = _bmGraphic.Width;
                SelectObject(_cImage.Hdc, _bmGraphic.GetHbitmap());
            }
        }

        /// <summary>
        /// bool ToolTip.BallonStyle Gets/Sets the appearance of a cartoon balloon.
        /// </summary>
        public bool BallonStyle
        {
            get { return _bBallonStyle; }
            set
            {
                _bBallonStyle = value;
                if (value)
                    windowStyle(_hTipWnd, GWL_STYLE, TTS_BALLOON, 0);
                else
                    windowStyle(_hTipWnd, GWL_STYLE, 0, TTS_BALLOON);
            }
        }

        /// <summary>
        /// Color ToolTip.BorderColor Gets/Sets the ToolTip border color in custom style mode.
        /// </summary>
        public Color BorderColor
        {
            get { return _oBorderColor; }
            set { _oBorderColor = value; }
        }

        /// <summary>
        /// bool ToolTip.CanFocus Gets/Sets a value determining wether the tip can get focus.
        /// </summary>
        public bool CanFocus
        {
            get { return _bCanFocus; }
            set { _bCanFocus = value; }
        }

        /// <summary>
        /// bool ToolTip.CenterTip Gets/Sets Centers the ToolTip window below the tool specified by the uId member.
        /// Must be added before SetTip method is invoked.
        /// </summary>
        public bool CenterTip
        {
            get { return _bCenterTip; }
            set { _bCenterTip = value; }
        }

        /// <summary>
        /// bool ToolTip.Clickable Gets/Sets the tooltip can respond to click events. 
        /// </summary>
        public bool Clickable
        {
            get { return _bClickable; }
            set { _bClickable = value; }
        }

        /// <summary>
        /// bool ToolTip.CloseButton Gets/Sets a Close button on the ToolTip. Valid only when the ToolTip has a title.
        /// </summary>
        public bool CloseButton
        {
            get { return _bCloseButton; }
            set
            {
                _bCloseButton = value;
                _bClickable = value;
                if (value)
                    windowStyle(_hTipWnd, GWL_STYLE, TTS_CLOSE, 0);
                else
                    windowStyle(_hTipWnd, GWL_STYLE, 0, TTS_CLOSE);
            }
        }

        /// <summary>
        /// enum ToolTip.CustomStyle Gets/Sets the custom style drawing option. Toggles the OwnerDrawn flag, set to 
        /// type 'None' to disable.
        /// </summary>
        public TipStyle CustomStyle
        {
            get { return _eCustomStyle; }
            set
            {
                _eCustomStyle = value;
                if (_eCustomStyle != TipStyle.Default)
                    windowStyle(_hTipWnd, GWL_EXSTYLE, 0, WS_EX_LAYERED);// in xp you get a nasty flicker with layered windows
                else
                    windowStyle(_hTipWnd, GWL_EXSTYLE, WS_EX_LAYERED, 0);
            }
        }

        /// <summary>
        /// int ToolTip.FadeDelay Gets/Sets the ToolTip fading delay interval in custom style mode.
        /// </summary>
        public int FadeDelay
        {
            get { return _iFadeDelay; }
            set { _iFadeDelay = value; }
        }

        /// <summary>
        /// (Vista) enum ToolTip.FadeOut Gets/Sets the ToolTip fading style in custom style mode.
        /// </summary>
        public FadeStyle FadeOut
        {
            get { return _eFadeStyle; }
            set { _eFadeStyle = value; }
        }

        /// <summary>
        /// (Vista) int ToolTip.FadePulseColor Gets/Sets the ToolTip fading border color when FadeStyle is set to Pulse in custom style mode.
        /// </summary>
        public Color FadePulseColor
        {
            get { return _oPulseColor; }
            set { _oPulseColor = value; }
        }

        /// <summary>
        /// Color ToolTip.ForeColor Gets/Sets the ToolTip ForeColor. If visual styles are 
        /// enabled, this will only have effect if using one of the customized modes.
        /// </summary>
        public Color ForeColor
        {
            get { return _oForeColor; }
            set
            {
                _oForeColor = value;
                SendMessage(_hTipWnd, TTM_SETTIPTEXTCOLOR, ColorTranslator.ToWin32(_oForeColor), 0);
            }
        }

        /// <summary>
        /// Color ToolTip.GradientEndColor Gets/Sets the gradient end blend color. Valid only with CustomStyles option. 
        /// </summary>
        public Color GradientEndColor
        {
            get { return _oGradientEndColor; }
            set { _oGradientEndColor = value; }
        }

        /// <summary>
        /// Color ToolTip.GradientStartColor Gets/Sets the gradient starting blend color. Valid only with CustomStyles option. 
        /// </summary>
        public Color GradientStartColor
        {
            get { return _oGradientStartColor; }
            set { _oGradientStartColor = value; }
        }

        /// <summary>
        /// enum ToolTip.GradientType Gets/Sets the style of gradient blending used. Valid only with CustomStyles option. 
        /// </summary>
        public GradientStyle GradientType
        {
            get { return _eGradientStyle; }
            set { _eGradientStyle = value; }
        }

        /// <summary>
        /// IntPtr ToolTip.Handle Gets the ToolTip windows handle.
        /// </summary>
        public new IntPtr Handle
        {
            get { return _hTipWnd; }
        }

        /// <summary>
        /// int ToolTip.InitialDelay Gets/Sets the time that passes before the ToolTip initially appears.
        /// </summary>
        public int InitialDelay
        {
            get { return _iInitialDelay; }
            set
            {
                _iInitialDelay = value;
                SendMessage(_hTipWnd, TTM_SETDELAYTIME, TTDT_INITIAL, _iInitialDelay);
            }
        }

        /// <summary>
        /// int ToolTip.MaximumWidth Global: Gets/Sets the maximum width of the ToolTip. 
        /// Use this property to tweak the multiline property display.
        /// </summary>
        public int MaximumWidth
        {
            get { return _iMaxWidth; }
            set
            {
                _iMaxWidth = value;
                if (_eCustomStyle == TipStyle.Default)
                {
                    SendMessage(_hTipWnd, TTM_SETMAXTIPWIDTH, 0, value);
                }
            }
        }

        /// <summary>
        /// bool ToolTip.MultiLine Gets/Sets the multiline attribute of the ToolTip. 
        /// For use with custom styles. Use the Maximum Width property to tweak the layout.
        /// </summary>
        public bool MultiLine
        {
            get { return _bMultiLine; }
            set
            {
                _bMultiLine = value;
                if (value)
                    SendMessage(_hTipWnd, TTM_SETMAXTIPWIDTH, 0, _iMaxWidth);
                else
                    SendMessage(_hTipWnd, TTM_SETMAXTIPWIDTH, 0, -1);
            }
        }

        /// <summary>
        /// bool ToolTip.Opacity Gets/Sets the transparency attribute of the ToolTip. 
        /// For use with custom styles. 
        /// </summary>
        public float Opacity
        {
            get { return _fOpacity; }
            set
            {
                if (value > 1.0f)
                    _fOpacity = 1.0f;
                else
                    _fOpacity = value;
            }
        }

        /// <summary>
        /// enum ToolTip.Position Gets/Sets the ToolTip position relative to the cursor.
        /// </summary>
        public TipPosition Position
        {
            get { return _ePosition; }
            set { _ePosition = value; }
        }

        /// <summary>
        /// bool ToolTip.NoPrefix Gets/Sets a value preventing the system from stripping the ampersand character from a string.
        /// </summary>
        public bool NoPrefix
        {
            get { return _bNoPrefix; }
            set
            {
                _bNoPrefix = value;
                if (value)
                    windowStyle(_hTipWnd, GWL_STYLE, TTS_NOPREFIX, 0);
                else
                    windowStyle(_hTipWnd, GWL_STYLE, 0, TTS_NOPREFIX);
            }
        }

        /// <summary>
        /// int ToolTip.ReShowDelay Gets/Sets the length of time that must transpire before subsequent 
        /// ToolTip windows appear as the pointer moves between one control to another.
        /// </summary>
        public int ReShowDelay
        {
            get { return _iReShowDelay; }
            set
            {
                _iReShowDelay = value;
                SendMessage(_hTipWnd, TTM_SETDELAYTIME, TTDT_RESHOW, _iReShowDelay);
            }
        }

        /// <summary>
        /// bool ToolTip.RightToLeft Gets/Sets ToolTip text will be displayed in the opposite direction to the text in the parent window. 
        /// Must be added before SetTip method is invoked.
        /// </summary>
        public bool RightToLeft
        {
            get { return _bRightToLeft; }
            set { _bRightToLeft = value; }
        }

        /// <summary>
        /// bool ToolTip.ShowAlways Gets/Sets a value indicating wether a ToolTip window is displayed, 
        /// even when its parent control is not active.
        /// </summary>
        public bool ShowAlways
        {
            get { return _bShowAlways; }
            set
            {
                _bShowAlways = value;
                if (value)
                    windowStyle(_hTipWnd, GWL_STYLE, TTS_ALWAYSTIP, 0);
                else
                    windowStyle(_hTipWnd, GWL_STYLE, 0, TTS_ALWAYSTIP);
            }
        }

        /// <summary>
        /// bool ToolTip.Showing Gets the visible state of the ToolTip.
        /// </summary>
        public bool Showing
        {
            get { return IsVisible(); }
        }

        /// <summary>
        /// Size ToolTip.Size Global: Gets/Sets the ToolTip windows current size. If the size is set, all tooltips 
        /// will render at the same size. To return to auto sizing, set the sizes to 0.
        /// </summary>
        public Size Size
        {
            get { return _oSize; }
            set { _oSize = value; }
        }

        /// <summary>
        /// Font ToolTip.TipFont Gets/Sets the ToolTip font for the main body of text. For use with OwnerDraw style or 
        /// custom styles options.
        /// </summary>
        public Font TipFont
        {
            get { return _oTipFont; }
            set
            {
                _oTipFont = value;
                SendMessage(_hTipWnd, WM_SETFONT, _oTipFont.ToHfont(), 0);
            }
        }

        /// <summary>
        /// Color ToolTip.TitleColor Gets/Sets the ToolTip title color. For use with custom styles options.
        /// </summary>
        public Color TitleColor
        {
            get { return _oTitleColor; }
            set { _oTitleColor = value; }
        }

        /// <summary>
        /// Font ToolTip.TitleFont Gets/Sets the ToolTip title font. For use with custom styles options.
        /// </summary>
        public Font TitleFont
        {
            get { return _oTitleFont; }
            set { _oTitleFont = value; }
        }

        /// <summary>
        /// bool ToolTip.UseAnimation Gets/Sets a value determing wether an animation 
        /// effect should be used when the ToolTip is displayed.
        /// </summary>
        public bool UseAnimation
        {
            get { return _bUseAnimation; }
            set
            {
                _bUseAnimation = value;
                if (!value)
                    windowStyle(_hTipWnd, GWL_STYLE, TTS_NOANIMATE, 0);
                else
                    windowStyle(_hTipWnd, GWL_STYLE, 0, TTS_NOANIMATE);
            }
        }

        /// <summary>
        /// (Vista) bool ToolTip.UseAnimation Gets/Sets a value determing wether a fade effect 
        /// should be used when the ToolTip is displayed.
        /// </summary>
        public bool UseFading
        {
            get { return _bUseFading; }
            set
            {
                _bUseFading = value;
                if (!value)
                    windowStyle(_hTipWnd, GWL_STYLE, TTS_NOFADE, 0);
                else
                    windowStyle(_hTipWnd, GWL_STYLE, 0, TTS_NOFADE);
            }
        }

        /// <summary>
        /// (Vista) bool ToolTip.UseVisualStyle Gets/Sets the use of themed hyperlinks in the ToolTip window.
        /// </summary>
        public bool UseVisualStyle
        {
            get { return _bUseVisualStyle; }
            set
            {
                _bUseVisualStyle = value;
                if (value)
                    windowStyle(_hTipWnd, GWL_STYLE, TTS_USEVISUALSTYLE, 0);
                else
                    windowStyle(_hTipWnd, GWL_STYLE, 0, TTS_USEVISUALSTYLE);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add an image to the internal imagelist.
        /// </summary>
        /// <param name="Image">Image</param>
        /// <param name="Size">Size</param>
        public void AddImage(Image Image, Size Size)
        {
            _oIcons.ImageSize = Size;
            _oIcons.Images.Add(Image);
        }

        /// <summary>
        /// Returns the ToolTip text associated with the control.
        /// </summary>
        /// <returns>string Text</returns>
        public string GetToolTipText(IntPtr Handle)
        {

            if (_dCaption.ContainsKey(Handle))
            {
                return (string)_dCaption[Handle];
            }
            return String.Empty;
        }

        /// <summary>
        /// Hides the ToolTip window.
        /// </summary>
        public void Hide()
        {
            SendMessage(_hTipWnd, TTM_POP, 0, 0);
            SetWindowPos(_hTipWnd,
                        HWND_TOP,
                        0, 0,
                        0, 0,
                        SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_NOSIZE | SWP_HIDEWINDOW | SWP_NOOWNERZORDER);
        }

        /// <summary>
        /// Returns the visible state of the ToolTip.
        /// </summary>
        /// <returns>state</returns>
        public bool IsVisible()
        {
            int style = GetWindowLong(_hTipWnd, GWL_STYLE);
            if ((style & WS_VISIBLE) == WS_VISIBLE)
                return true;
            return false;
        }

        /// <summary>
        /// Measures a string in the control context.
        /// </summary>
        /// <param name="Text">string text</param>
        /// <param name="Font">font</param>
        /// <returns>SizeF</returns>
        public SizeF MeasureString(string Text, Font Font)
        {
            SizeF sF = new SizeF();
            IntPtr hdc = GetDC(_hTipWnd);
            IntPtr hfont = (IntPtr)SendMessage(_hTipWnd, WM_GETFONT, 0, 0);
            IntPtr hfontOld = SelectObject(hdc, hfont);
            Graphics g = Graphics.FromHdc(hdc);
            sF = g.MeasureString(Text, Font, _iMaxWidth);
            SelectObject(hdc, hfontOld);
            ReleaseDC(_hTipWnd, hdc);
            g.Dispose();
            return sF;
        }

        /// <summary>
        /// Returns the current tip size and position.
        /// </summary>
        /// <returns>Rectangle</returns>
        public Rectangle ScreenPosition()
        {
            RECT tR = new RECT();
            GetWindowRect(_hTipWnd, ref tR);
            return new Rectangle(tR.Left, tR.Top, tR.Right, tR.Bottom);
        }

        /// <summary>
        /// Remove the ToolTip associated with the specified control window handle.
        /// </summary>
        public void RemoveTip(IntPtr Handle)
        {
            TOOLINFO tI = new TOOLINFO(0);
            if (_aHandles.Contains(Handle))
            {
                tI.hwnd = Handle;
                tI.uId = Handle;
                SendMessage(_hTipWnd, TTM_SETTITLE, IntPtr.Zero, "");
                SendMessage(_hTipWnd, TTM_DELTOOL, 0, ref tI);
                if (_dCaption.ContainsKey(Handle))
                    _dCaption.Remove(Handle);
                if (_dSize.ContainsKey(Handle))
                    _dSize.Remove(Handle);
                if (_oIcons.Images.ContainsKey(Handle.ToString()))
                    _oIcons.Images.RemoveByKey(Handle.ToString());
                _aHandles.RemoveAt(_aHandles.IndexOf(Handle));
            }
        }

        /// <summary>
        /// Remove all ToolTips.
        /// </summary>
        public void RemoveAll()
        {
            SendMessage(_hTipWnd, TTM_SETTITLE, IntPtr.Zero, "");
            foreach (IntPtr h in _aHandles)
            {
                TOOLINFO tI = new TOOLINFO(0);
                tI.hwnd = h;
                tI.uId = h;
                SendMessage(_hTipWnd, TTM_DELTOOL, 0, ref tI);
            }
            _dCaption.Clear();
            _aHandles.Clear();
            _dSize.Clear();
            _oIcons.Images.Clear();
        }

        /// <summary>
        /// Set the text margin boundary within the ToolTip
        /// </summary>
        /// <param name="Boundary">Rectangle boundary</param>
        public void SetMargin(Rectangle Boundary)
        {
            RECT tR = new RECT(Boundary.X, Boundary.Y, Boundary.Width, Boundary.Height);
            SendMessage(_hTipWnd, TTM_SETMARGIN, 0, ref tR);

        }

        /// <summary>
        /// Sets the bounding rectangle size for an individual ToolTip.
        /// </summary>
        /// <param name="Handle">IntPtr control handle</param>
        /// <param name="Size">Size dimensions</param>
        public void SetTipSize(IntPtr Handle, Size Size)
        {
            // TTM_NEWTOOLRECT doesn't appear to work on a themed control so, here's a workaround..
            if (Handle != IntPtr.Zero)
            {
                _dSize.Add(Handle, Size);
            }
        }

        /// <summary>
        /// Global/Local: Displays a title caption and an icon on the ToolTip. 
        /// If using Custom ToolTips, add the handle to the control for control specific icons.
        /// In normal mode, pass IntPtr.Zero as the handle, the Icon then becomes global.
        /// </summary>
        /// <param name="Handle">IntPtr control handle</param>
        /// <param name="Caption">string title</param>
        /// <param name="Image">Image image</param>
        public void SetTipTitle(IntPtr Handle, string Caption, Image Image, Size ImageSize)
        {
            if (Handle != IntPtr.Zero)
            {
                if (_dTitle.ContainsKey(Handle))
                    _dTitle.Remove(Handle);
                _dTitle.Add(Handle, Caption);
                if (Image != null)
                {
                    _oIcons.ImageSize = ImageSize;
                    _oIcons.Images.Add(Handle.ToString(), Image);
                }
            }
            else
            {
                Bitmap bM = new Bitmap(Image);
                SendMessage(_hTipWnd, TTM_SETTITLE, bM.GetHicon(), Caption);
                bM.Dispose();
            }
            _bMultiLine = true;
        }

        /// <summary>
        /// Global: Displays a title caption and a system icon on the ToolTip.
        /// Not for use with Custom styles options.
        /// </summary>
        /// <param name="Caption">string title</param>
        /// <param name="Image">enum system image</param>
        public void SetTipTitle(string Caption, OsIcon Image)
        {
            if (_eCustomStyle == TipStyle.Default)
            {
                IntPtr hIcon = LoadIcon(IntPtr.Zero, (int)Image);
                SendMessage(_hTipWnd, TTM_SETTITLE, hIcon, Caption);
                DestroyIcon(hIcon);
            }
            else
            {
                throw new Exception("The OsIcon option is only avaliable in the Default style mode. Specify the tool handle and icon size in non default modes.");
            }
        }

        /// <summary>
        /// Global: Displays a title caption and an icon on the ToolTip.
        /// </summary>
        /// <param name="Caption">string title</param>
        /// <param name="Image">Image Icon</param>
        public void SetTipTitle(string Caption, Icon Image)
        {
            if (_eCustomStyle == TipStyle.Default)
            {
                if (Image != null)
                    SendMessage(_hTipWnd, TTM_SETTITLE, Image.Handle, Caption);
            }
            else
            {
                throw new Exception("This method option is only avaliable in the Default style mode. Specify the tool handle and icon size in non default modes.");
            }
        }

        /// <summary>
        /// Global/Local: Displays a title caption on the ToolTip.
        /// </summary>
        /// <param name="Handle">IntPtr Handle - handle to the owner, can be zero in default mode</param>
        /// <param name="Caption">string title</param>
        public void SetTipTitle(IntPtr Handle, string Caption)
        {
            if (_eCustomStyle == TipStyle.Default)
            {
                SendMessage(_hTipWnd, TTM_SETTITLE, IntPtr.Zero, Caption);
            }
            else
            {
                if (Handle != IntPtr.Zero)
                {
                    if (_dTitle.ContainsKey(Handle))
                        _dTitle.Remove(Handle);
                    _dTitle.Add(Handle, Caption);
                }
            }
        }

        /// <summary>
        /// Binds the ToolTip window with the specified control.
        /// </summary>
        /// <param name="control">Control control</param>
        /// <param name="Caption">string Caption</param>
        public void SetToolTip(Control control, string Caption)
        {
            if (!_aHandles.Contains(control.Handle))
            {
                _aHandles.Add(control.Handle);
                _dCaption.Add(control.Handle, Caption);
                // create the tip default params
                TOOLINFO tI = new TOOLINFO(TTF_IDISHWND | TTF_SUBCLASS | TTF_PARSELINKS |
                    (_bRightToLeft ? TTF_RTLREADING : 0) |
                    (_bCenterTip ? TTF_CENTERTIP : 0));
                tI.hwnd = control.Handle;
                tI.uId = control.Handle;
                tI.hinst = _hInstance;
                IntPtr pS = Marshal.StringToHGlobalAuto(Caption);
                tI.lpszText = pS;
                GetClientRect(control.Handle, ref tI.rect);
                SendMessage(_hTipWnd, TTM_ADDTOOL, 0, ref tI);
                SendMessage(_hTipWnd, TTM_ACTIVATE, 1, 0);
                Marshal.FreeHGlobal(pS);
            }
        }

        /// <summary>
        /// Binds the ToolTip window with the specified control.
        /// </summary>
        /// <param name="control">IntPtr Handle</param>
        /// <param name="Caption">string Caption</param>
        public void SetToolTip(IntPtr Handle, string Caption)
        {
            if (!_aHandles.Contains(Handle))
            {
                _aHandles.Add(Handle);
                _dCaption.Add(Handle, Caption);
                // create the tip default params
                TOOLINFO tI = new TOOLINFO(TTF_IDISHWND | TTF_SUBCLASS | TTF_PARSELINKS | TTF_TRANSPARENT |
                    (_bRightToLeft ? TTF_RTLREADING : 0) |
                    (_bCenterTip ? TTF_CENTERTIP : 0));
                tI.hwnd = Handle;
                tI.uId = Handle;
                tI.hinst = _hInstance;
                IntPtr pS = Marshal.StringToHGlobalAuto(Caption);
                tI.lpszText = pS;
                GetClientRect(Handle, ref tI.rect);
                SendMessage(_hTipWnd, TTM_ADDTOOL, 0, ref tI);
                SendMessage(_hTipWnd, TTM_ACTIVATE, 1, 0);
                Marshal.FreeHGlobal(pS);
            }
        }

        /// <summary>
        /// Global: Binds the ToolTip window with the specified control. Tooltip is only activated
        /// within the specified rectangle.
        /// </summary>
        /// <param name="control">Control control</param>
        /// <param name="Caption">string caption</param>
        /// <param name="Bounds">Rectangle bounds</param>
        public void SetToolTipByRect(Control control, string Caption, Rectangle Bounds)
        {
            if (!_aHandles.Contains(control.Handle))
            {
                _aHandles.Add(control.Handle);
                _dCaption.Add(control.Handle, Caption);
            }
            if (_eCustomStyle == TipStyle.Default)
            {
                // create the tip default params
                TOOLINFO tI = new TOOLINFO(TTF_SUBCLASS | TTF_PARSELINKS | TTF_ABSOLUTE |
                    (_bRightToLeft ? TTF_RTLREADING : 0) |
                    (_bCenterTip ? TTF_CENTERTIP : 0));
                tI.hwnd = control.Handle;
                tI.uId = control.Handle;
                tI.hinst = _hInstance;
                IntPtr pS = Marshal.StringToHGlobalAuto(Caption);
                tI.lpszText = pS;
                tI.rect = new RECT(Bounds.X, Bounds.Y, Bounds.Right, Bounds.Bottom);
                SendMessage(_hTipWnd, TTM_ADDTOOL, 0, ref tI);
                SendMessage(_hTipWnd, TTM_ACTIVATE, 1, 0);
                Marshal.FreeHGlobal(pS);
            }
            else
            {
                throw new Exception("This method option is only avaliable in the Default style mode.");
            }
        }

        /// <summary>
        /// Global: Binds the ToolTip window with the specified control. Tooltip is only activated
        /// within the specified rectangle.
        /// </summary>
        /// <param name="control">Control control</param>
        /// <param name="Caption">string caption</param>
        /// <param name="Bounds">Rectangle bounds</param>
        public void SetToolTipByRect(IntPtr Handle, string Caption, Rectangle Bounds)
        {
            if (!_aHandles.Contains(Handle))
            {
                _aHandles.Add(Handle);
                _dCaption.Add(Handle, Caption);
            }
            if (_eCustomStyle == TipStyle.Default)
            {
                // create the tip default params
                TOOLINFO tI = new TOOLINFO(TTF_SUBCLASS | TTF_PARSELINKS | TTF_ABSOLUTE |
                    (_bRightToLeft ? TTF_RTLREADING : 0) |
                    (_bCenterTip ? TTF_CENTERTIP : 0));
                tI.hwnd = Handle;
                tI.uId = Handle;
                tI.hinst = _hInstance;
                IntPtr pS = Marshal.StringToHGlobalAuto(Caption);
                tI.lpszText = pS;
                tI.rect = new RECT(Bounds.X, Bounds.Y, Bounds.Right, Bounds.Bottom);
                SendMessage(_hTipWnd, TTM_ADDTOOL, 0, ref tI);
                SendMessage(_hTipWnd, TTM_ACTIVATE, 1, 0);
                Marshal.FreeHGlobal(pS);
            }
            else
            {
                throw new Exception("This method option is only avaliable in the Default style mode.");
            }
        }

        /// <summary>
        /// Redraw the ToolTip.
        /// </summary>
        public void Refresh()
        {
            SendMessage(_hTipWnd, TTM_UPDATE, 0, 0);
        }

        /// <summary>
        /// Displays the ToolTip window.
        /// </summary>
        public void Show()
        {
            SendMessage(_hTipWnd, TTM_POPUP, 0, 0);
        }

        /// <summary>
        /// Update the ToolTip text.
        /// </summary>
        /// <param name="Handle">IntPtr control handle</param>
        /// <param name="Caption">string caption</param>
        public void ToolUpdateText(IntPtr Handle, string Caption)
        {
            if (!_aHandles.Contains(Handle))
            {
                TOOLINFO tI = new TOOLINFO(0);
                tI.uId = Handle;
                tI.hinst = _hInstance;
                tI.hwnd = Handle;
                IntPtr pS = Marshal.StringToHGlobalAuto(Caption);
                SendMessage(_hTipWnd, TTM_UPDATETIPTEXT, 0, ref tI);
                Marshal.FreeHGlobal(pS);
            }
        }
        #endregion

        #region Private Methods
        #region Fader
        private void alphaBlit(IntPtr DstDc, int X, int Y, int Width, int Height, IntPtr SrcDc, int SrcX, int SrcY, int SrcWidth, int SrcHeight, byte btAlpha)
        {
            AlphaBlend(DstDc, X, Y, Width, Height, SrcDc, SrcX, SrcY, SrcWidth, SrcHeight, new BLENDFUNCTION(AC_SRC_OVER, 0x0, btAlpha, 0x0));
        }

        private void tipFade()
        {
            IntPtr hdc = GetDC(_hTipWnd);
            byte bA = 0;
            int iIncr = 0;

            switch (_eFadeStyle)
            {
                case FadeStyle.Default:
                    // bg
                    BitBlt(hdc, 0, 0, _cBgDc.Width, _cBgDc.Height, _cBgDc.Hdc, 0, 0, 0xCC0020);
                    iIncr = 255 / (_iFadeDelay / 50);
                    bA = (byte)(255 - (_iSafeTimer * iIncr));
                    alphaBlit(hdc, 0, 0, _cFaderDc.Width, _cFaderDc.Height, _cFaderDc.Hdc, 0, 0, _cFaderDc.Width, _cFaderDc.Height, bA);
                    break;
                case FadeStyle.Pulse:
                    iIncr = (255 / (_iFadeDelay / 50) * (_iSafeTimer * 2));
                    drawPulse(iIncr, hdc);
                    break;
                case FadeStyle.Phaser:
                    iIncr = _cFaderDc.Height / (_iFadeDelay / 50);
                    BitBlt(hdc, 0, 0, _cFaderDc.Width, _cFaderDc.Height, _cFaderDc.Hdc, 0, 0, 0xCC0020);
                    drawPhaseBar(iIncr * _iSafeTimer, hdc);
                    break;
            }
            ReleaseDC(_hTipWnd, hdc);
        }

        private void drawPhaseBar(int y, IntPtr hdc)
        {
            Rectangle rDmn = new Rectangle(2, y, _cFaderDc.Width - 4, 12);
            Graphics g = Graphics.FromHdc(hdc);
            Color c1 = Color.FromArgb(60, Color.Snow);
            Color c2 = Color.FromArgb(60, Color.WhiteSmoke);
            LinearGradientBrush hB = new LinearGradientBrush(
                rDmn,
                c1,
                c2,
                LinearGradientMode.Vertical);
            // magnifier effect
            alphaBlit(hdc, 2, y, _cFaderDc.Width - 4, 12, _cFaderDc.Hdc, 2, y, _cFaderDc.Width - 4, 14, 220);
            hB.SetBlendTriangularShape(.5f, 1.0f);
            g.FillRectangle(hB, rDmn);
            hB.Dispose();
            g.Dispose();
        }

        private void drawPulse(int intensity, IntPtr hdc)
        {
            int iPulse = 0;
            Graphics g = Graphics.FromHdc(hdc);
            if (intensity >= 255)
                iPulse = 255 - (intensity - 255);
            else
                iPulse = intensity;
            Brush hB = new SolidBrush(Color.FromArgb(iPulse, _oPulseColor));
            Rectangle rDmn = new Rectangle(0, 0, _cFaderDc.Width, _cFaderDc.Height);

            if (intensity > 250)
            {
                BitBlt(hdc, 0, 0, _cBgDc.Width, _cBgDc.Height, _cBgDc.Hdc, 0, 0, 0xCC0020);
                g.FillRectangle(hB, rDmn);
                BitBlt(hdc, 1, 1, _cBgDc.Width - 2, _cBgDc.Height - 2, _cBgDc.Hdc, 1, 1, 0xCC0020);
                alphaBlit(hdc, 1, 1, _cFaderDc.Width - 2, _cFaderDc.Height - 2, _cFaderDc.Hdc, 1, 1, _cFaderDc.Width - 2, _cFaderDc.Height - 2, (byte)iPulse);
            }
            else
            {
                BitBlt(hdc, 0, 0, _cBgDc.Width, _cBgDc.Height, _cBgDc.Hdc, 0, 0, 0xCC0020);
                g.FillRectangle(hB, rDmn);
                BitBlt(hdc, 1, 1, _cFaderDc.Width - 2, _cFaderDc.Height - 2, _cFaderDc.Hdc, 1, 1, 0xCC0020);
            }

            hB.Dispose();
            g.Dispose();
        }

        private void startFadeTimer()
        {
            if (!_bTimerActive)
            {
                _cFaderDc.Width = _cBufferDc.Width;
                _cFaderDc.Height = _cBufferDc.Height;
                BitBlt(_cFaderDc.Hdc, 0, 0, _cBufferDc.Width, _cBufferDc.Height, _cBufferDc.Hdc, 0, 0, 0xCC0020);
                SetTimer(_hTipWnd, 77, 50, IntPtr.Zero);
                _bTimerActive = true;
            }
        }

        private void stopFadeTimer()
        {
            if (_bTimerActive)
            {
                KillTimer(_hTipWnd, 77);
                _bTimerActive = false;
                _iSafeTimer = 0;
                SetWindowPos(_hTipWnd,
                HWND_TOP,
                0, 0,
                0, 0,
                SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_NOSIZE | SWP_HIDEWINDOW | SWP_NOOWNERZORDER);
            }
        }
        #endregion

        #region Helpers
        private DrawEventArgs getEventParams()
        {
            DrawEventArgs dR = new DrawEventArgs();
            TOOLINFO tI = new TOOLINFO(0);
            RECT tR = new RECT();
            string sT = String.Empty;
            string sC = String.Empty;

            GetWindowRect(_hTipWnd, ref tR);
            dR.Bounds = new Rectangle(0, 0, tR.Right - tR.Left, tR.Bottom - tR.Top);
            dR.Position = new Point(tR.Left, tR.Top);

            tI.uId = _hTipWnd;
            SendMessage(_hTipWnd, TTM_GETCURRENTTOOL, 0, ref tI);
            dR.ParentWnd = _hParentWnd;
            if (tI.hwnd != IntPtr.Zero)
            {
                if (_dTitle.ContainsKey(tI.hwnd))
                {
                    sT = (string)_dTitle[tI.hwnd];
                }
                if (_dCaption.ContainsKey(tI.hwnd))
                {
                    sC = (string)_dCaption[tI.hwnd];
                }
            }
            dR.Caption = sC;
            dR.Title = sT;
            dR.Hdc = IntPtr.Zero;
            return dR;
        }

        private void windowStyle(IntPtr handle, int type, int style, int stylenot)
        {
            int nStyle = GetWindowLong(handle, type);
            nStyle = ((nStyle & ~stylenot) | style);
            SetWindowLong(handle, type, nStyle);
            SetWindowPos(handle,
                        HWND_TOP,
                        0, 0,
                        0, 0,
                        (SWP_NOMOVE | SWP_NOSIZE | SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_FRAMECHANGED | SWP_NOACTIVATE));
        }

        private void tipChangeStyle(IntPtr handle, int Style, int StyleNot)
        {
            string sC = String.Empty;

            if (!_aHandles.Contains(handle))
            {
                TOOLINFO tI = new TOOLINFO(0);
                tI.hwnd = Handle;
                tI.uId = Handle;
                // get the tip data
                SendMessage(_hTipWnd, TTM_GETTOOLINFO, 0, ref tI);
                sC = (string)_dCaption[handle];
                // remove old
                RemoveTip(handle);
                // change the flags
                tI.uFlags = ((tI.uFlags & ~StyleNot) | Style);
                IntPtr pS = Marshal.StringToHGlobalAuto(sC);
                tI.lpszText = pS;
                // add the tip
                SendMessage(_hTipWnd, TTM_ADDTOOL, 0, ref tI);
                SendMessage(_hTipWnd, TTM_ACTIVATE, 1, 0);
                Marshal.FreeHGlobal(pS);
            }
        }

        private void trackMouse(IntPtr Hwnd)
        {
            TRACKMOUSEEVENT tM = new TRACKMOUSEEVENT();
            tM.cbSize = (uint)Marshal.SizeOf(tM);
            tM.dwFlags = TRACKMOUSEEVENT_FLAGS.TME_LEAVE;
            tM.hwndTrack = Hwnd;
            TrackMouseEvent(ref tM);
        }
        #endregion

        #region Custom Draw
        private void drawTip(Rectangle rDmn, string caption, string title, IntPtr hdc, IntPtr parent)
        {
            switch (_eCustomStyle)
            {
                case TipStyle.Glass:
                    _cBufferDc.Width = rDmn.Width;
                    _cBufferDc.Height = rDmn.Height;
                    drawGlass(rDmn, _cBufferDc.Hdc, caption, title, parent);
                    tipDrawCaption(rDmn, _cBufferDc.Hdc, caption, title, parent);
                    drawCloseButton(rDmn, _cBufferDc.Hdc);
                    BitBlt(hdc, 0, 0, rDmn.Width, rDmn.Height, _cBufferDc.Hdc, 0, 0, 0xCC0020);
                    break;
                case TipStyle.Gradient:
                    _cBufferDc.Width = rDmn.Width;
                    _cBufferDc.Height = rDmn.Height;
                    tipDrawGradient(rDmn, _cBufferDc.Hdc, caption, title, parent);
                    tipDrawBorder(rDmn, _cBufferDc.Hdc);
                    tipDrawCaption(rDmn, _cBufferDc.Hdc, caption, title, parent);
                    drawCloseButton(rDmn, _cBufferDc.Hdc);
                    BitBlt(hdc, 0, 0, rDmn.Width, rDmn.Height, _cBufferDc.Hdc, 0, 0, 0xCC0020);
                    break;
                case TipStyle.Graphical:
                    _cBufferDc.Width = rDmn.Width;
                    _cBufferDc.Height = rDmn.Height;
                    drawGraphic(rDmn, _cBufferDc.Hdc, caption, title, parent);
                    tipDrawCaption(rDmn, _cBufferDc.Hdc, caption, title, parent);
                    drawCloseButton(rDmn, _cBufferDc.Hdc);
                    BitBlt(hdc, 0, 0, rDmn.Width, rDmn.Height, _cBufferDc.Hdc, 0, 0, 0xCC0020);
                    break;
                case TipStyle.Mirror:
                    _cBufferDc.Width = rDmn.Width;
                    _cBufferDc.Height = rDmn.Height;
                    drawMirror(ref rDmn, _cBufferDc.Hdc, caption, title, parent);
                    tipDrawCaption(rDmn, _cBufferDc.Hdc, caption, title, parent);
                    drawCloseButton(rDmn, _cBufferDc.Hdc);
                    BitBlt(hdc, 0, 0, rDmn.Width, rDmn.Height, _cBufferDc.Hdc, 0, 0, 0xCC0020);
                    break;
                case TipStyle.Solid:
                    _cBufferDc.Width = rDmn.Width;
                    _cBufferDc.Height = rDmn.Height;
                    tipDrawSolid(rDmn, _cBufferDc.Hdc);
                    tipDrawBorder(rDmn, _cBufferDc.Hdc);
                    tipDrawCaption(rDmn, _cBufferDc.Hdc, caption, title, parent);
                    drawCloseButton(rDmn, _cBufferDc.Hdc);
                    BitBlt(hdc, 0, 0, rDmn.Width, rDmn.Height, _cBufferDc.Hdc, 0, 0, 0xCC0020);
                    break;

            }
        }

        #region Graphic Method
        private void drawGraphic(Rectangle rDmn, IntPtr hdc, string caption, string title, IntPtr parent)
        {
            RECT tR = new RECT();
            GetWindowRect(_hTipWnd, ref tR);
            // blit the capture, simulating transparency 
            if (_fOpacity < 1.0f)
                BitBlt(hdc, 0, 0, rDmn.Width, rDmn.Height, _cBgDc.Hdc, 0, 0, 0xCC0020);
            if (_bmGraphic != null)
            {
                _cDraw.Height = rDmn.Height;
                _cDraw.Width = rDmn.Width;
                // left side
                StretchBlt(_cDraw.Hdc, 0, 3, 3, (rDmn.Height - 6), _cImage.Hdc, 0, 3, 3, (_cImage.Height - 6), 0xCC0020);
                // right side
                StretchBlt(_cDraw.Hdc, (rDmn.Width - 3), 3, 3, (rDmn.Height - 6), _cImage.Hdc, (_cImage.Width - 3), 3, 3, (_cImage.Height - 6), 0xCC0020);
                // top left corner
                StretchBlt(_cDraw.Hdc, 0, 0, 3, 3, _cImage.Hdc, 0, 0, 3, 3, 0xCC0020);
                // top
                StretchBlt(_cDraw.Hdc, 3, 0, (rDmn.Width - 3), 3, _cImage.Hdc, 3, 0, (_cImage.Width - 3), 3, 0xCC0020);
                // bottom
                StretchBlt(_cDraw.Hdc, 3, (rDmn.Height - 3), (rDmn.Width - 3), 3, _cImage.Hdc, 3, (_cImage.Height - 3), (_cImage.Width - 3), 3, 0xCC0020);
                // bottom left corner
                StretchBlt(_cDraw.Hdc, 0, (rDmn.Height - 3), 3, 3, _cImage.Hdc, 0, (_cImage.Height - 3), 3, 3, 0xCC0020);
                // center
                StretchBlt(_cDraw.Hdc, 3, 3, (rDmn.Width - 6), (rDmn.Height - 6), _cImage.Hdc, 3, 3, (_cImage.Width - 6), (_cImage.Height - 6), 0xCC0020);
                // draw to buffer
                byte bt = (byte)(_fOpacity * 255);
                alphaBlit(hdc, 0, 0, rDmn.Width, rDmn.Height, _cDraw.Hdc, 0, 0, rDmn.Width, rDmn.Height, bt);
            }
        }
        #endregion

        #region Glass Method
        private void drawGlass(Rectangle rDmn, IntPtr hdc, string caption, string title, IntPtr parent)
        {
            RECT tR = new RECT();
            GetWindowRect(_hTipWnd, ref tR);
            // blit the capture, simulating transparency 
            if (_fOpacity < 1.0f)
                BitBlt(hdc, 0, 0, rDmn.Width, rDmn.Height, _cBgDc.Hdc, 0, 0, 0xCC0020);
            Graphics g = Graphics.FromHdc(hdc);
            // draw the frame -CDEBEB 7EABD4 BCD9EB 6D90B0
            Color c1 = Color.FromArgb(255, 0xCD, 0xEB, 0xEB);
            Color c2 = Color.FromArgb(255, 0x7E, 0xAB, 0xD4);
            Pen p1 = new Pen(c1, .9f);
            Pen p2 = new Pen(c2, .9f);
            g.DrawLines(p1, new Point[] {
                    new Point (0, rDmn.Height - 1), 
                    new Point (0, 0), 
                    new Point (rDmn.Width - 1, 0)
                });
            p1 = new Pen(c2, .1f);
            g.DrawLines(p2, new Point[] {
                    new Point (0, rDmn.Height - 1), 
                    new Point (rDmn.Width - 1, rDmn.Height - 1), 
                    new Point (rDmn.Width - 1, 0)
                });
            p1.Dispose();
            p2.Dispose();
            // draw 'sheen'
            rDmn.Inflate(-2, -2);
            float o = _fOpacity * 255;
            c1 = Color.FromArgb((int)o, Color.Snow);
            c2 = Color.FromArgb((int)o, 0x6D, 0x90, 0xB0);
            LinearGradientBrush hb = new LinearGradientBrush(
                rDmn,
                c1,
                c2,
                LinearGradientMode.ForwardDiagonal);
            hb.SetSigmaBellShape(1f, .5f);
            g.FillRectangle(hb, rDmn);
            hb.Dispose();
            g.Dispose();
        }
        #endregion

        #region Gradient Methods
        private void tipDrawGradient(Rectangle rDmn, IntPtr hdc, string caption, string title, IntPtr parent)
        {
            if (_fOpacity < 1.0f)
                BitBlt(hdc, 0, 0, rDmn.Width, rDmn.Height, _cBgDc.Hdc, 0, 0, 0xCC0020);
            switch (_eGradientStyle)
            {
                case GradientStyle.BackwardDiagonal:
                    drawLinearGradient(rDmn, hdc, LinearGradientMode.BackwardDiagonal);
                    break;
                case GradientStyle.ForwardDiagonal:
                    drawLinearGradient(rDmn, hdc, LinearGradientMode.ForwardDiagonal);
                    break;
                case GradientStyle.LinearHorizontal:
                    drawLinearGradient(rDmn, hdc, LinearGradientMode.Horizontal);
                    break;
                case GradientStyle.LinearVertical:
                    drawLinearGradient(rDmn, hdc, LinearGradientMode.Vertical);
                    break;
                case GradientStyle.VerticalTube:
                    drawLinearGradient(rDmn, hdc, LinearGradientMode.Horizontal);
                    break;
                case GradientStyle.HorizontalTube:
                    drawLinearGradient(rDmn, hdc, LinearGradientMode.Vertical);
                    break;
                case GradientStyle.SigmaBellShape:
                    drawLinearGradient(rDmn, hdc, LinearGradientMode.Vertical);
                    break;
                case GradientStyle.BlendTriangular:
                    drawPathGradient(rDmn, hdc);
                    break;
                case GradientStyle.FloatingBoxed:
                    drawPathGradient(rDmn, hdc);
                    break;
            }
        }

        private void drawLinearGradient(Rectangle rDmn, IntPtr hdc, LinearGradientMode m)
        {
            Graphics g = Graphics.FromHdc(hdc);
            float o = _fOpacity * 255;
            Color c1 = Color.FromArgb((int)o, _oGradientStartColor);
            Color c2 = Color.FromArgb((int)o, _oGradientEndColor);
            LinearGradientBrush hB = new LinearGradientBrush(
                rDmn,
                c1,
                c2,
                m);
            switch (_eGradientStyle)
            {
                case GradientStyle.VerticalTube:
                    hB.SetBlendTriangularShape(.5f, 1.0f);
                    g.FillRectangle(hB, rDmn);
                    break;
                case GradientStyle.HorizontalTube:
                    hB.SetBlendTriangularShape(.5f, 1.0f);
                    g.FillRectangle(hB, rDmn);
                    break;
                case GradientStyle.SigmaBellShape:
                    hB.SetSigmaBellShape(.5f, 1.0f);
                    g.FillRectangle(hB, rDmn);
                    break;
                default:
                    g.FillRectangle(hB, rDmn);
                    break;
            }
            hB.Dispose();
            g.Dispose();
        }

        private void drawPathGradient(Rectangle rDmn, IntPtr hdc)
        {
            Graphics g = Graphics.FromHdc(hdc);
            GraphicsPath gP = new GraphicsPath();
            gP.AddRectangle(rDmn);
            PathGradientBrush pGp = new PathGradientBrush(gP);

            float o = _fOpacity * 255;
            Color c1 = Color.FromArgb((int)o, _oGradientStartColor);
            Color c2 = Color.FromArgb((int)o, _oGradientEndColor);

            switch (_eGradientStyle)
            {
                case GradientStyle.BlendTriangular:
                    pGp.CenterPoint = new PointF(rDmn.Width / 2, rDmn.Height / 2);
                    pGp.CenterColor = c2;
                    pGp.SurroundColors = new Color[] { c1 };
                    g.FillPath(pGp, gP);
                    break;
                case GradientStyle.FloatingBoxed:
                    pGp.FocusScales = new PointF(0f, 0f);
                    pGp.CenterColor = c2;
                    pGp.SurroundColors = new Color[] { c1 };
                    Blend bP = new Blend();
                    bP.Positions = new float[] { 0f, .2f, .4f, .6f, .8f, 1f };
                    bP.Factors = new float[] { .2f, .5f, .2f, .5f, .2f, .5f };
                    pGp.Blend = bP;
                    g.FillPath(pGp, gP);
                    break;
            }
            pGp.Dispose();
            gP.Dispose();
            g.Dispose();
        }

        private void tipDrawBorder(Rectangle rDmn, IntPtr hdc)
        {
            // Draw the custom border to appear 3-dimensional.
            Graphics g = Graphics.FromHdc(hdc);
            Pen hP = new Pen(_oBorderColor);
            g.DrawLines(hP, new Point[] {
                    new Point (0, rDmn.Height - 1), 
                    new Point (0, 0), 
                    new Point (rDmn.Width - 1, 0)
                });
            g.DrawLines(hP, new Point[] {
                    new Point (0, rDmn.Height - 1), 
                    new Point (rDmn.Width - 1, rDmn.Height - 1), 
                    new Point (rDmn.Width - 1, 0)
                });
            hP.Dispose();
            g.Dispose();
        }
        #endregion

        #region Mirror Method
        private void drawMirror(ref Rectangle rDmn, IntPtr hdc, string caption, string title, IntPtr parent)
        {
            RECT tR = new RECT();
            GetWindowRect(_hTipWnd, ref tR);
            // blit the capture, simulating transparency
            if (_fOpacity < 1.0f)
                BitBlt(hdc, 0, 0, rDmn.Width, rDmn.Height, _cBgDc.Hdc, 0, 0, 0xCC0020);
            Graphics g = Graphics.FromHdc(hdc);
            // draw the frame
            Color c1 = Color.Silver;
            Color c2 = Color.SteelBlue;
            Pen p1 = new Pen(c1, .9f);
            Pen p2 = new Pen(c2, .9f);
            g.DrawLines(p1, new Point[] {
                    new Point (0, rDmn.Height - 1), 
                    new Point (0, 0), 
                    new Point (rDmn.Width - 1, 0)
                });
            p1 = new Pen(c2, .1f);
            g.DrawLines(p2, new Point[] {
                    new Point (0, rDmn.Height - 1), 
                    new Point (rDmn.Width - 1, rDmn.Height - 1), 
                    new Point (rDmn.Width - 1, 0)
                });
            p1.Dispose();
            p2.Dispose();

            // draw bevel
            rDmn.Inflate(-2, -2);
            rDmn.Offset(1, 1);
            float fO = _fOpacity * 255;
            c1 = Color.FromArgb((int)fO, Color.Snow);
            c2 = Color.FromArgb((int)fO, Color.Silver);
            // left
            Rectangle rBv = new Rectangle(1, 1, 4, rDmn.Height);
            Brush hS1 = new SolidBrush(c1);
            Brush hS2 = new SolidBrush(c2);
            g.FillRectangle(hS2, rBv);
            // bottom
            rBv = new Rectangle(1, rDmn.Height - 1, rDmn.Width, 4);
            g.FillRectangle(hS1, rBv);
            // right
            rBv = new Rectangle(rDmn.Width, 2, 4, rDmn.Height + 1);
            g.FillRectangle(hS1, rBv);
            // top
            rBv = new Rectangle(1, 1, rDmn.Width, 4);
            g.FillRectangle(hS2, rBv);
            hS1.Dispose();
            hS2.Dispose();
            // fill
            LinearGradientBrush hB = new LinearGradientBrush(
                rDmn,
                c1,
                c2,
                LinearGradientMode.ForwardDiagonal);
            rDmn.Inflate(1, 1);
            rDmn.Offset(-1, -1);
            hB.SetSigmaBellShape(1f, .5f);
            g.FillRectangle(hB, rDmn);

            hB.Dispose();
            g.Dispose();
        }
        #endregion

        #region Solid Method
        private void tipDrawSolid(Rectangle rDmn, IntPtr hdc)
        {
            Graphics g = Graphics.FromHdc(hdc);
            float o = _fOpacity * 255;
            Brush hB = new SolidBrush(Color.FromArgb((int)o, _oBackColor));
            g.FillRectangle(hB, rDmn);
            hB.Dispose();
            g.Dispose();
        }
        #endregion

        #region Common
        private SizeF calcTextSize(string text, Font font, int width, StringFormat format)
        {
            SizeF sF = new SizeF();
            IntPtr hdc = GetDC(_hTipWnd);
            Graphics g = Graphics.FromHdc(hdc);
            sF = g.MeasureString(text, font, width, format);
            ReleaseDC(_hTipWnd, hdc);
            g.Dispose();
            return sF;
        }

        private SizeF calcTextSize(string text, Font font, SizeF layout, StringFormat format)
        {
            SizeF sF = new SizeF();
            IntPtr hdc = GetDC(_hTipWnd);
            Graphics g = Graphics.FromHdc(hdc);
            sF = g.MeasureString(text, font, layout, format);
            ReleaseDC(_hTipWnd, hdc);
            g.Dispose();
            return sF;
        }

        private void calcTipSize(ref Rectangle tDm, IntPtr handle, Font font, string text, StringFormat format)
        {
            SizeF sF = new SizeF();
            if (_bMultiLine)
            {
                if (_iMaxWidth == -1)
                    _iMaxWidth = 1000;
                tDm.Width = _iMaxWidth;
            }
            IntPtr hdc = GetDC(_hTipWnd);
            Graphics g = Graphics.FromHdc(hdc);
            sF = calcTextSize(text, font, _iMaxWidth, format);

            if (_bMultiLine)
            {
                tDm.Height += (int)sF.Height + 4;
                tDm.Width = (int)sF.Width + 8;
            }
            else
            {
                tDm.Height = (int)font.GetHeight(g);
                tDm.Width = (int)sF.Width;
            }
            if (hasTitle(handle))
            {
                sF = calcTextSize(_dTitle[handle], _oTitleFont, _iMaxWidth, format);
                if (tDm.Width < (int)sF.Width)
                    tDm.Width = (int)sF.Width;
                if (hasIcon(handle))
                {
                    tDm.Height += _oIcons.ImageSize.Height;
                    sF.Width += _oIcons.ImageSize.Width + 8;
                    if (tDm.Width < (int)sF.Width)
                        tDm.Width = (int)sF.Width;
                }
                else
                    tDm.Height += _oTitleFont.Height;
            }
            if (_bCloseButton)
                tDm.Width += 20;
            g.Dispose();
            ReleaseDC(_hTipWnd, hdc);
        }

        private bool closeButtonHitTest()
        {
            Point pT = new Point();
            RECT tR = new RECT();

            GetClientRect(_hTipWnd, ref tR);
            GetCursorPos(ref pT);
            ScreenToClient(_hTipWnd, ref pT);
            tR.Left = tR.Right - 25;
            tR.Right -= 4;
            tR.Bottom = tR.Top + 25;
            tR.Top += 4;
            return (PtInRect(ref tR, pT));
        }

        private void drawCloseButton(Rectangle tDm, IntPtr hdc)
        {
            if (_bCloseButton)
            {
                Graphics g = Graphics.FromHdc(hdc);
                Pen pN = new Pen(Color.DarkSlateGray, 1.6F);
                g.DrawLine(pN, tDm.Width - 14, tDm.Y + 6, tDm.Width - 6, tDm.Y + 14);
                g.DrawLine(pN, tDm.Width - 6, tDm.Y + 6, tDm.Width - 14, tDm.Y + 14);
                pN.Dispose();
                g.Dispose();
            }
        }

        private bool hasIcon(IntPtr parent)
        {
            return _oIcons.Images.ContainsKey(parent.ToString());
        }

        private bool hasTitle(IntPtr parent)
        {
            return _dTitle.ContainsKey(parent);
        }

        private void tipCapture()
        {
            RECT tR = new RECT();

            GetWindowRect(_hTipWnd, ref tR);
            Size sZ = new Size(tR.Right - tR.Left, tR.Bottom - tR.Top);
            _cBgDc.Height = sZ.Height;
            _cBgDc.Width = sZ.Width;
            IntPtr pD = GetDesktopWindow();
            IntPtr pH = GetDC(pD);
            BitBlt(_cBgDc.Hdc, 0, 0, sZ.Width, sZ.Height, pH, tR.Left, tR.Top, 0xCC0020);
            ReleaseDC(pD, pH);
        }

        private void tipDrawCaption(Rectangle rDmn, IntPtr hdc, string caption, string title, IntPtr parent)
        {
            Graphics g = Graphics.FromHdc(hdc);
            int iOffsetX = 0;
            int iOffsetY = 0;
            SizeF sZ = new SizeF();

            if (hasIcon(parent))
            {
                _oIcons.Draw(g, rDmn.Left + 4, rDmn.Top + 4, _oIcons.Images.IndexOfKey(parent.ToString()));
                iOffsetX += _oIcons.ImageSize.Width + 4;
                iOffsetY = (_oIcons.ImageSize.Height / 2) + 4;
            }
            if (hasTitle(parent))
            {
                // draw title
                if (title.Length > 0)
                {
                    using (StringFormat sF = new StringFormat())
                    {
                        sF.Alignment = StringAlignment.Near;
                        sF.LineAlignment = StringAlignment.Center;
                        sF.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                        sF.FormatFlags = StringFormatFlags.NoWrap;
                        sZ = calcTextSize(_dTitle[parent], _oTitleFont, rDmn.Width - iOffsetX, sF);
                        if (!hasIcon(parent))
                        {
                            iOffsetY = 12;
                            iOffsetX = 4;
                        }
                        if (_bRightToLeft)
                            sF.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                        Brush hT = new SolidBrush(_oTitleColor);
                        g.DrawString(title, _oTitleFont, hT, new PointF(iOffsetX, iOffsetY), sF);
                        hT.Dispose();
                        if (!hasIcon(parent))
                            iOffsetY = (int)sZ.Height + 4;
                        else
                            iOffsetY = (_oIcons.ImageSize.Height) + 4;
                    }
                }
            }
            // draw caption
            if (caption.Length > 0)
            {
                using (StringFormat sF = new StringFormat())
                {
                    Brush hC = new SolidBrush(_oForeColor);
                    if (_oSize.Width != 0)
                    {
                        rDmn.Width = _oSize.Width;
                    }
                    if (_bMultiLine)
                    {
                        sF.Alignment = StringAlignment.Near;
                        sF.LineAlignment = StringAlignment.Near;
                        RECT tR = new RECT(4, iOffsetY, rDmn.Width, rDmn.Height);
                        SendMessage(_hTipWnd, TTM_ADJUSTRECT, 0, ref tR);
                        g.DrawString(caption, _oTipFont, hC, new RectangleF(4, tR.Top, tR.Right, tR.Bottom), sF);
                    }
                    else
                    {
                        sF.FormatFlags = StringFormatFlags.NoWrap;
                        g.DrawString(caption, _oTipFont, hC, new PointF(2, (rDmn.Bottom - _oTipFont.Height) / 2), sF);
                    }
                    hC.Dispose();
                }
            }
            g.Dispose();
        }
        #endregion
        #endregion
        #endregion

        #region WndProc
        /// <summary>
        /// message pump
        /// </summary>
        /// <param name="m">message struct</param>
        protected override void WndProc(ref Message m)
        {
            TOOLINFO tI = new TOOLINFO(0);
            RECT tR = new RECT();
            Size sZ = new Size();
            Point pT = new Point();
            DrawEventArgs dR;

            switch (m.Msg)
            {
                // window painting
                case WM_PAINT:
                    PAINTSTRUCT tPaint = new PAINTSTRUCT();
                    string sT = String.Empty;
                    string sC = String.Empty;
                    if (_eCustomStyle != TipStyle.Default)
                    {
                        if (!_bPainting)
                        {
                            _bPainting = true;
                            // start painting engine
                            BeginPaint(m.HWnd, ref tPaint);
                            dR = getEventParams();
                            if (Draw != null)
                            {
                                dR.Hdc = tPaint.hdc;
                                Draw(this, dR);
                            }
                            else
                            {
                                drawTip(dR.Bounds, dR.Caption, dR.Title, tPaint.hdc, dR.ParentWnd);
                            }
                            // done
                            EndPaint(m.HWnd, ref tPaint);
                            _bPainting = false;
                        }
                        else
                        {
                            base.DefWndProc(ref m);
                        }
                    }
                    else
                    {
                        // call the old proc
                        base.WndProc(ref m);
                    }
                    break;

                case (WM_NOTIFY | WM_REFLECT):
                    NMHDR nM = new NMHDR(0);
                    RtlMoveMemory(ref nM, m.LParam, Marshal.SizeOf(nM));
                    if (nM.hwndFrom == _hTipWnd)
                    {
                        switch (nM.code)
                        {
                            //case TTN_GETDISPINFOA: <- not working
                            //case TTN_GETDISPINFOW:
                            //    break;
                            case TTN_SHOW:
                                Point tp = new Point();
                                SendMessage(_hTipWnd, TTM_GETCURRENTTOOL, 0, ref tI);
                                _hParentWnd = tI.hwnd;
                                //// SIZE ////
                                // tip size set globally
                                if ((_oSize.Width != 0) || (_oSize.Height != 0))
                                {
                                    tR.Left = 0;
                                    tR.Top = 0;
                                    tR.Bottom = _oSize.Height;
                                    tR.Right = _oSize.Width;
                                    SendMessage(_hTipWnd, TTM_ADJUSTRECT, 1, ref tR);
                                    SetWindowPos(_hTipWnd,
                                                HWND_TOP,
                                                0, 0,
                                                tR.Right, tR.Bottom,
                                                SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_NOOWNERZORDER);
                                    m.Result = RETURN_TRUE;
                                }
                                else
                                {
                                    // tip size set individually
                                    tI.uId = _hTipWnd;
                                    // get tool parent
                                    SendMessage(_hTipWnd, TTM_GETCURRENTTOOL, 0, ref tI);
                                    if (tI.hwnd != IntPtr.Zero)
                                    {
                                        // test the dictionary
                                        if (_dSize.ContainsKey(tI.hwnd))
                                        {
                                            sZ = _dSize[tI.hwnd];
                                            // size tip
                                            if ((sZ.Width != 0) || (sZ.Height != 0))
                                            {
                                                tR.Left = 0;
                                                tR.Top = 0;
                                                tR.Bottom = sZ.Height;
                                                tR.Right = sZ.Width;
                                                SendMessage(_hTipWnd, TTM_ADJUSTRECT, 1, ref tR);
                                                SetWindowPos(_hTipWnd,
                                                            HWND_TOP,
                                                            0, 0,
                                                            tR.Right, tR.Bottom,
                                                            SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_NOOWNERZORDER);
                                                m.Result = RETURN_TRUE;
                                            }
                                        }
                                        // calculate size
                                        else if (_eCustomStyle != TipStyle.Default)
                                        {
                                            StringFormat sF = new StringFormat();
                                            sF.Alignment = StringAlignment.Near;
                                            sF.LineAlignment = StringAlignment.Near;
                                            Rectangle rDmn = new Rectangle(4, sZ.Height, (tR.Right - tR.Left), (tR.Bottom - tR.Top));
                                            calcTipSize(ref rDmn, tI.hwnd, _oTipFont, _dCaption[tI.hwnd], sF);
                                            sF.Dispose();
                                            SetWindowPos(_hTipWnd,
                                                        HWND_TOP,
                                                        0, 0,
                                                        rDmn.Width + 8, rDmn.Height + sZ.Height + 8,
                                                        SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_NOOWNERZORDER);
                                            m.Result = RETURN_TRUE;
                                        }
                                    }
                                }
                                //// POSITION ////
                                if (_ePosition != TipPosition.AutoPosition)
                                {
                                    if (tI.hwnd != IntPtr.Zero)
                                    {
                                        GetCursorPos(ref tp);
                                        GetWindowRect(_hTipWnd, ref tR);
                                        // offset for global size
                                        if ((_oSize.Width != 0) || (_oSize.Height != 0))
                                        {
                                            tR.Bottom = tR.Top + _oSize.Height;
                                            tR.Right = tR.Left + _oSize.Width;
                                        }
                                        // offset for tp size mod
                                        else if (_dSize.ContainsKey(tI.hwnd))
                                        {
                                            sZ = _dSize[tI.hwnd];
                                            tR.Bottom = tR.Top + sZ.Height;
                                            tR.Right = tR.Left + sZ.Width;
                                        }
                                        int iX = 0;
                                        int iY = 0;
                                        if ((_bShowAlways) || (_bClickable))
                                        {
                                            iX = 12;
                                            iY = 12;
                                        }
                                        else
                                        {
                                            iX = 20;
                                            iY = 20;
                                        }
                                        switch (_ePosition)
                                        {
                                            case TipPosition.BottomCenter:
                                                tp.Y += iX;
                                                tp.X -= ((tR.Right - tR.Left) / 2);
                                                break;
                                            case TipPosition.BottomLeft:
                                                tp.Y += iX;
                                                tp.X -= ((tR.Right - tR.Left) - 4);
                                                break;
                                            case TipPosition.BottomRight:
                                                tp.Y += iX;
                                                tp.X += 12;
                                                break;
                                            case TipPosition.LeftCenter:
                                                tp.Y -= ((tR.Bottom - tR.Top) / 2);
                                                tp.X -= ((tR.Right - tR.Left) + 4);
                                                break;
                                            case TipPosition.RightCenter:
                                                tp.Y -= ((tR.Bottom - tR.Top) / 2);
                                                tp.X += 20;
                                                break;
                                            case TipPosition.TopCenter:
                                                tp.Y -= ((tR.Bottom - tR.Top) + 4);
                                                tp.X -= ((tR.Right - tR.Left) / 2);
                                                break;
                                            case TipPosition.TopLeft:
                                                tp.Y -= ((tR.Bottom - tR.Top) + 4);
                                                tp.X -= ((tR.Right - tR.Left) + 4);
                                                break;
                                            case TipPosition.TopRight:
                                                tp.Y -= ((tR.Bottom - tR.Top) + 4);
                                                break;
                                        }
                                        SetWindowPos(_hTipWnd,
                                                    HWND_TOP,
                                                    tp.X, tp.Y,
                                                    0, 0,
                                                    SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_NOOWNERZORDER);
                                    }
                                    m.Result = RETURN_TRUE;
                                }
                                if (PopUp != null) PopUp();
                                tipCapture();
                                break;

                            case TTN_POP:
                                if (Pop != null) Pop();
                                //base.WndProc(ref m);
                                break;

                            //case NM_CUSTOMDRAW:
                            //NMTTCUSTOMDRAW nC = new NMTTCUSTOMDRAW(); //<- can't get this to work..
                            //RtlMoveMemory(ref nC, m.LParam, Marshal.SizeOf(nC));
                            //    break;
                            default:
                                base.WndProc(ref m);
                                break;
                        }
                    }
                    break;

                // mouse events
                case WM_MOUSEMOVE:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (MouseMove != null)
                        {
                            GetCursorPos(ref pT);
                            MouseMove(pT);
                        }
                        if (_bClickable)
                            m.Result = RETURN_TRUE;
                        else
                            base.WndProc(ref m);
                        trackMouse(m.HWnd);
                    }
                    if ((!_bShowAlways) && (!_bClickable))
                        base.WndProc(ref m);
                    break;
                case WM_MOUSELEAVE:
                    if ((_bShowAlways) || (_bClickable))
                    {
                        if (IsVisible())
                        {
                            Hide();
                        }
                    }
                    base.WndProc(ref m);
                    break;
                case WM_LBUTTONDOWN:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (LeftButtonDown != null)
                        {
                            dR = getEventParams();
                            dR.Hdc = GetDC(_hTipWnd);
                            LeftButtonDown(this, dR);
                            ReleaseDC(_hTipWnd, dR.Hdc);
                        }
                        if (_bClickable)
                            m.Result = RETURN_TRUE;
                        else
                            base.WndProc(ref m);
                    }
                    break;
                case WM_LBUTTONUP:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (LeftButtonUp != null)
                        {
                            dR = getEventParams();
                            dR.Hdc = GetDC(_hTipWnd);
                            LeftButtonUp(this, dR);
                            ReleaseDC(_hTipWnd, dR.Hdc);
                        }
                        if (_bCloseButton)
                        {
                            if (closeButtonHitTest())
                            {
                                Hide();
                            }
                            else
                            {
                                base.WndProc(ref m);
                            }
                        }
                        else if (_bClickable)
                        {
                            m.Result = RETURN_TRUE;
                        }
                        else
                        {
                            base.WndProc(ref m);
                        }
                    }
                    break;
                case WM_LBUTTONDBLCLK:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (LeftButtonDblClick != null)
                        {
                            dR = getEventParams();
                            dR.Hdc = GetDC(_hTipWnd);
                            LeftButtonDblClick(this, dR);
                            ReleaseDC(_hTipWnd, dR.Hdc);
                        }
                        if (_bClickable)
                            m.Result = RETURN_TRUE;
                        else
                            base.WndProc(ref m);
                    }
                    break;
                case WM_MBUTTONDOWN:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (MiddleButtonDown != null)
                        {
                            dR = getEventParams();
                            dR.Hdc = GetDC(_hTipWnd);
                            MiddleButtonDown(this, dR);
                            ReleaseDC(_hTipWnd, dR.Hdc);
                        }
                        if (_bClickable)
                            m.Result = RETURN_TRUE;
                        else
                            base.WndProc(ref m);
                    }
                    break;
                case WM_MBUTTONUP:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (MiddleButtonUp != null)
                        {
                            dR = getEventParams();
                            dR.Hdc = GetDC(_hTipWnd);
                            MiddleButtonUp(this, dR);
                            ReleaseDC(_hTipWnd, dR.Hdc);
                        }
                        if (_bClickable)
                            m.Result = RETURN_TRUE;
                        else
                            base.WndProc(ref m);
                    }
                    break;
                case WM_MBUTTONDBLCLK:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (MiddleButtonDblClick != null)
                        {
                            dR = getEventParams();
                            dR.Hdc = GetDC(_hTipWnd);
                            MiddleButtonDblClick(this, dR);
                            ReleaseDC(_hTipWnd, dR.Hdc);
                        }
                        if (_bClickable)
                            m.Result = RETURN_TRUE;
                        else
                            base.WndProc(ref m);
                    }
                    break;
                case WM_RBUTTONDOWN:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (RightButtonDown != null)
                        {
                            dR = getEventParams();
                            dR.Hdc = GetDC(_hTipWnd);
                            RightButtonDown(this, dR);
                            ReleaseDC(_hTipWnd, dR.Hdc);
                        }
                        if (_bClickable)
                            m.Result = RETURN_TRUE;
                        else
                            base.WndProc(ref m);
                    }
                    break;
                case WM_RBUTTONUP:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (RightButtonUp != null)
                        {
                            dR = getEventParams();
                            dR.Hdc = GetDC(_hTipWnd);
                            RightButtonDown(this, dR);
                            ReleaseDC(_hTipWnd, dR.Hdc);
                        }
                        if (_bClickable)
                            m.Result = RETURN_TRUE;
                        else
                            base.WndProc(ref m);
                    }
                    break;
                case WM_RBUTTONDBLCLK:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (RightButtonUp != null)
                        {
                            dR = getEventParams();
                            dR.Hdc = GetDC(_hTipWnd);
                            RightButtonDblClick(this, dR);
                            ReleaseDC(_hTipWnd, dR.Hdc);
                        }
                        if (_bClickable)
                            m.Result = RETURN_TRUE;
                        else
                            base.WndProc(ref m);
                    }
                    break;
                case WM_MOUSEWHEEL:
                    if (m.HWnd == _hTipWnd)
                    {
                        if (MouseWheel != null)
                        {
                            MouseWheel();
                        }
                    }
                    break;

                case WM_TIMER:
                    switch (m.WParam.ToInt32())
                    {
                        case HOVER_SIGNALED:
                            if (IsVisible())
                            {
                                if ((_bShowAlways) || (_bClickable))
                                {
                                    GetWindowRect(_hTipWnd, ref tR);
                                    GetCursorPos(ref pT);
                                    if (PtInRect(ref tR, pT))
                                        m.Result = RETURN_TRUE;
                                    else
                                        base.WndProc(ref m);
                                }
                                else
                                {
                                    base.WndProc(ref m);
                                }
                            }
                            else
                            {
                                base.WndProc(ref m);
                            }
                            break;
                        case FADER_SIGNALED: //<-vista only
                            if (IsVisible())
                            {
                                if ((_bShowAlways) || (_bClickable))
                                {
                                    GetWindowRect(_hTipWnd, ref tR);
                                    GetCursorPos(ref pT);
                                    if (PtInRect(ref tR, pT))
                                        m.Result = RETURN_TRUE;
                                    else
                                        base.WndProc(ref m);
                                }
                                else if (_eCustomStyle != TipStyle.Default)
                                {
                                    if (!_bTimerActive)
                                    {
                                        startFadeTimer();
                                        m.Result = RETURN_TRUE;
                                    }
                                    else
                                    {
                                        m.Result = RETURN_TRUE;
                                    }
                                }
                                else
                                {
                                    base.WndProc(ref m);
                                }
                            }
                            break;
                        case TIMER_FADE:
                            _iSafeTimer += 1;
                            if (_iSafeTimer > (_iFadeDelay / 50))
                                stopFadeTimer();
                            else
                                tipFade();
                            break;
                        default:
                            base.WndProc(ref m);
                            break;
                    }
                    break;

                case WM_SETFOCUS:
                    if (!_bCanFocus)
                    {
                        if (_hParentWnd != IntPtr.Zero)
                            SetFocus(_hParentWnd);
                        if (_bClickable)
                        {
                            if (closeButtonHitTest())
                                SendMessage(_hTipWnd, WM_LBUTTONUP, 0, 0);
                        }
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;

                case WM_STYLECHANGED:
                    if (_eCustomStyle == TipStyle.Default)
                        SendMessage(_hTipWnd, TTM_SETTIPBKCOLOR, ColorTranslator.ToWin32(Color.LightYellow), 0); //<-added for xp
                    base.WndProc(ref m);
                    break;

                /*case WM_NCPAINT:
                    break;
                case WM_DESTROY:
                    break;
                case WM_ACTIVATEAPP:
                    break;
                 case WM_ACTIVATE:
                    break;
                case WM_KILLFOCUS:
                    break;
                 case WM_IME_NOTIFY:
                    break;
                 case WM_IME_SETCONTEXT:
                    break;
                 case WM_NCACTIVATE:
                    break;
                case WM_STYLECHANGING:
                    break;
                case WM_WINDOWPOSCHANGING:
                    break;
                case WM_WINDOWPOSCHANGED:
                    break;
                case WM_NCCALCSIZE:
                    break;
                // undocumented?
                case 0x410: // effects show/timer
                    break;
                case 0x418: // format
                    break;
                case 0x421: // title
                    break;
                case 0x407: //show/timer
                    break;*/

                default:
                    //Debug.Print(m.Msg.ToString());
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

        #region Finalizers
        public void Dispose()
        {
            if (_hTipWnd != IntPtr.Zero)
            {
                RemoveAll();
                if (Disposing != null)
                    Disposing();
                base.ReleaseHandle();
                if (_cBgDc != null)
                    _cBgDc.Dispose();
                if (_cBufferDc != null)
                    _cBufferDc.Dispose();
                if (_cFaderDc != null)
                    _cFaderDc.Dispose();
                if (_cImage != null)
                    _cImage.Dispose();
                if (_cDraw != null)
                    _cDraw.Dispose();
                if (_oIcons != null)
                    _oIcons.Dispose();
                if (_bmGraphic != null)
                    _bmGraphic.Dispose();
                if (_oTipFont != null)
                    _oTipFont.Dispose();
                if (_oTitleFont != null)
                    _oTitleFont.Dispose();
                DestroyWindow(_hTipWnd);
                _hTipWnd = IntPtr.Zero;
                if (Disposed != null)
                    Disposed();
            }
        }

        ~WSNToolTip()
        {
            Dispose();
        }

        #endregion
    }
    #region EventArgs
    public class DrawEventArgs
    {
        internal Rectangle _oBounds = new Rectangle();
        internal Point _oPosition = new Point();
        internal IntPtr _pParentWnd = IntPtr.Zero;
        internal IntPtr _pHdc = IntPtr.Zero;
        internal string _sCaption = String.Empty;
        internal string _sTitle = String.Empty;

        public Rectangle Bounds
        {
            get { return _oBounds; }
            set { _oBounds = value; }
        }

        public Point Position
        {
            get { return _oPosition; }
            set { _oPosition = value; }
        }

        public IntPtr ParentWnd
        {
            get { return _pParentWnd; }
            set { _pParentWnd = value; }
        }

        public IntPtr Hdc
        {
            get { return _pHdc; }
            set { _pHdc = value; }
        }

        public string Caption
        {
            get { return _sCaption; }
            set { _sCaption = value; }
        }

        public string Title
        {
            get { return _sTitle; }
            set { _sTitle = value; }
        }
    }
    #endregion

    #region StoreDc
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class cStoreDc
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDCA([MarshalAs(UnmanagedType.LPStr)]string lpszDriver, [MarshalAs(UnmanagedType.LPStr)]string lpszDevice, [MarshalAs(UnmanagedType.LPStr)]string lpszOutput, int lpInitData);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDCW([MarshalAs(UnmanagedType.LPWStr)]string lpszDriver, [MarshalAs(UnmanagedType.LPWStr)]string lpszDevice, [MarshalAs(UnmanagedType.LPWStr)]string lpszOutput, int lpInitData);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, int lpInitData);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr hObject);

        private int _Height = 0;
        private int _Width = 0;
        private IntPtr _Hdc = IntPtr.Zero;
        private IntPtr _Bmp = IntPtr.Zero;
        private IntPtr _BmpOld = IntPtr.Zero;

        public IntPtr Hdc
        {
            get { return _Hdc; }
        }

        public IntPtr HBmp
        {
            get { return _Bmp; }
        }

        public int Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    ImageCreate(_Width, _Height);
                }
            }
        }

        public int Width
        {
            get { return _Width; }
            set
            {
                if (_Width != value)
                {
                    _Width = value;
                    ImageCreate(_Width, _Height);
                }
            }
        }

        private void ImageCreate(int Width, int Height)
        {
            IntPtr pHdc = IntPtr.Zero;

            ImageDestroy();
            pHdc = CreateDC("DISPLAY", "", "", 0);
            _Hdc = CreateCompatibleDC(pHdc);
            _Bmp = CreateCompatibleBitmap(pHdc, _Width, _Height);
            _BmpOld = SelectObject(_Hdc, _Bmp);
            if (_BmpOld == IntPtr.Zero)
            {
                ImageDestroy();
            }
            else
            {
                _Width = Width;
                _Height = Height;
            }
            DeleteDC(pHdc);
            pHdc = IntPtr.Zero;
        }

        private void ImageDestroy()
        {
            if (_BmpOld != IntPtr.Zero)
            {
                SelectObject(_Hdc, _BmpOld);
                _BmpOld = IntPtr.Zero;
            }
            if (_Bmp != IntPtr.Zero)
            {
                DeleteObject(_Bmp);
                _Bmp = IntPtr.Zero;
            }
            if (_Hdc != IntPtr.Zero)
            {
                DeleteDC(_Hdc);
                _Hdc = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            ImageDestroy();
        }
    }
    #endregion
}
