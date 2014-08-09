using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WSN.Ants.Controls.WSNAutoCompleteTextBox
{
    public partial class WSNAutoCompleteTextBox : WSNTextBox.WSNTextBox
    {
        private class WinHook : NativeWindow
        {
            private WSNAutoCompleteTextBox tb;

            public WinHook(WSNAutoCompleteTextBox tbox)
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
                            //Form form = tb.FindForm();
                            Control form = tb.Parent;
                            if (form != null)
                            {

                                Point p = form.PointToScreen(new Point((int)m.LParam));
                                Point p2 = tb.PointToScreen(new Point(0, 0));
                                Rectangle rect = new Rectangle(p2, tb.Size);
                                if (!rect.Contains(p))
                                {
                                    tb.HideList();
                                }
                            }
                        } break;
                    case Messages.WM_SIZE:
                    case Messages.WM_MOVE:
                        {
                            tb.HideList();
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
                                        //Form form = tb.FindForm();
                                        Control form = tb.Parent;
                                        if (form != null)
                                        {
                                            Point p = form.PointToScreen(new Point((int)m.LParam));
                                            Point p2 = tb.PointToScreen(new Point(0, 0));
                                            Rectangle rect = new Rectangle(p2, tb.Size);
                                            if (!rect.Contains(p))
                                            {
                                                tb.HideList();
                                            }
                                        }
                                    } break;
                            }
                        }
                        break;
                }

                base.WndProc(ref m);
            }
        }
    }
}
