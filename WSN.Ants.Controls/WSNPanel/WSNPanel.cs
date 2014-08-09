using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;

namespace WSN.Ants.Controls.WSNPanel
{
    /// <summary>
    /// WSN控件：
    ///     WSNPanel面板
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNPanel
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNPanelExamples.cs" region="WSNPanelExample"/>
    /// </example>
    public class WSNPanel : Panel
    {
        #region 字段
        private FaceStyle controlSchema=new FaceStyle();
        private RoundStyle roundStyle = RoundStyle.All;
        private int radius = 8;

        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置圆角半径
        /// </summary>
        /// <remarks>默认为：8</remarks>
        [DefaultValue(8)]
        public int Radius
        {
            get { return radius; }
            set
            {
                if (radius != value)
                {
                    radius = value < 4 ? 4 : value;
                    SetReion();
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置圆角样式
        /// </summary>
        /// <remarks>默认为：四个角都是圆角</remarks>
        [DefaultValue(typeof(RoundStyle), "1")]
        public RoundStyle RoundStyle
        {
            get { return roundStyle; }
            set
            {
                if (roundStyle != value)
                {
                    roundStyle = value;
                    SetReion();
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置控件外观样式 <c>FaceStyle</c>
        /// </summary>
        public FaceStyle ControlSchema
        {
            get { return controlSchema; }
            set { controlSchema = value; }
        }
        #endregion

        #region  构造函数
        /// <summary>
        /// 创建<c>WSNPanel</c>面板
        /// </summary>
        public WSNPanel()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); 
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            InitColor();
        }
        #endregion

        #region 私有方法

        private void InitColor()
        {
            controlSchema.BackNormalStyle.Color1 = Color.FromArgb(255, 255, 255);
            controlSchema.BackNormalStyle.Color2 = Color.FromArgb(215, 215, 215);
            controlSchema.BackNormalStyle.Mode = LinearGradientMode.Vertical;

            controlSchema.BorderNormalStyle.Color1 = Color.FromArgb(179, 179, 179);
            controlSchema.BorderNormalStyle.Color2 = Color.Empty;


        }
        private void SetReion()
        {

            if (base.Region != null)
            {
                base.Region.Dispose();
            }
            using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, false))
            {
                base.Region = new Region(path);
            }

        }

        #endregion

        #region 重载方法
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SetReion();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetReion();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

           
            using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(ClientRectangle, Radius, RoundStyle, false))
            {
                if (controlSchema.BackNormalStyle.Color2 != Color.Empty
                    && controlSchema.BackNormalStyle.Color1 != controlSchema.BackNormalStyle.Color2)
                {
                    if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
                    {
                        using (LinearGradientBrush br = new LinearGradientBrush(ClientRectangle, controlSchema.BackNormalStyle.Color1, controlSchema.BackNormalStyle.Color2, controlSchema.BackNormalStyle.Mode))
                        {
                            g.FillPath(br, path);
                        }
                    }
                }
                else
                {
                    using (SolidBrush br = new SolidBrush(controlSchema.BackNormalStyle.Color1))
                    {
                        g.FillPath(br, path);
                    }
 
                }
                
            }


            #region 画border
            Rectangle r = ClientRectangle;
            //r.Width -= 1;
            //r.Height -= 1;
            using (GraphicsPath path =
            GraphicsPathHelper.CreatePath(r, Radius, RoundStyle, true))
            {
                using (Pen p = new Pen(controlSchema.BorderNormalStyle.Color1))
                {

                    g.DrawPath(p, path);
                }
            }

            if (controlSchema.BorderNormalStyle.Color2 != Color.Empty
                && controlSchema.BorderNormalStyle.Color2 != controlSchema.BorderNormalStyle.Color1)
            {
                r.Inflate(-1, -1);
                using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                r, radius, roundStyle, true))
                {
                    using (Pen pen = new Pen(controlSchema.BorderNormalStyle.Color2))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
            #endregion


            #region 画背景图片
            if(BackgroundImage!=null)
            {
                switch (BackgroundImageLayout)
                {
                    case ImageLayout.None:
                        g.DrawImageUnscaled(BackgroundImage,
                            ClientRectangle.X,
                            ClientRectangle.Y,
                            BackgroundImage.Width,
                            BackgroundImage.Height);
                        break;
                    case ImageLayout.Tile:
                        using (TextureBrush Txbrus = new TextureBrush(BackgroundImage))
                        {
                            Txbrus.WrapMode = WrapMode.Tile;

                            g.FillRectangle(Txbrus, new Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1));

                        }
                        break;
                    case ImageLayout.Center:

                        int xx = (ClientRectangle.Width - BackgroundImage.Width) / 2;
                        int yy = (ClientRectangle.Height - BackgroundImage.Height) / 2;
                        g.DrawImage(BackgroundImage, new Rectangle(xx, yy, BackgroundImage.Width, BackgroundImage.Height), new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height), GraphicsUnit.Pixel);



                        break;
                    case ImageLayout.Stretch:

                        g.DrawImage(BackgroundImage, new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height), new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height), GraphicsUnit.Pixel);


                        break;

                    case ImageLayout.Zoom:
                        {
                            double tm = 0.0;
                            int W = BackgroundImage.Width;
                            int H = BackgroundImage.Height;
                            if (W > ClientRectangle.Width)
                            {
                                tm = ClientRectangle.Width / BackgroundImage.Width;
                                W = (int)(W * tm);
                                H = (int)(H * tm);
                            }
                            if (H > ClientRectangle.Height)
                            {
                                tm = ClientRectangle.Height / H;
                                W = (int)(W * tm);
                                H = (int)(H * tm);
                            }
                            using (Bitmap tmpBP = new Bitmap(W, H))
                            {
                                using (Graphics G2 = Graphics.FromImage(tmpBP))
                                {
                                    G2.DrawImage(BackgroundImage, new Rectangle(0, 0, W, H), new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height), GraphicsUnit.Pixel);

                                    int xxx = (ClientRectangle.Width - W) / 2;
                                    int yyy = (ClientRectangle.Height - H) / 2;
                                    g.DrawImage(tmpBP, new Rectangle(xxx, yyy, W, H), new Rectangle(0, 0, W, H), GraphicsUnit.Pixel);

                                }
                            }
                        }
                        break;

                }

            }

            #endregion


        }
        #endregion

    }
}
