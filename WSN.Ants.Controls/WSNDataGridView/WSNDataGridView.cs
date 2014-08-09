using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Drawing;
using System.Data;

namespace WSN.Ants.Controls.WSNDataGridView
{
    /// <summary>
    /// WSN控件:
    ///     WSNDataGridView
    /// </summary>
    /// <remarks>在可自定义的网格中显示数据</remarks>
    /// <example>
    /// 以下示例展示如何使用WSNDataGridView
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNDataGridViewExamples.cs" region="WSNDataGridViewExample"/>
    /// </example>
    public class WSNDataGridView : DataGridView
    {
        private Color _columnHeaderColor1 = Color.FromArgb(245, 245, 245);
        private Color _columnHeaderColor2 = Color.FromArgb(225, 225, 225);

        private string msgText = "暂无相关数据";
        private bool isShowText = true;
        private bool isLoading = false;
        private bool isShowArrow = false;

        protected int msgBlockWith = 180;
        protected int msgBlockHeight = 120;

        private DataGridViewColumn oldSelectedColumn = null; //记录上一个排序过的列
        private SortOrder sortOrder = SortOrder.None; //记录上一个排序方法

        //private bool isLoadingData = false;
        /// <summary>
        /// 创建<c>WSNDataGridView</c>
        /// </summary>
        public WSNDataGridView()
        {
            PropertiesProSet();
        }

        //[DefaultValue(DataGridViewAutoSizeColumnsMode.Fill)]
        //public new DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
        //{
        //    get { return base.AutoSizeColumnsMode; }
        //    set { base.AutoSizeColumnsMode = value; }
        //}

        /// <summary>
        /// 控件属性：
        ///     获取或设置当列表内容为空时，显示的文字信息内容
        /// </summary>
        [Description("文字提示信息内容")]
        public string MsgText
        {
            get { return this.msgText; }
            set
            {
                this.msgText = value;
            }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置是否在没有数据的时候，DataGridView中显示提示信息
        /// </summary>
        [Description("是否显示提示信息内容")]
        public bool IsShowMsgText
        {
            get { return this.isShowText; }
            set { this.isShowText = value; }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置是否在列NotSortable模式下点击列头时显示列头的排序箭头
        /// </summary>
        [Description("是否显示排序箭头")]
        public bool IsShowSortArrow
        {
            get { return this.isShowArrow; }
            set { this.isShowArrow = value; }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置排序方法
        /// </summary>
        public SortOrder SortOrder
        {
            get { return this.sortOrder; }
            set { this.sortOrder = value; }
        }

        /// <summary>
        /// 显示加载数据提示
        /// </summary>
        public void LoadingData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(LoadingData));
            }
            else
            {
                this.isLoading = true;
                //this.msgText = "正在加载数据...";
                this.Refresh();
            }
        }

        /// <summary>
        /// 数据加载完后调用
        /// </summary>
        /// <remarks>如果没有任何数据，将显示无数据的提示信息</remarks>
        public void LoadFinished()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(LoadFinished));
            }
            else
            {
                this.isLoading = false;
                //this.msgText = "暂无相关数据";
                this.Refresh();
            }
        }

        /// <summary>
        /// 控件首次加载，未选择查询条件时，应当输出的提示信息
        /// </summary>
        public void BeforeDataLoad()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(BeforeDataLoad));
            }
            else
            {
                this.msgText = "请选择相关条件";
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);

                if (this.isShowText)
                {
                    if (this.DataSource == null)
                    {
                        this.msgText = "暂未加载数据";

                        PaintContent(e);
                    }
                    else
                    {
                        if (this.Rows.Count == 0)
                        {
                            if (isLoading)
                            {
                                this.msgText = "正在加载数据...";
                            }
                            else
                            {
                                this.msgText = "暂无相关数据";
                            }

                            PaintContent(e);
                        }
                    }
                }
            }
            catch
            { }
        }

        private void PaintContent(PaintEventArgs e)
        {
            int x = this.Location.X + this.Width / 2 - msgBlockWith / 2;
            int y = this.Location.Y + this.Height / 2 - msgBlockHeight / 2;

            using (GraphicsPath path = GetRoundedRectanglePath(x, y, msgBlockWith, msgBlockHeight, 4))
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(247, 247, 247)))
                {
                    e.Graphics.FillPath(brush, path);
                }

                using (Pen pen = new Pen(Color.FromArgb(179, 179, 179)))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }

            using (StringFormat sf = new StringFormat())
            {
                sf.FormatFlags = StringFormatFlags.NoWrap;
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.None;

                Rectangle stringRect = new Rectangle(x + 48, y + 53, 100, 20);
                e.Graphics.DrawString(this.msgText, Font, SystemBrushes.WindowText, stringRect, sf);
            }
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                if (!(this._columnHeaderColor1 == Color.Transparent) && !(this._columnHeaderColor2 == Color.Transparent) &&
                    !this._columnHeaderColor1.IsEmpty && !this._columnHeaderColor2.IsEmpty)
                {
                    DrawLinearGradient(e.CellBounds, e.Graphics, this._columnHeaderColor1, this._columnHeaderColor2);
                    e.Paint(e.ClipBounds, (DataGridViewPaintParts.All & ~DataGridViewPaintParts.Background));
                    e.Handled = true;

                    e.Graphics.DrawLine(new Pen(Color.FromArgb(179, 179, 179)),
                        new Point(e.CellBounds.X, e.CellBounds.Y + e.CellBounds.Height - 1),
                        new Point(e.CellBounds.Right, e.CellBounds.Y + e.CellBounds.Height - 1));
                }
            }

            //画标题栏的分隔线
            if (e.RowIndex == -1 && e.ColumnIndex > -1 && e.ColumnIndex < this.Columns.Count - 1)
            {
                Color Color1 = Color.FromArgb(0, 179, 179, 179);
                Color Color2 = Color.FromArgb(179, 179, 179);
                Color Color3 = Color.FromArgb(0, 179, 179, 179);

                if (e.CellBounds.Width > 0 && e.CellBounds.Height > 0)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        e.CellBounds, Color1, Color3, 90))
                    {
                        ColorBlend blend = new ColorBlend();
                        Color[] colors = new Color[3] { Color1, Color2, Color3 };
                        blend.Colors = colors;
                        blend.Positions = new float[] { 0f, .5f, 1f };
                        brush.InterpolationColors = blend;

                        using (Pen pen = new Pen(brush))
                        {
                            e.Graphics.DrawLine(pen, e.CellBounds.X + e.CellBounds.Width - 2, e.CellBounds.Y - 2,
                                e.CellBounds.X + e.CellBounds.Width - 2, e.CellBounds.Y + e.CellBounds.Height - 2);
                        }
                    }
                }
            }

            if (e.RowIndex == -1 && this.isShowArrow && this.Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.NotSortable)
            {
                if (this.oldSelectedColumn != null && this.oldSelectedColumn.Index > -1)
                {
                    //绘制排序的箭头
                    if (e.ColumnIndex == this.oldSelectedColumn.Index)
                    {
                        bool isUpArrow = false;
                        if (this.sortOrder == SortOrder.Ascending)
                        {
                            isUpArrow = true;
                        }

                        Rectangle rect = new Rectangle();
                        rect.X = e.CellBounds.X + e.CellBounds.Width - 14;
                        rect.Y = e.CellBounds.Y + 13;
                        rect.Width = 8;
                        rect.Height = 6;

                        using (GraphicsPath path = GetArrowPath(rect, isUpArrow))
                        {
                            using (Brush brush = new SolidBrush(Color.Silver))
                            {
                                e.Graphics.FillPath(brush, path);
                            }

                            using (Pen pen = new Pen(Color.Silver))
                            {
                                e.Graphics.DrawPath(pen, path);
                            }
                        }
                    }
                }

            }

            base.OnCellPainting(e);
        }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            //记录点击列的排序方式
            if (e.RowIndex == -1 && this.isShowArrow && this.Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.NotSortable)
            {
                DataGridViewColumn newColumn = this.Columns[e.ColumnIndex];
                DataGridViewColumn oldColumn = this.oldSelectedColumn;
                ListSortDirection direction;

                if (oldColumn != null)
                {
                    if (oldColumn == newColumn && this.sortOrder == SortOrder.Ascending)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        direction = ListSortDirection.Ascending;
                        oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                    }
                }
                else
                {
                    direction = ListSortDirection.Ascending;
                }

                this.oldSelectedColumn = newColumn;
                this.sortOrder = direction == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
            }

            base.OnCellClick(e);
        }

        private static void DrawLinearGradient(Rectangle Rec, Graphics Grp, Color Color1, Color Color2)
        {
            if (Color1 == Color2)
            {
                Brush backbrush = new SolidBrush(Color1);
                Grp.FillRectangle(backbrush, Rec);
            }
            else
            {
                if (Rec.Width > 0 && Rec.Height > 0)
                {
                    using (Brush backbrush = new LinearGradientBrush(Rec, Color1, Color2, LinearGradientMode.Vertical))
                    {
                        Grp.FillRectangle(backbrush, Rec);
                    }
                }
            }
        }

        #region 预设属性
        /// <summary>
        /// 预设属性
        /// </summary>
        private void PropertiesProSet()
        {
            #region
            System.Windows.Forms.DataGridViewCellStyle alternatingRowsStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle columnHeadersDefaultStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle defaultStyle = new System.Windows.Forms.DataGridViewCellStyle();

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeColumns = false;
            this.AllowUserToResizeRows = false;
            this.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            alternatingRowsStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            alternatingRowsStyle.BackColor = System.Drawing.Color.White;
            alternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            alternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.AlternatingRowsDefaultCellStyle = alternatingRowsStyle;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            columnHeadersDefaultStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            columnHeadersDefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(214)))));
            columnHeadersDefaultStyle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            columnHeadersDefaultStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            columnHeadersDefaultStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            columnHeadersDefaultStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            columnHeadersDefaultStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnHeadersDefaultCellStyle = columnHeadersDefaultStyle;
            this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ColumnHeadersHeight = 32;
            defaultStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            defaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            defaultStyle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            defaultStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            defaultStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            defaultStyle.SelectionForeColor = System.Drawing.Color.Black;
            defaultStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DefaultCellStyle = defaultStyle;
            this.EnableHeadersVisualStyles = false;
            this.MultiSelect = false;
            this.ReadOnly = true;
            this.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.RowHeadersVisible = false;
            this.RowTemplate.Height = 24;
            this.RowTemplate.ReadOnly = true;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.BackgroundColor = Color.FromArgb(247, 247, 247);
            this.AutoGenerateColumns = false;
            #endregion
        }

        #endregion

        /// <summary>
        /// 画圆角边框
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 箭头路径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="isUp"></param>
        /// <returns></returns>
        private GraphicsPath GetArrowPath(Rectangle rect, bool isUp)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            if (!isUp)
            {
                path.AddLine(rect.X, rect.Y, rect.X + rect.Width, rect.Y);
                path.AddLine(rect.X + rect.Width, rect.Y, rect.X + rect.Width / 2, rect.Y + rect.Height);
            }
            else
            {
                path.AddLine(rect.X, rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height);
                path.AddLine(rect.X + rect.Width, rect.Y + rect.Height, rect.X + rect.Width / 2, rect.Y);
            }
            path.CloseFigure();
            return path;
        }
    }
}
