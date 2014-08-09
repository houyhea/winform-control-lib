using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
namespace WSN.Ants.Controls.WSNAutoCompleteTextBox
{
    public partial class WSNAutoCompleteTextBox : WSNTextBox.WSNTextBox
    {
        private class WSNAutoCompleteListBox : ListBox
        {
            public Color SelectBackColor { get; set; }
            
            /// <summary>
            /// 自画item前事件。用户可以在这里设置显示的Text
            /// </summary>
            public event EventHandler<WSNAutoCompleteListBoxDrawingItemEventArgs> DrawingItem;
            private void OnDrawingItem(WSNAutoCompleteListBoxDrawingItemEventArgs e)
            {
                if (DrawingItem != null)
                {
                    DrawingItem(this, e);
                    
                }
               
            }


            public WSNAutoCompleteListBox()
                : base()
            {
                base.DrawMode = DrawMode.OwnerDrawFixed;
                SelectBackColor = Color.FromArgb(217, 217, 217);
            }

            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                base.OnDrawItem(e);
                if (Items.Count <= 0)
                    return;

                if (e.Index != -1)
                {

                    Color bColor = e.BackColor;

                    if ((e.State & DrawItemState.Selected)
                        == DrawItemState.Selected)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(SelectBackColor), e.Bounds);
                    }
                    else
                    {
                        Color backColor;
                        backColor = Color.White;
                        using (SolidBrush brush = new SolidBrush(Color.White))
                        {
                            e.Graphics.FillRectangle(brush, e.Bounds);
                        }
                    }
                    #region 设置显示Text
                    
                    //提供一个委托接口，让外面改变显示的文本
                    WSNAutoCompleteListBoxDrawingItemEventArgs drawingItemEventArgs = new WSNAutoCompleteListBoxDrawingItemEventArgs(GetItemText(Items[e.Index]), Items[e.Index]);
                    OnDrawingItem(drawingItemEventArgs);
                    
                    #endregion



                    TextFormatFlags formatFlags = TextFormatFlags.VerticalCenter;
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        formatFlags |= TextFormatFlags.RightToLeft;
                    }
                    else
                    {
                        formatFlags |= TextFormatFlags.Left;
                    }

                    TextRenderer.DrawText(
                        e.Graphics,
                        drawingItemEventArgs.Text,
                        Font,
                        new Rectangle((e.Bounds.Width > 5) ? (e.Bounds.X + 5) : (e.Bounds.X), e.Bounds.Y, e.Bounds.Width, e.Bounds.Height),
                        ForeColor,
                        formatFlags);
                }
            }

            //protected override void OnMouseMove(MouseEventArgs e)
            //{
            //    base.OnMouseMove(e);
            //    Point mousePoint = new Point(e.X, e.Y);

            //    for (int i = 0; i < Items.Count; i++)
            //    {
            //        Rectangle itemRect = GetItemRectangle(i);
            //        if (itemRect.Contains(mousePoint))
            //        {
            //            SelectedIndex = i;
            //            return;
            //        }
            //    }

            //}
        }
        /// <summary>
        /// 自画item前事件参数类
        /// </summary>
        public class WSNAutoCompleteListBoxDrawingItemEventArgs : EventArgs
        {       
            /// <summary>
            /// 事件源：
            ///     当前ITEM
            /// </summary>
            public object Item { get; set; }
            
            /// <summary>
            /// 事件数据：
            ///     显示的Text
            /// </summary>
            public string Text{get;set;}

            /// <summary>
            /// 新建自画item前事件参数类
            /// </summary>
            /// <param name="text">显示的Text</param>
            /// <param name="item">当前ITEM</param>
            public WSNAutoCompleteListBoxDrawingItemEventArgs(string text, object item)
            {
                this.Text = text;
                Item = item;
                
                
            }
            
        }
        
    }
}
