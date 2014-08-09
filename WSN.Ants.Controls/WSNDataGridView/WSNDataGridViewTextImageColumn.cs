using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace WSN.Ants.Controls.WSNDataGridView
{
    /// <summary>
    /// WSN控件：
    ///    WSNDataGridView列
    /// </summary>
    /// <remarks>承载了WSNDataGridViewTextImageCell的一个集合</remarks>
    /// <example>
    /// 以下示例展示如何使用WSNDataGridViewTextImageColumn
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNDataGridViewExamples.cs" region="WSNDataGridViewTextImageColumnExample"/>
    /// </example>
    public class WSNDataGridViewTextImageColumn : DataGridViewTextBoxColumn
    {
        /// <summary>
        ///  创建<c>WSNDataGridViewTextImageColumn</c>
        /// </summary>
        public WSNDataGridViewTextImageColumn()
        {
            this.CellTemplate = new WSNDataGridViewTextImageCell();
        }

        #region 属性
        [Category("自定义属性")]
        [Description("显示文本")]
        [DisplayName("显示文本")]
        /// <summary>
        /// 控件属性：
        ///     获取或设置显示文本
        /// </summary>
        public string Text
        {
            get
            {
                if (this.ImageButtonCellTemplate == null)
                {
                    throw new InvalidOperationException("ImageButtonCellTemplate为空.");
                }
                return this.ImageButtonCellTemplate.Text;
            }
            set
            {
                if (this.ImageButtonCellTemplate == null)
                {
                    throw new InvalidOperationException("ImageButtonCellTemplate为空.");
                }
                this.ImageButtonCellTemplate.Text = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        WSNDataGridViewTextImageCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as WSNDataGridViewTextImageCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetText(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }

            }
        }

        [Category("自定义属性")]
        [Description("显示图片.normal状态")]
        [DisplayName("显示Normal图片")]
        /// <summary>
        /// 控件属性：
        ///     获取或设置显示图片NORMAL
        /// </summary>
        public Image NormalImage
        {
            get
            {
                if (this.ImageButtonCellTemplate == null)
                {
                    throw new InvalidOperationException("ImageButtonCellTemplate为空.");
                }
                return this.ImageButtonCellTemplate.NormalImage;
            }
            set
            {
                if (this.ImageButtonCellTemplate == null)
                {
                    throw new InvalidOperationException("ImageButtonCellTemplate为空.");
                }
                this.ImageButtonCellTemplate.NormalImage = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        WSNDataGridViewTextImageCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as WSNDataGridViewTextImageCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetNormalImage(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }

            }
        }



        [Category("自定义属性")]
        [Description("显示图片.hover状态")]
        [DisplayName("显示Hover图片")]
        /// <summary>
        /// 控件属性：
        ///     获取或设置显示图片HOVER
        /// </summary>
        public Image HoverImage
        {
            get
            {
                if (this.ImageButtonCellTemplate == null)
                {
                    throw new InvalidOperationException("ImageButtonCellTemplate为空.");
                }
                return this.ImageButtonCellTemplate.HoverImage;
            }
            set
            {
                if (this.ImageButtonCellTemplate == null)
                {
                    throw new InvalidOperationException("ImageButtonCellTemplate为空.");
                }
                this.ImageButtonCellTemplate.HoverImage = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        WSNDataGridViewTextImageCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as WSNDataGridViewTextImageCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetHoverImage(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }

            }
        }
        #endregion
        /// <summary>
        /// 控件属性：
        ///     获取或设置单元格样式
        /// </summary>
        /// <exception cref="InvalidCastException">
        /// 当赋值时，值不为null且该值不是<c>WSNDataGridViewTextImageCell</c>类型时抛出
        /// </exception>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(WSNDataGridViewTextImageCell)))
                {
                    throw new InvalidCastException("必须是 WSNDataGridViewTextImageCell 类型");
                }
                base.CellTemplate = value;
            }
        }
        private WSNDataGridViewTextImageCell ImageButtonCellTemplate
        {
            get
            {
                return (WSNDataGridViewTextImageCell)this.CellTemplate;
            }
        }
    }

}
