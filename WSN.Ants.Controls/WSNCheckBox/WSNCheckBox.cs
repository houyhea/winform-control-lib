using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace WSN.Ants.Controls.WSNCheckBox
{
    /// <summary>
    /// WSN控件：
    ///     单选框控件<c>WSNCheckBox</c>
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNCheckBox
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNCheckBoxExamples.cs" region="WSNCheckBoxExample"/>
    /// </example>
    public class WSNCheckBox : CheckBox
    {
        #region 字段
        private ControlState _controlState;

        private Cursor _oldCursor;
        private static readonly ContentAlignment RightAlignment =
            ContentAlignment.TopRight |
            ContentAlignment.BottomRight |
            ContentAlignment.MiddleRight;
        private static readonly ContentAlignment LeftAligbment =
            ContentAlignment.TopLeft |
            ContentAlignment.BottomLeft |
            ContentAlignment.MiddleLeft;



        private CheckImageTable _checkImageTable = new CheckImageTable();

        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置单选框各状态图片列表
        /// </summary>
        public CheckImageTable CheckImageTable
        {
            get { return _checkImageTable; }
            set { _checkImageTable = value; }
        }

        internal ControlState ControlState
        {
            get { return _controlState; }
            set { _controlState = value; }
        }

        protected virtual int DefaultCheckButtonWidth
        {
            get { return 16; }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 创建单选框控件<c>WSNCheckBox</c>
        /// </summary>
        public WSNCheckBox()
            : base()
        {

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            //载入图片
            //Assembly assembly = GetType().Assembly;
            //System.IO.Stream streamSmall = assembly.GetManifestResourceStream("WSN.Ants.Controls.WSNCheckBox.Check_c.jpg");
            ////CheckImageTable.CheckedImage = new Bitmap(streamSmall);
            CheckImageTable.CheckedImage = new Bitmap(typeof(WSNCheckBox), "Check_c.png");
            CheckImageTable.CheckedImageDisabled = new Bitmap(typeof(WSNCheckBox), "Check_c_d.png");//(typeof(WSNCheckBox), "Check_c_d.png");

            CheckImageTable.UnCheckedImage = new Bitmap(typeof(WSNCheckBox), "Check.png");
            CheckImageTable.UnCheckedImageDisabled = new Bitmap(typeof(WSNCheckBox), "Check_d.png");

            CheckImageTable.IndeterminateImage = new Bitmap(typeof(WSNCheckBox), "Indeterminate.png");
            CheckImageTable.IndeterminateImageDisabled = new Bitmap(typeof(WSNCheckBox), "Indeterminate_d.png");

            ControlState = ControlState.Normal;
            _oldCursor = this.Cursor;
        }


        #endregion

        #region 重载方法
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _oldCursor = this.Cursor;
            ControlState = ControlState.Hover;
            Cursor = Cursors.Hand;

        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlState = ControlState.Normal;
            Cursor = _oldCursor;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                ControlState = ControlState.Pressed;
            }
            //Checked = !Checked;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (ClientRectangle.Contains(e.Location))
                {
                    ControlState = ControlState.Hover;
                }
                else
                {
                    ControlState = ControlState.Normal;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.OnPaintBackground(e);

            Graphics g = e.Graphics;
            Rectangle checkButtonRect;
            Rectangle textRect;


            g.SmoothingMode = SmoothingMode.AntiAlias;
            CalculateRect(out checkButtonRect, out textRect);


            //开始画UI元素
            Image img = CheckImageTable.UnCheckedImage;
            img = GetImageByState();
            if (img != null)
            {
                //g.DrawImageUnscaled(img, base.ClientRectangle.X, base.ClientRectangle.Y, img.Width, img.Height);
                g.DrawImageUnscaled(img, checkButtonRect);
            }

            Color textColor = Enabled ? ForeColor : SystemColors.GrayText;
            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRect,
                textColor,
                GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));

        }

        #endregion

        #region 私有方法

        private Image GetImageByState()
        {
            Image im = null;
            switch (this.CheckState)
            {
                case System.Windows.Forms.CheckState.Checked:
                    im = Enabled ? CheckImageTable.CheckedImage : CheckImageTable.CheckedImageDisabled;
                    break;
                case System.Windows.Forms.CheckState.Unchecked:
                    im = Enabled ? CheckImageTable.UnCheckedImage : CheckImageTable.UnCheckedImageDisabled;
                    break;
                case System.Windows.Forms.CheckState.Indeterminate:
                    im = Enabled ? CheckImageTable.IndeterminateImage : CheckImageTable.IndeterminateImageDisabled;
                    break;
                default: break;
            }
            return im;
        }

        private void CalculateRect(
             out Rectangle checkButtonRect, out Rectangle textRect)
        {
            checkButtonRect = new Rectangle(
                0, 0, DefaultCheckButtonWidth, DefaultCheckButtonWidth);
            textRect = Rectangle.Empty;
            bool bCheckAlignLeft = (int)(LeftAligbment & CheckAlign) != 0;
            bool bCheckAlignRight = (int)(RightAlignment & CheckAlign) != 0;
            bool bRightToLeft = RightToLeft == RightToLeft.Yes;



            if ((bCheckAlignLeft && !bRightToLeft) ||
                (bCheckAlignRight && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopRight:
                    case ContentAlignment.TopLeft:
                        checkButtonRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        checkButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        checkButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                checkButtonRect.X = 0;

                textRect = new Rectangle(
                    checkButtonRect.Right + 2,
                    0,
                    Width - checkButtonRect.Right - 4,
                    Height);
            }
            else if ((bCheckAlignRight && !bRightToLeft)
                || (bCheckAlignLeft && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        checkButtonRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        checkButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        checkButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                checkButtonRect.X = Width - DefaultCheckButtonWidth - 0;

                textRect = new Rectangle(
                    2, 0, Width - DefaultCheckButtonWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        checkButtonRect.Y = 2;
                        textRect.Y = checkButtonRect.Bottom + 2;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;
                    case ContentAlignment.MiddleCenter:
                        checkButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;
                    case ContentAlignment.BottomCenter:
                        checkButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;
                }

                checkButtonRect.X = (Width - DefaultCheckButtonWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }
        internal static TextFormatFlags GetTextFormatFlags(
            ContentAlignment alignment,
            bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }
        #endregion
    }
}
