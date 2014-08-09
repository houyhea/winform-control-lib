using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace WSN.Ants.Controls.WSNDataGridView
{
    /// <summary>
    ///  WSNDataGridView的单元格信息
    /// </summary>
    /// <remarks>显示WSNDataGridView控件中可编辑文本信息</remarks>
    /// <example>
    /// 以下示例展示如何使用WSNDataGridViewTextImageCell
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNDataGridViewExamples.cs" region="WSNDataGridViewTextImageCellExample"/>
    /// </example>
    public class WSNDataGridViewTextImageCell : DataGridViewTextBoxCell
    {
        private string text = string.Empty;
        private Image normalimage = null;
        private Image hoverimage = null;
        private Image clickimage = null;
        private CellStatus cellstatus = CellStatus.Normal;

        private Image image = null;

        private Point mousePoint = new Point();
        private Rectangle buttonArea;
        private Rectangle imageArea;

        private bool clickEnable = true;

        #region 颜色
        private Color normalbordercolor = Color.Empty;
        private Color hoverbordercolor = Color.FromArgb(179, 179, 179);

        private Color normalbackcolor1 = Color.FromArgb(226, 226, 226);
        private Color normalbackcolor2 = Color.FromArgb(245, 245, 245);

        private Color hoverbackcolor1 = Color.FromArgb(255, 255, 255);
        private Color hoverbackcolor2 = Color.FromArgb(230, 230, 230);

        private Color clickbackcolor1 = Color.FromArgb(242, 242, 242);
        private Color clickbackcolor2 = Color.FromArgb(211, 211, 211);
        #endregion

        private int radius = 4;
        /// <summary>
        /// 创建<c>WSNDataGridViewTextImageCell</c>
        /// </summary>
        public WSNDataGridViewTextImageCell()
        {

        }

        #region 公有属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置在单元格内显示的文本
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置正常状态下在单元格内显示的图片
        /// </summary>
        public Image NormalImage
        {
            get { return this.normalimage; }
            set
            {
                this.normalimage = value;
            }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置鼠标悬停在单元格内显示的图片
        /// </summary>
        public Image HoverImage
        {
            get { return this.hoverimage; }
            set { this.hoverimage = value; }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置鼠标在单元格内点击时显示的图片
        /// </summary>
        public Image ClickImage
        {
            get { return this.clickimage; }
            set { this.clickimage = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置鼠标位置
        /// </summary>
        public Point MousePoint
        {
            get { return mousePoint; }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置单元格区域
        /// </summary>
        public Rectangle ButtonArea
        {
            get { return buttonArea; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置图片区域
        /// </summary>
        public Rectangle ImageArea
        {
            get { return imageArea; }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置是否可用
        /// </summary>
        public bool Enabled
        {
            get { return clickEnable; }
            set { clickEnable = value; }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置单元格状态
        /// </summary>
        public CellStatus CellStatus
        {
            get { return this.cellstatus; }
            set { this.cellstatus = value; }
        }
        #endregion
        /// <summary>
        ///  复制单元格
        /// </summary>
        /// <returns>单元格对象</returns>
        public override object Clone()
        {
            WSNDataGridViewTextImageCell c = base.Clone() as WSNDataGridViewTextImageCell;
            c.hoverimage = this.hoverimage;
            c.normalimage = this.normalimage;
            c.clickimage = this.clickimage;
            c.text = this.text;
            c.cellstatus = this.cellstatus;
            return c;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue,
            string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            buttonArea = cellBounds;

            if ((paintParts & DataGridViewPaintParts.Background) ==
            DataGridViewPaintParts.Background)
            {
                SolidBrush cellBackground;

                if ((cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                {
                    cellBackground =
                        new SolidBrush(cellStyle.SelectionBackColor);
                }
                else
                {

                    cellBackground =
                    new SolidBrush(cellStyle.BackColor);
                }
                graphics.FillRectangle(cellBackground, cellBounds);
                cellBackground.Dispose();
            }

            //Draw the cell borders, if specified.
            if ((paintParts & DataGridViewPaintParts.Border) ==
                DataGridViewPaintParts.Border)
            {
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                    advancedBorderStyle);
            }

            Rectangle borderRect = new Rectangle();
            if (buttonArea.Width > 80)
            {
                borderRect.X = buttonArea.X + (buttonArea.Width / 2 - 40);
                borderRect.Width = 80;
            }
            else
            {
                borderRect.X = buttonArea.X;
                borderRect.Width = buttonArea.Width;
            }
            borderRect.Y = buttonArea.Y;
            borderRect.Height = buttonArea.Height;

            if (!clickEnable)
            {
                //this.cellstatus = _2MonitorEquipment.CellStatus.Disable;
                using (GraphicsPath path = GetRoundedRectanglePath(borderRect.X, borderRect.Y, borderRect.Width - 1, borderRect.Height - 1, this.radius))
                {
                    using (Brush hbrush = new LinearGradientBrush(borderRect, Color.Silver, Color.Silver, 90))
                    {
                        graphics.FillPath(hbrush, path);
                    }

                    using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
                    {
                        graphics.DrawPath(pen, path);
                    }
                }

                image = this.normalimage;
            }
            else
            {
                switch (this.cellstatus)
                {
                    #region 画背景，边框，文字，图片
                    case CellStatus.Normal:
                        image = this.normalimage;
                        break;
                    case CellStatus.Hover:
                        //画背景和边框
                        using (GraphicsPath path = GetRoundedRectanglePath(borderRect.X, borderRect.Y, borderRect.Width - 1, borderRect.Height - 1, this.radius))
                        {
                            using (Brush hbrush = new LinearGradientBrush(borderRect, this.hoverbackcolor1, this.hoverbackcolor2, 90))
                            {
                                graphics.FillPath(hbrush, path);
                            }

                            using (Pen pen = new Pen(this.hoverbordercolor))
                            {
                                graphics.DrawPath(pen, path);
                            }
                        }

                        if (this.hoverimage != null)
                        {
                            image = this.hoverimage;
                        }
                        else
                        {
                            image = this.normalimage;
                        }
                        break;
                    case CellStatus.Click:
                        //画背景和边框
                        using (GraphicsPath path = GetRoundedRectanglePath(borderRect.X, borderRect.Y, borderRect.Width - 1, borderRect.Height - 1, this.radius))
                        {
                            using (Brush hbrush = new LinearGradientBrush(borderRect, this.clickbackcolor1, this.clickbackcolor2, 90))
                            {
                                graphics.FillPath(hbrush, path);
                            }

                            using (Pen pen = new Pen(this.hoverbordercolor))
                            {
                                graphics.DrawPath(pen, path);
                            }
                        }

                        if (this.clickimage != null)
                        {
                            image = this.clickimage;
                        }
                        else
                        {
                            image = this.normalimage;
                        }
                        break;
                    case CellStatus.Disable:
                        using (GraphicsPath path = GetRoundedRectanglePath(borderRect.X, borderRect.Y, borderRect.Width - 1, borderRect.Height - 1, this.radius))
                        {
                            using (Brush hbrush = new LinearGradientBrush(borderRect, Color.Silver, Color.Silver, 90))
                            {
                                graphics.FillPath(hbrush, path);
                            }

                            using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
                            {
                                graphics.DrawPath(pen, path);
                            }
                        }

                        image = this.normalimage;
                        break;
                    default:
                        image = this.normalimage;
                        break;
                    #endregion
                }
            }
            // Calculate the area in which to draw the button.
            // Adjusting the following algorithm and values affects
            // how the image will appear on the button.

            //buttonArea = cellBounds;

            Rectangle buttonAdjustment =
                BorderWidths(advancedBorderStyle);

            buttonArea.X += buttonAdjustment.X;
            buttonArea.Y += buttonAdjustment.Y;
            buttonArea.Height -= buttonAdjustment.Height;
            buttonArea.Width -= buttonAdjustment.Width;

            Font f = new Font("宋体", 10, FontStyle.Regular);
            int TIWidth = 0, ImageHeight = 0, StringHeight = 0;
            if (image != null)
            {
                TIWidth += image.Width + 2;
                ImageHeight = image.Height;
            }
            if (!string.IsNullOrEmpty(text))
            {
                SizeF rec = graphics.MeasureString(text, f);
                TIWidth += 10;
                TIWidth += (int)rec.Width;
                StringHeight = (int)rec.Height;
            }

            //Rectangle imageArea;
            int offsetX = (buttonArea.Width - TIWidth) / 2;
            int offsetY = (buttonArea.Height - ImageHeight) / 2;
            if (image != null)
            {
                imageArea = new Rectangle(
                    buttonArea.X + offsetX,
                    buttonArea.Y + offsetY,
                    image.Width,
                    image.Height);
                graphics.DrawImage(image, imageArea);
            }

            if (!string.IsNullOrEmpty(text))
            {
                Point point;
                if (image == null)
                {
                    SizeF size = graphics.MeasureString(text, f);
                    offsetX = buttonArea.X + (buttonArea.Width - Convert.ToInt32(size.Width)) / 2;
                    //offsetX = buttonArea.X + (buttonArea.Width - TIWidth - 10) / 2;

                    offsetY = buttonArea.Y + (buttonArea.Height - StringHeight) / 2 + 2;
                    point = new Point(offsetX, offsetY);
                }
                else
                {
                    offsetX = buttonArea.X + (buttonArea.Width - TIWidth) / 2 + image.Width + 2;
                    offsetY = buttonArea.Y + (buttonArea.Height - StringHeight) / 2 + 2;
                    point = new Point(offsetX, offsetY);

                }
                SolidBrush TextBackground = new SolidBrush(Color.Black);
                graphics.DrawString(text, f, TextBackground, point);
            }
        }

        protected override void OnMouseEnter(int rowIndex)
        {
            base.OnMouseEnter(rowIndex);
            if (this.normalimage != null && this.hoverimage != null)
            {
                //this.cellstatus = CellStatus.Hover;
                this.DataGridView.Cursor = Cursors.Hand;
            }
            if (this.text != string.Empty)
            {
                //this.cellstatus = CellStatus.Hover;
                this.DataGridView.Cursor = Cursors.Hand;
            }
            this.DataGridView.InvalidateCell(this);

        }

        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);
            if (this.normalimage != null && this.hoverimage != null)
            {
                //this.cellstatus = CellStatus.Normal;
                this.DataGridView.Cursor = Cursors.Default;
            }
            if (this.text != string.Empty)
            {
                //this.cellstatus = CellStatus.Normal;
                this.DataGridView.Cursor = Cursors.Default;
            }
            this.DataGridView.InvalidateCell(this);
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.normalimage != null && this.hoverimage != null && this.clickimage != null)
            {
                this.cellstatus = CellStatus.Click;
                this.DataGridView.InvalidateCell(this);
            }

            mousePoint.X = e.X;
            mousePoint.Y = e.Y;
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseUp(e);
            //if (this.normalimage != null && this.hoverimage != null && this.clickimage != null)
            //{
            //    this.cellstatus = CellStatus.Hover;
            //    this.DataGridView.InvalidateCell(this);
            //}
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            mousePoint.X = e.X;
            mousePoint.Y = e.Y;
        }

        private GraphicsPath GetRoundedRectanglePath(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(x + radius, y, x + width - radius, y);
            if (radius > 0)
                path.AddArc(x + width - 2 * radius, y, 2 * radius, 2 * radius, 270.0f, 90.0f);
            path.AddLine(x + width, y + radius, x + width, y + height - radius);
            if (radius > 0)
                path.AddArc(x + width - 2 * radius, y + height - 2 * radius, 2 * radius, 2 * radius, 0.0f, 90.0f);
            path.AddLine(x + width - radius, y + height, x + radius, y + height);
            if (radius > 0)
                path.AddArc(x, y + height - 2 * radius, 2 * radius, 2 * radius, 90.0f, 90.0f);
            path.AddLine(x, y + height - radius, x, y + radius);
            if (radius > 0)
                path.AddArc(x, y, 2 * radius, 2 * radius, 180.0f, 90.0f);
            return path;
        }


        internal void SetText(int rowIndex, string value)
        {
            this.Text = value;
        }

        internal void SetNormalImage(int rowIndex, Image value)
        {
            this.NormalImage = value;
        }

        internal void SetHoverImage(int rowIndex, Image value)
        {
            this.HoverImage = value;
        }
    }
    /// <summary>
    /// 单元格状态
    /// </summary>
    public enum CellStatus
    {
        /// <summary>
        /// 正常状态
        /// </summary>
        Normal,
        /// <summary>
        /// 鼠标悬停状态
        /// </summary>
        Hover,
        /// <summary>
        /// 鼠标点击状态
        /// </summary>
        Click,
        /// <summary>
        /// 禁用状态
        /// </summary>
        Disable
    }
}
