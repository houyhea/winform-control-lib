using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WSN.Ants.Controls.WSNToolTips
{
    /// <summary>
    /// WSN控件：
    ///     WSNToolTipManager悬浮提示框管理
    /// </summary>
    public class WSNToolTipManager : IDisposable
    {
        #region 字段

        private WSNToolTip wsnToolTip;
        private IntPtr pOwner;
        private static WSNToolTipManager instance;

        #endregion

        #region 属性
        /// <summary>
        /// 单例模式
        /// </summary>
        public static WSNToolTipManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WSNToolTipManager();
                }
                return instance;
            }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        ///     创建<c>WSNToolTipManager</c>
        /// </summary>
        public WSNToolTipManager()
        {
            wsnToolTip = new WSNToolTip();
            pOwner = IntPtr.Zero;
        }

        #endregion

        #region 公共函数

        /// <summary>
        /// 不带图标悬浮提示
        /// </summary>
        /// <param name="Owner">关联控件</param>
        /// <param name="text">提示文本</param>
        /// <param name="title">提示标题</param>
        /// <param name="IsBallon">是否气泡</param>
        /// <param name="position">显示位置</param>
        /// <param name="width">提示宽度</param>
        public void Show(Control Owner, string text, string title, bool IsBallon, TipPosition position, int width)
        {
            try
            {
                if (!wsnToolTip.IsVisible())
                {
                    Reset();
                    pOwner = Owner.Handle;
                    
                    wsnToolTip.BallonStyle = IsBallon;
                    wsnToolTip.UseVisualStyle = true;
                    wsnToolTip.MultiLine = true;
                    wsnToolTip.MaximumWidth = width;
                    wsnToolTip.Opacity = 0.85F;
                    wsnToolTip.CloseButton = false;
                    wsnToolTip.ShowAlways = true;
                    wsnToolTip.AutoPopDelay = 5000;
                    wsnToolTip.Position = position;

                    if (IsBallon)
                    {
                        wsnToolTip.CustomStyle = TipStyle.Gradient;
                        wsnToolTip.GradientType = GradientStyle.VerticalTube;
                        wsnToolTip.GradientStartColor = Color.White;
                        wsnToolTip.GradientEndColor = Color.FromArgb(0xC0D8DD);
                    }

                    wsnToolTip.SetToolTip(Owner, text);

                    if (title != null)
                    {
                        wsnToolTip.SetTipTitle(pOwner, title);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 带默认图标悬浮提示
        /// </summary>
        /// <param name="Owner">关联控件</param>
        /// <param name="icon">OsIcon默认图标</param>
        /// <param name="text">提示文本</param>
        /// <param name="title">提示标题</param>
        /// <param name="IsBallon">是否气泡</param>
        /// <param name="position">显示位置</param>
        /// <param name="width">提示宽度</param>
        public void Show(Control Owner, OsIcon icon, string text, string title, bool IsBallon, TipPosition position, int width)
        {
            try
            {
                if (!wsnToolTip.IsVisible())
                {
                    Reset();
                    pOwner = Owner.Handle;
                    
                    wsnToolTip.BallonStyle = IsBallon;
                    wsnToolTip.UseVisualStyle = false;
                    wsnToolTip.CloseButton = false;
                    wsnToolTip.MultiLine = true;
                    wsnToolTip.MaximumWidth = width;

                    wsnToolTip.SetToolTip(Owner, text);

                    if (title != null)
                    {
                        wsnToolTip.SetTipTitle(title, icon);
                    }
                }

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 隐藏提示框
        /// </summary>
        /// <param name="Owner">提示框的父控件</param>
        public void HideToolTip(Control Owner)
        {
            try
            {
                if (wsnToolTip.IsVisible())
                {
                    wsnToolTip.RemoveTip(wsnToolTip.Handle);
                    wsnToolTip.Hide();
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                wsnToolTip.Dispose();
                wsnToolTip = null;
                instance = null;
            }
            catch
            {
                return;
            }
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 重设参数
        /// </summary>
        private void Reset()
        {
            try
            {
                if (pOwner != IntPtr.Zero)
                {
                    wsnToolTip.RemoveTip(pOwner);
                }
                wsnToolTip.BorderColor = Color.FromKnownColor(KnownColor.ButtonShadow);
                wsnToolTip.CenterTip = false;
                wsnToolTip.Clickable = false;
                //wsnToolTip.CloseButton = false;
                //wsnToolTip.CustomStyle = TipStyle.Default;
                //wsnToolTip.ForeColor = Color.Black;
                //wsnToolTip.GradientEndColor = Color.LightGray;
                //wsnToolTip.GradientStartColor = Color.White;
                //wsnToolTip.GradientType = GradientStyle.VerticalTube;
                wsnToolTip.MaximumWidth = 150;
                wsnToolTip.MultiLine = false;
                wsnToolTip.Opacity = 0.6f;
                wsnToolTip.RightToLeft = false;
                wsnToolTip.ShowAlways = false;
                wsnToolTip.TitleColor = Color.Black;
                wsnToolTip.UseAnimation = false;
                wsnToolTip.UseFading = true;
                wsnToolTip.UseVisualStyle = false;
                wsnToolTip.Size = new Size(0, 0);
                wsnToolTip.FadePulseColor = Color.LimeGreen;

            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}
