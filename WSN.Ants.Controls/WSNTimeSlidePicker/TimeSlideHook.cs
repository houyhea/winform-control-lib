using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WSN.Ants.Controls.WSNTimeSlidePicker
{
    public partial class WSNTimeSlidePicker : TextBox
    {
        private const int WA_INACTIVE = 0;

        private class TimeSlideHook : NativeWindow
        {
            private class Messages
            {
                private Messages()
                { }

                public const int WM_PARENTNOTIFY = 0x210;
                public const int WM_RBUTTONDBLCLK = 0x206;
                public const int WM_RBUTTONDOWN = 0x204;
                public const int WM_RBUTTONUP = 0x205;
                public const int WM_LBUTTONDBLCLK = 0x203;
                public const int WM_LBUTTONDOWN = 0x201;
                public const int WM_LBUTTONUP = 0x202;
                public const int WM_MBUTTONDBLCLK = 0x209;
                public const int WM_MBUTTONDOWN = 0x207;
                public const int WM_MBUTTONUP = 0x208;
                public const int WM_NCLBUTTONDOWN = 0x00A1;
                public const int WM_NCLBUTTONUP = 0x00A2;
                public const int WM_NCLBUTTONDBLCLK = 0x00A3;
                public const int WM_NCRBUTTONDOWN = 0x00A4;
                public const int WM_NCRBUTTONUP = 0x00A5;
                public const int WM_NCRBUTTONDBLCLK = 0x00A6;
                public const int WM_NCMBUTTONDOWN = 0x00A7;
                public const int WM_NCMBUTTONUP = 0x00A8;
                public const int WM_NCMBUTTONDBLCLK = 0x00A9;
                public const int WM_SIZE = 0x0005;
                public const int WM_MOVE = 0x0003;
                public const int WM_KILLFOCUS = 0x0008;
                public const int WM_ACTIVATE = 0x0006;



            }

            private WSNTimeSlidePicker tb;

            public TimeSlideHook(WSNTimeSlidePicker tbox)
            {
                this.tb = tbox;
            }


            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case Messages.WM_LBUTTONDOWN:
                    case Messages.WM_LBUTTONDBLCLK:
                    case Messages.WM_MBUTTONDOWN:
                    case Messages.WM_MBUTTONDBLCLK:
                    case Messages.WM_RBUTTONDOWN:
                    case Messages.WM_RBUTTONDBLCLK:
                    case Messages.WM_NCLBUTTONDOWN:
                    case Messages.WM_NCMBUTTONDOWN:
                    case Messages.WM_NCRBUTTONDOWN:
                        {
                            //Control form = tb.FindForm();
                            //if (form == null)
                            Control form = tb.Parent;
                            if (form != null)
                            {
                                Point p = form.PointToScreen(new Point((int)m.LParam));
                                Point p2 = tb.PointToScreen(new Point(0, 0));
                                Rectangle rect = new Rectangle(p2, tb.Size);
                                if (!rect.Contains(p))
                                {
                                    tb.HideDropDown();
                                }
                            }
                        } break;


                    case Messages.WM_ACTIVATE:
                        {
                            // test if window is being activated 
                            if (LOWORD16(m.LParam.ToInt32()) == WA_INACTIVE)
                            {
                                tb.HideDropDown();
                            }
                        }
                        break;
                    case Messages.WM_KILLFOCUS:
                    case Messages.WM_SIZE:
                    case Messages.WM_MOVE:
                        {
                            tb.HideDropDown();
                        }
                        break;
                    // This is the message that gets sent when a childcontrol gets activity
                    case Messages.WM_PARENTNOTIFY:
                        {
                            switch ((int)m.WParam)
                            {
                                case Messages.WM_LBUTTONDOWN:
                                case Messages.WM_LBUTTONDBLCLK:
                                case Messages.WM_MBUTTONDOWN:
                                case Messages.WM_MBUTTONDBLCLK:
                                case Messages.WM_RBUTTONDOWN:
                                case Messages.WM_RBUTTONDBLCLK:
                                case Messages.WM_NCLBUTTONDOWN:
                                case Messages.WM_NCMBUTTONDOWN:
                                case Messages.WM_NCRBUTTONDOWN:
                                    {
                                        // Same thing as before
                                        //Control form = tb.FindForm();
                                        // if (form == null)
                                        Control form = tb.Parent;
                                        if (form != null)
                                        {
                                            Point p = form.PointToScreen(new Point((int)m.LParam));
                                            Point p2 = tb.PointToScreen(new Point(0, 0));
                                            Rectangle rect = new Rectangle(p2, tb.Size);
                                            if (!rect.Contains(p))
                                            {
                                                tb.HideDropDown();
                                            }
                                        }
                                    } break;
                            }
                        }
                        break;
                }

                base.WndProc(ref m);
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

            public static int HIWORD(int value)
            {
                return value >> 16;
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
}
