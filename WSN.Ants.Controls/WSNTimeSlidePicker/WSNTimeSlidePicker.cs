using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace WSN.Ants.Controls.WSNTimeSlidePicker
{
    /// <summary>
    /// WSN控件：
    ///     WSNTimeSlidePicker
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNTimeSlidePicker
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNTimeSlidePickerExamples.cs" region="WSNTimeSlidePickerExample"/>
    /// </example>
    public partial class WSNTimeSlidePicker : TextBox 
    {
        private WSNPopupControlHost.WSNPopupControlHost host;
        private WSNTrackBar.WSNTrackBar bar;
        private TimeSlideHook hook;
        private Color backNormalColor;
        private Timer timer;
        private int blinkCount;
        private bool blink;

        private int blinkTotalCount;

        /// <summary>
        /// 控件属性：
        ///     获取时间（小时）
        /// </summary>
        public int Hours
        {
            get
            {
                int h= GetHours();
                if (h >= 24)
                    h = 0;
                if (h < 0)
                    h = 0;
                return h;
            }
        } 
        /// <summary>
        /// 控件属性：
        ///     获取时间（分钟）
        /// </summary>
        public int Minutes
        {
            get
            {
                int m= GetMinutes();
                if (m >= 60)
                    m = 0;
                if (m < 0)
                    m = 0;
                return m;
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取秒数
        /// </summary>
        public int Seconds
        {
            get
            {
                int s=GetSeconds();
                if (s >= 60)
                    s = 0;
                if (s < 0)
                    s = 0;
                return s;
            }
        }

        private int GetSeconds()
        {
            return bar.Value % (60 * 60) % 60;
        }

        private int GetMinutes()
        {
            return (bar.Value % (60 * 60)) / 60;
        }

        private int GetHours()
        {
            return bar.Value / (60 * 60);

        }
        /// <summary>
        /// bar是否在显示状态
        /// </summary>
        public bool IsSlidePopUp
        {
            get
            {
                return host.Visible;
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置Blink总数
        /// </summary>
        /// <remarks>默认为：6</remarks>
        [DefaultValue(6)]
        public int BlinkTotalCount
        {
            get { return blinkTotalCount; }
            set { blinkTotalCount = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置正常情况下的背景色
        /// </summary>
        /// <remarks>默认为："255,255,255"</remarks>
        [DefaultValue("255,255,255")]
        public Color BackNormalColor
        {
            get
            {
                return backNormalColor;
            }
            set
            {
                backNormalColor = value;
                this.BackColor = value;
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置出错情况下的背景色
        /// </summary>
        /// <remarks>默认为："255,211,115"</remarks>
        private Color backErrorColor;
        [DefaultValue("255,211,115")]
        public Color BackErrorColor
        {
            get { return backErrorColor; }
            set { backErrorColor = value; }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置下拉边框颜色
        /// </summary>
        /// <remarks>默认为："179, 179, 179"</remarks>
        [DefaultValue("179, 179, 179")]
        public Color DropDownBorderColor
        {
            get
            {
                if (host == null)
                    return Color.Empty;
                return host.BorderColor;

            }
            set
            {
                if (host != null)
                {
                    host.BorderColor = value;
                }
            }
        }

        
        /// <summary>
        /// 构造函数：
        ///     创建，<c>WSNTimeSlidePicker</c>
        /// </summary>
        public WSNTimeSlidePicker()
        {
            bar = new WSNTrackBar.WSNTrackBar();
            bar.Minimum = 0;
            bar.Maximum = 24 * 60 * 60-1;
            bar.SmallChange = 60;
            BackErrorColor = Color.FromArgb(255, 211, 115);
            backNormalColor = Color.White;

            timer = new Timer();
            timer.Interval = 200;
            BlinkTotalCount = 6;
            timer.Tick += new EventHandler(timer_Tick);
            blinkCount = 0;

            //this.bar.Value = 0;

            bar.Orientation = Orientation.Vertical;
            host = new WSNPopupControlHost.WSNPopupControlHost(bar);
            //host.ChangeRegion = true;//设置显示区域。

            //host.Opacity = 0.8F;//设置透明度。
            host.CanResize = false;//设置可以改变大小。
            host.OpenFocused = false;//把焦点设置到弹出窗体上。
            host.AutoClose = false;

            this.Click += new EventHandler(WSNTimeSlidePicker_Click);
            bar.ValueChanged += new EventHandler(bar_ValueChanged);
            this.Validating += new System.ComponentModel.CancelEventHandler(WSNTimeSlidePicker_Validating);
            this.TextChanged += new EventHandler(WSNTimeSlidePicker_TextChanged);
            this.Text = "23:59:59";

        }

        void timer_Tick(object sender, EventArgs e)
        {
            blinkCount++;
            if (blinkCount > BlinkTotalCount)
            {
                StopBlink();

            }
            else
            {
                blink = !blink;
                this.BackColor = blink ? BackNormalColor : BackErrorColor;
            }
        }

        void WSNTimeSlidePicker_TextChanged(object sender, EventArgs e)
        {
            bool b = IsTime(this.Text.Trim());
            if (!b)
            {
                StartBlink();
            }
            else
            {
                this.bar.Value = Time2TrackValue(this.Text);

            }
        }
        private void StopBlink()
        {
            timer.Enabled = false;
            this.BackColor = BackNormalColor;
        }
        private void StartBlink()
        {
            timer.Enabled = true;
            blinkCount = 0;
            blink = true;
        }

        void WSNTimeSlidePicker_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool b = IsTime(this.Text.Trim());
            if (!b)
            {
                StartBlink();
                e.Cancel = true;
            }

        }

        void bar_ValueChanged(object sender, EventArgs e)
        {
            int v = bar.Value;
            //if (v == bar.Maximum)
            //    v = 0;
            this.Text = TrackValue2Time(v);
        }

        private string TrackValue2Time(int value)
        {
            int h = value / (60 * 60);
            int m = (value % (60 * 60)) / 60;
            int s = value % (60 * 60) % 60;

            return string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);

        }
        private int Time2TrackValue(string time)
        {

            try
            {
                string[] strs = time.Split(':');
                int h = int.Parse(strs[0]);
                int m = int.Parse(strs[1]);
                int s = int.Parse(strs[2]);
                return h * 60 * 60 + m * 60 + s;
            }
            catch
            {
                return 0;
            }

        }

        void WSNTimeSlidePicker_Click(object sender, EventArgs e)
        {
            ShowDropDown();
        }

        /// <summary>
        /// 显示下拉选项
        /// </summary>
        public void ShowDropDown()
        {
            Point thisP = this.Location;
            host.Show(this, new Point(this.Width, -bar.Height / 4));
            if (this.hook == null)
            {
                this.hook = new TimeSlideHook(this);
                if (this.Parent != null)
                {
                    this.hook.AssignHandle(this.Parent.Handle);
                }
            }
        }

        



        public void HideDropDown()
        {

            if (this.hook != null)
                this.hook.ReleaseHandle();
            this.hook = null;
            host.Close();
        }
        /// <summary>  
        /// 是否为时间型字符串  
        /// </summary>  
        /// <param name="source">时间字符串(15:00:00)</param>  
        /// <returns></returns>  
        public static bool IsTime(string StrSource)
        {
            return Regex.IsMatch(
                    StrSource,
                    @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }
    }
}
