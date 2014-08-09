using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace WSN.Ants.Controls.WSNTextBox
{
    /// <summary>
    /// WSN控件：
    ///     WSNTextBox文本框
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNTextBox
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNTextBoxExamples.cs" region="WSNTextBoxExample"/>
    /// </example>
    public class WSNTextBox : TextBox
    {
        #region Windows API
        /// <summary> 
        /// 获得当前进程，以便重绘控件 
        /// </summary> 
        /// <param name="hWnd"></param> 
        /// <returns></returns> 
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        #endregion

        #region 字段
        private FaceStyle _controlFaceSchema = new FaceStyle();
        private ControlState _controlState;
        private RoundStyle _roundStyle = RoundStyle.All;
        private int _radius = 8;
        private string _emptyTextTip;
        private Color _emptyTextTipColor = Color.DarkGray;
        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置空文本时显示文本
        /// </summary>
        /// <remarks>默认为："请输入"</remarks>
        [DefaultValue("请输入")]
        public string EmptyTextTip
        {
            get { return _emptyTextTip; }
            set
            {
                _emptyTextTip = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置空文本时显示文本的颜色
        /// </summary>
        /// <remarks>默认为：DarkGray</remarks>
        [DefaultValue(typeof(Color), "DarkGray")]
        public Color EmptyTextTipColor
        {
            get { return _emptyTextTipColor; }
            set
            {
                _emptyTextTipColor = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置控件外观样式
        /// </summary>
        public FaceStyle ControlFaceSchema
        {
            get { return _controlFaceSchema; }
            set { _controlFaceSchema = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置圆角路径的样式
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
        ///     获取或设置圆角半径
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

        #region 构造函数
        /// <summary>
        /// 构造函数:
        ///     创建<c>WSNTextBox</c>
        /// </summary>
        public WSNTextBox()
        {
            ControlFaceSchema.BorderNormalStyle.Color1 = Color.FromArgb(179, 179, 179);
            ControlFaceSchema.BorderHoverStyle.Color1 = Color.FromArgb(179, 179, 179);
            ControlFaceSchema.BorderDisabledStyle.Color1 = Color.FromArgb(100,100,100);
            //ControlFaceSchema.BorderDisabledStyle.Color1 = Color.FromArgb(255, 0,0);
            
        }
        #endregion
        #region 重载方法
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ControlState = ControlState.Hover;

        }
        /// <summary> 
        /// 当鼠标从该控件移开时 
        /// </summary> 
        /// <param name="e"></param> 
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlState = ControlState.Normal;
        }

        /// <summary> 
        /// 当该控件获得焦点时 
        /// </summary> 
        /// <param name="e"></param> 
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.Invalidate();
        }
        /// <summary> 
        /// 当该控件失去焦点时 
        /// </summary> 
        /// <param name="e"></param> 
        protected override void OnLostFocus(EventArgs e)
        {

            base.OnLostFocus(e);
            this.Invalidate();

        }

        /// <summary> 
        /// 获得操作系统消息 
        /// </summary> 
        /// <param name="m"></param> 
        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);
            if (m.Msg == 0xf || m.Msg == 0x133)
            {
                #region 注释
                //拦截系统消息，获得当前控件进程以便重绘。 
                //一些控件（如TextBox、Button等）是由系统进程绘制，重载OnPaint方法将不起作用. 
                //所有这里并没有使用重载OnPaint方法绘制TextBox边框。 
                // 
                //MSDN:重写 OnPaint 将禁止修改所有控件的外观。 
                //那些由 Windows 完成其所有绘图的控件（例如 Textbox）从不调用它们的 OnPaint 方法， 
                //因此将永远不会使用自定义代码。请参见您要修改的特定控件的文档， 
                //查看 OnPaint 方法是否可用。如果某个控件未将 OnPaint 作为成员方法列出， 
                //则您无法通过重写此方法改变其外观。 
                // 
                //MSDN:要了解可用的 Message.Msg、Message.LParam 和 Message.WParam 值， 
                //请参考位于 MSDN Library 中的 Platform SDK 文档参考。可在 Platform SDK（“Core SDK”一节） 
                //下载中包含的 windows.h 头文件中找到实际常数值，该文件也可在 MSDN 上找到。 
                #endregion
                IntPtr hDC = GetWindowDC(m.HWnd);
                if (hDC.ToInt32() == 0)
                {
                    return;
                }
                System.Drawing.Graphics g = Graphics.FromHdc(hDC);

                #region 画边框
                if (this.BorderStyle != BorderStyle.None)
                {
                    
                    //边框Width为1个像素 
                    System.Drawing.Pen pen = new Pen(ControlFaceSchema.BorderNormalStyle.Color1, 1);

                    if (this.Enabled)
                    {
                        if (ControlState == ControlState.Hover)
                        {
                            pen.Color = ControlFaceSchema.BorderHoverStyle.Color1;
                        }
                        else
                        {
                            if (this.Focused)
                            {
                                pen.Color = ControlFaceSchema.BorderHoverStyle.Color1;
                            }
                            else
                            {
                                pen.Color = ControlFaceSchema.BorderNormalStyle.Color1;
                            }
                        }
                    }
                    else
                    {
                        pen.Color = ControlFaceSchema.BorderDisabledStyle.Color1;
                    }

                    //绘制边框 
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                    pen.Dispose();
                }
                #endregion

                #region 画水印提示
                if (Text.Length == 0
                    && !string.IsNullOrEmpty(_emptyTextTip)
                    && !Focused 
                    && Enabled)
                {
                    TextFormatFlags format =
                        TextFormatFlags.EndEllipsis |
                        TextFormatFlags.VerticalCenter;

                    if (RightToLeft == RightToLeft.Yes)
                    {
                        format |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                    }

                    TextRenderer.DrawText(
                        g,
                        _emptyTextTip,
                        Font,
                        base.ClientRectangle,
                        _emptyTextTipColor,
                        format);
                }

                #endregion


                //返回结果 
                m.Result = IntPtr.Zero;
                //释放 
                ReleaseDC(m.HWnd, hDC);
                g.Dispose();
            }
        }
        #endregion
    }
}
