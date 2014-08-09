using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WSN.Ants.Controls.WSNAutoCompleteTextBox
{
    /// <summary>
    /// WSN控件：
    ///     WSNAutoCompleteTextBox文本框
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNAutoCompleteTextBox
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNAutoCompleteTextBoxExamples.cs" region="WSNAutoCompleteTextBoxExample"/>
    /// </example>
    [Serializable]
    [DefaultEvent("InputChanged")]
    public partial class WSNAutoCompleteTextBox : WSNTextBox.WSNTextBox
    {
        /// <summary>
        /// 显示模式
        /// </summary>
        public enum EntryMode
        {
            /// <summary>
            /// 文本
            /// </summary>
            Text,
            /// <summary>
            /// 列表
            /// </summary>
            List
        }
        #region Win32

        const Int32 HWND_TOPMOST = -1;
        const Int32 SWP_NOACTIVATE = 0x0010;
        const Int32 SW_SHOWNOACTIVATE = 4;
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        protected static extern bool SetWindowPos(IntPtr hWnd,
                                                  Int32 hWndInsertAfter
                                                    , Int32 X
                                                    , Int32 Y
                                                    , Int32 cx
                                                    , Int32 cy
                                                    , uint uFlags);



        //[DllImport("user32.dll")]
        //public static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region 事件
        /// <summary>
        /// 输入改变事件，外面注册该事件，以设置搜索结果列表
        /// </summary>
        public event EventHandler InputChanged;

        protected void OnInputChanged(EventArgs e)
        {
            if (InputChanged != null)
                InputChanged(this, e);
        }
        /// <summary>
        /// Enter键按下事件
        /// </summary>
        public event EventHandler EnterKeyDown;
        protected void OnEnterKeyDown(EventArgs e)
        {
            if (EnterKeyDown != null)
            {
                EnterKeyDown(this, e);
            }
        }
        [Description("数据输入完成事件.当用户输入完一段文本后，控件自动判断是否输入完毕，然后触发该事件")]
        /// <summary>
        /// 数据输入完成事件
        /// </summary>
        public event EventHandler<DataInputCompletedEventArgs> DataInputCompleted;
        protected void OnDataInputCompleted(DataInputCompletedEventArgs e)
        {
            if (DataInputCompleted != null)
            {
                DataInputCompleted(this, e);
            }
        }
        [Description("自动完成列表ITEM自绘前事件（这里设置显示的TEXT）")]
        /// <summary>
        /// 自动完成列表ITEM自绘前事件（这里设置显示的TEXT）
        /// </summary>
        public event EventHandler<WSNAutoCompleteListBoxDrawingItemEventArgs> ListDrawingItem
        {
            add
            {
                if (list != null)
                {
                    list.DrawingItem += value;
                }
            }
            remove
            {
                if (list != null)
                {
                    list.DrawingItem -= value;
                }
            }
        }
        [Description("下拉列表选择项填充到自动完成文本事件")]
        /// <summary>
        /// 下拉列表选择项填充到自动完成文本事件
        /// </summary>
        public event EventHandler<SelectItemEventArgs> FillingAutoCompleteTextEvent;
        /// <summary>
        /// 触发下拉列表选择项填充到自动完成文本事件。这里让外部注册事件决定填充选中项的哪个数据作为自动完成输入文本。
        /// </summary>
        /// <param name="e"></param>
        private void OnFillingAutoCompleteTextEvent(SelectItemEventArgs e)
        {
            if (FillingAutoCompleteTextEvent != null)
            {
                FillingAutoCompleteTextEvent(this, e);

            }

        }

        #endregion

        #region 字段

        //输入完成的上一次文本，用于输入完成事件对比
        private string oldText;


        private WSNAutoCompleteListBox list;
        protected Form popup;
        private WinHook hook;
        private WSNAutoCompletePanel panel;
        #endregion


        #region 延时自动完成

        private int inputChangeTriggerTime;
        /// <summary>
        /// 单位：毫秒，0《 s 《  5000
        /// </summary>
        public int InputChangeTriggerTime
        {
            get { return inputChangeTriggerTime; }
            set
            {
                inputChangeTriggerTime = value;
                if (inputChangeTriggerTime > 5000)
                    inputChangeTriggerTime = 5000;
                if (inputChangeTriggerTime < 0)
                    inputChangeTriggerTime = 0;
            }
        }

        private DateTime lastInputTriggerTime = DateTime.Now;
        private Timer timerTriggerInputChanged;
        #endregion


        #region 属性

        /// <summary>
        /// 控件属性：
        ///     获取或设置空格键是否有效。
        /// </summary>
        public bool IsSpaceKeyCodeValid
        {
            get;
            set;
        }

        private EntryMode mode = EntryMode.Text;
        /// <summary>
        /// 控件属性：
        ///     获取或设置显示模式
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public EntryMode Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;
            }
        }



        /// <summary>
        /// 控件属性：
        ///     获取搜索结果集合（外部通过InputChanged事件填充）
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public System.Windows.Forms.ListBox.ObjectCollection Items
        {
            get { return list.Items; }
        }


        /// <summary>
        /// 控件属性：
        ///     获取或设置自动完成触发器集合
        /// </summary>
        private AutoCompleteTriggerCollection triggers = new AutoCompleteTriggerCollection();
        [System.ComponentModel.Editor(typeof(AutoCompleteTriggerCollection.AutoCompleteTriggerCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public AutoCompleteTriggerCollection Triggers
        {
            get
            {
                return this.triggers;
            }
            set
            {
                this.triggers = value;
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置弹出窗体宽度
        /// </summary>
        [Browsable(true)]
        [Description("The width of the popup (-1 will auto-size the popup to the width of the textbox).")]
        public int PopupWidth
        {
            get
            {
                return this.popup.Width;
            }
            //set
            //{
            //    if (value == -1)
            //    {
            //        this.popup.Width = this.Width;
            //    }
            //    else
            //    {
            //        this.popup.Width = value;
            //    }
            //}
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置弹出框边框颜色
        /// </summary>
        public Color PopupBorderColor
        {
            get
            {
                return this.panel.BorderColor;
            }
            set
            {
                this.panel.BorderColor = value;
                Invalidate();
            }
        }

        private Point popOffset = new Point(0, 0);
        /// <summary>
        /// 控件属性：
        ///     获取或设置弹出框偏移量 
        /// </summary>
        [Description("The popup defaults to the lower left edge of the textbox.")]
        public Point PopupOffset
        {
            get
            {
                return this.popOffset;
            }
            set
            {
                this.popOffset = value;
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置弹出选项背景色
        /// </summary>
        public Color PopupSelectionBackColor
        {
            get
            {
                return this.list.SelectBackColor;
            }
            set
            {
                this.list.SelectBackColor = value;
                Invalidate();
            }
        }


        private bool triggersEnabled = true;
        protected bool TriggersEnabled
        {
            get
            {
                return this.triggersEnabled;
            }
            set
            {
                this.triggersEnabled = value;
            }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置文本框内文本值
        /// </summary>
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                this.TriggersEnabled = false;
                base.Text = value;
                this.TriggersEnabled = true;

            }
        }

        #endregion

        #region 构造函数
        public WSNAutoCompleteTextBox()
        {
            // Create the form that will hold the list
            this.popup = new Form();
            this.popup.SuspendLayout();
            this.popup.StartPosition = FormStartPosition.Manual;
            this.popup.ShowInTaskbar = false;
            this.popup.FormBorderStyle = FormBorderStyle.None;
            //this.popup.TopMost = true;
            this.popup.ResumeLayout();

            this.popup.Deactivate += new EventHandler(Popup_Deactivate);

            // Create the list box that will hold mathcing items
            this.list = new WSNAutoCompleteListBox();
            this.list.Cursor = Cursors.Hand;
            this.list.BorderStyle = BorderStyle.None;
            this.list.SelectedIndexChanged += new EventHandler(List_SelectedIndexChanged);
            this.list.MouseDown += new MouseEventHandler(List_MouseDown);
            this.list.ItemHeight = 20;
            this.list.Dock = DockStyle.Fill;

            panel = new WSNAutoCompletePanel();
            this.popup.Controls.Add(panel);
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(1);

            // Add the list box to the popup form
            panel.Controls.Add(this.list);

            // Add default triggers.
            this.triggers.Add(new TextLengthTrigger(1));
            this.triggers.Add(new ShortCutTrigger(Keys.Enter, TriggerState.SelectAndConsume));
            this.triggers.Add(new ShortCutTrigger(Keys.Tab, TriggerState.Select));
            //this.triggers.Add(new ShortCutTrigger(Keys.Control | Keys.Space, TriggerState.ShowAndConsume));
            this.triggers.Add(new ShortCutTrigger(Keys.Escape, TriggerState.HideAndConsume));

            oldText = this.Text;
            this.LostFocus += new EventHandler(WSNAutoCompleteTextBox_LostFocus);

            InitTimer();
            IsSpaceKeyCodeValid = false;
            
        }

        private void InitTimer()
        {
            if (timerTriggerInputChanged == null)
            {
                InputChangeTriggerTime = 600;
                timerTriggerInputChanged = new Timer();
                timerTriggerInputChanged.Enabled = false;
                timerTriggerInputChanged.Interval = 100;
                timerTriggerInputChanged.Tick += new EventHandler(timerTriggerInputChanged_Tick);
            }
        }

        void timerTriggerInputChanged_Tick(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(timerTriggerInputChanged_Tick), sender, e);
                return;
            }
            DateTime dt = lastInputTriggerTime.AddMilliseconds(inputChangeTriggerTime);

            //小于零 t1 早于 t2。 零 t1 与 t2 相同。 大于零 t1 晚于 t2
            int t = DateTime.Compare(dt, DateTime.Now);

            if (t <= 0)
            {

                timerTriggerInputChanged.Enabled = false;
                switch (this.Triggers.OnTextChanged(this.Text))
                {
                    case TriggerState.Show:
                        {
                            this.ShowList();
                        } break;
                    case TriggerState.Hide:
                        {
                            this.HideList();
                        } break;
                    default:
                        {
                            this.UpdateList();
                        } break;
                }
            }
        }


        #endregion

        #region 重载方法


        protected virtual bool DefaultCmdKey(ref Message msg, Keys keyData)
        {
            bool val = base.ProcessCmdKey(ref msg, keyData);

            if (this.TriggersEnabled)
            {
                switch (this.Triggers.OnCommandKey(keyData))
                {
                    case TriggerState.ShowAndConsume:
                        {
                            val = true;
                            this.ShowList();
                        } break;
                    case TriggerState.Show:
                        {
                            this.ShowList();
                        } break;
                    case TriggerState.HideAndConsume:
                        {
                            val = true;
                            this.HideList();
                        } break;
                    case TriggerState.Hide:
                        {
                            this.HideList();
                        } break;
                    case TriggerState.SelectAndConsume:
                        {
                            if (this.popup.Visible == true)
                            {
                                val = true;
                                this.SelectCurrentItem();

                            }
                        } break;
                    case TriggerState.Select:
                        {
                            if (this.popup.Visible == true)
                            {
                                this.SelectCurrentItem();
                            }
                        } break;
                    default:
                        break;
                }
            }

            return val;

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ReadOnly)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            //先判断是否已经enter按键被自动完成的enter触发器消化，如果是则直接触发enterkeydown事件
            if (keyData == Keys.Enter && this.popup.Visible == false)
            {
                OnEnterKeyDown(new EventArgs());
                TriggerDataInputCompleted();//触发输入完成事件
                return true;
            }
            if (!popup.Visible)
            {
                switch (keyData)
                {
                    case Keys.Up:
                    case Keys.Down:
                        {
                            this.ShowList();

                            return true;
                        }
                        //break;
                }
            }
            switch (keyData)
            {
                case Keys.Up:
                    {
                        this.Mode = EntryMode.List;
                        if (this.list.SelectedIndex > 0)
                        {
                            this.list.SelectedIndex--;
                        }
                        return true;
                    }
                    //break;
                case Keys.Down:
                    {
                        this.Mode = EntryMode.List;
                        if (this.list.SelectedIndex < this.list.Items.Count - 1)
                        {
                            this.list.SelectedIndex++;
                        }
                        return true;
                    }
                    //break;
                default:
                    {
                        return DefaultCmdKey(ref msg, keyData);
                    }
                    //break;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            
            this.list.SelectedIndex = -1;//清除掉所选项

            if (ReadOnly) return;

            if (this.TriggersEnabled)
            {
                //当text有值变为空的时候，显示全部
                if (this.Text.Length == 0)
                {
                    this.ShowList();
                    return;
                }
                //如果空格无效，则当前去掉空格后，还是空字符串，则返回不触发
                if ((!this.IsSpaceKeyCodeValid)
                    && this.Text.Trim() == string.Empty)
                {
                    return;
                }
                lastInputTriggerTime = DateTime.Now;
                timerTriggerInputChanged.Enabled = true;

            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            if (ReadOnly) return;

            if (!(this.Focused || this.popup.Focused || this.list.Focused))
            {
                this.HideList();
            }
        }
        #endregion

        #region 内部方法

        protected virtual void SelectCurrentItem()
        {
            //if (this.list.SelectedIndex == -1)
            //{
            //    return;
            //}

            this.Focus();
            if (this.list.SelectedIndex != -1)
            {
                this.Text = GetSelectText(list.Items[list.SelectedIndex]);

            }

            if (this.Text.Length > 0)
            {
                this.SelectionStart = this.Text.Length;
            }

            this.HideList();

            //触发输入完成事件
            TriggerDataInputCompleted();
        }

        private string GetSelectText(object selecteItem)
        {
            if (selecteItem == null)
                return string.Empty;

            SelectItemEventArgs e = new SelectItemEventArgs(this.list.GetItemText(selecteItem), selecteItem);

            OnFillingAutoCompleteTextEvent(e);
            return e.Text;

        }

        protected virtual void ShowList()
        {
            if (this.popup.Visible == false)
            {
                this.list.SelectedIndex = -1;

                this.UpdateList();

                Point p = this.PointToScreen(new Point(0, 0));
                p.X += this.PopupOffset.X;
                p.Y += this.Height + this.PopupOffset.Y;
                this.popup.Location = p;
                if (this.list.Items.Count > 0)
                {
                    //this.popup.Show();

                    ShowWindow(this.popup.Handle, SW_SHOWNOACTIVATE);//显示一个SW_SHOWNOACTIVATE的窗体

                    //TOPMOST，不获取焦点的方式
                    SetWindowPos(this.popup.Handle, HWND_TOPMOST, this.popup.Left, this.popup.Top, this.popup.Width, this.popup.Height, SWP_NOACTIVATE);


                    if (this.hook == null)
                    {
                        this.hook = new WinHook(this);

                        if (this.Parent != null)
                        {
                            this.hook.AssignHandle(this.Parent.Handle);
                        }
                        //this.hook.AssignHandle(this.FindForm().Handle);
                    }
                    this.Focus();
                }
            }
            else
            {
                this.UpdateList();
            }
        }

        protected virtual void HideList()
        {
            this.Mode = EntryMode.Text;
            if (this.hook != null)
                this.hook.ReleaseHandle();
            this.hook = null;
            this.popup.Hide();
        }

        protected virtual void UpdateList()
        {
            object selectedItem = this.list.SelectedItem;

            this.list.Items.Clear();

            //这里需要触发输入事件，让外面自己去根据输入填充搜索结果项
            OnInputChanged(new EventArgs());

            if (selectedItem != null &&
                this.list.Items.Contains(selectedItem))
            {
                EntryMode oldMode = this.Mode;
                this.Mode = EntryMode.List;
                this.list.SelectedItem = selectedItem;
                this.Mode = oldMode;
            }

            if (this.list.Items.Count == 0)
            {
                this.HideList();
            }
            else
            {
                int visItems = this.list.Items.Count;
                if (visItems > 8)
                    visItems = 8;

                this.popup.Height = (visItems * this.list.ItemHeight) + 2;

                this.popup.Width = this.Width;

                if (this.list.Items.Count > 0 &&
                    this.list.SelectedIndex == -1)
                {
                    EntryMode oldMode = this.Mode;
                    this.Mode = EntryMode.List;
                    //this.list.SelectedIndex = 0;
                    this.Mode = oldMode;
                }

            }
        }

        #endregion

        #region 事件处理方法
        void WSNAutoCompleteTextBox_LostFocus(object sender, EventArgs e)
        {
            if (!this.popup.Visible)
                TriggerDataInputCompleted();
        }

        private void TriggerDataInputCompleted()
        {
            if (this.Text != oldText)
            {
                OnDataInputCompleted(new DataInputCompletedEventArgs(true, oldText));
                oldText = this.Text;
            }
        }

        private void List_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Mode != EntryMode.List)
            {
                SelectCurrentItem();
            }
        }

        private void List_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < this.list.Items.Count; i++)
            {
                if (this.list.GetItemRectangle(i).Contains(e.X, e.Y))
                {
                    this.list.SelectedIndex = i;
                    this.SelectCurrentItem();
                    this.HideList();
                    return;
                }
            }
            this.HideList();
        }

        private void Popup_Deactivate(object sender, EventArgs e)
        {
            if (!(this.Focused || this.popup.Focused || this.list.Focused))
            {
                this.HideList();
            }
        }
        #endregion


    }
    /// <summary>
    /// 自动完成控件输入完成事件参数类
    /// </summary>
    public class DataInputCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// 数据是否改变 true-是 false-否
        /// </summary>
        public bool IsDataChanged { get; set; }
        /// <summary>
        /// 原来的文本
        /// </summary>
        public string OldText
        { get; set; }
        /// <summary>
        /// 新建自动完成控件输入完成事件参数类的一个实例
        /// </summary>
        /// <param name="isDataChanged">数据是否改变</param>
        /// <param name="oldText">原来的文本</param>
        public DataInputCompletedEventArgs(bool isDataChanged, string oldText)
        {
            IsDataChanged = isDataChanged;
            OldText = oldText;
        }
    }
    /// <summary>
    /// 自动完成下拉列表选择项填充事件参数类
    /// </summary>
    public class SelectItemEventArgs : EventArgs
    {
        /// <summary>
        /// 应该填充在自动完成输入框的文本.由事件注册者负责填充
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 下拉列表中的选择项
        /// </summary>
        public object SelectedItem { get; set; }

        public SelectItemEventArgs(string text, object item)
        {
            Text = text;
            SelectedItem = item;
        }
    }

}
