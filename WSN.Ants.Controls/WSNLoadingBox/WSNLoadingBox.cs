using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Threading;



namespace WSN.Ants.Controls.WSNLoadingBox
{
    /// <summary>
    /// WSN控件：
    ///     WSNLoadingBox
    /// </summary>
    /// <remarks>正在加载中提示框</remarks>
    /// <example>
    /// 以下示例展示如何使用WSNLoadingBox
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNLoadingBoxExamples.cs" region="WSNLoadingBoxExample"/>
    /// </example>
    public class WSNLoadingBox : Control
    {
        #region 字段
        private const double NumberOfDegreesInCircle = 360;
        private const double NumberOfDegreesInHalfCircle = NumberOfDegreesInCircle / 2;


        //private Timer m_Timer;
        private bool m_IsTimerActive;
        private int m_ProgressValue;

        private Color m_Color;
        private Color[] m_Colors;
        private double[] m_Angles;

        private int m_NumberOfSpoke;
        private int m_SpokeThickness;
        private int m_OuterCircleRadius;
        private int m_InnerCircleRadius;
        private PointF m_CenterPoint;

        Thread myThread;
        #endregion

        #region 属性
        /// <summary>
        /// 控件属性：
        ///     获取或设置辐条颜色
        /// </summary>
        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;

                GenerateColorsPallet();
                Invalidate();
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置外边框圆角半径
        /// </summary>
        public int OuterCircleRadius
        {
            get
            {
                if (m_OuterCircleRadius == 0)
                    m_OuterCircleRadius = 10;

                return m_OuterCircleRadius;
            }
            set
            {
                m_OuterCircleRadius = value;
                Invalidate();
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置内边框圆角半径
        /// </summary>
        public int InnerCircleRadius
        {
            get
            {
                if (m_InnerCircleRadius == 0)
                    m_InnerCircleRadius = 8;

                return m_InnerCircleRadius;
            }
            set
            {
                m_InnerCircleRadius = value;
                Invalidate();
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置辐条数目
        /// </summary>
        /// <remarks>辐条数目必须大于0</remarks>
        public int NumberSpoke
        {
            get
            {
                if (m_NumberOfSpoke == 0)
                    m_NumberOfSpoke = 10;

                return m_NumberOfSpoke;
            }
            set
            {
                if (m_NumberOfSpoke != value && m_NumberOfSpoke > 0)
                {
                    m_NumberOfSpoke = value;
                    GenerateColorsPallet();
                    GetSpokesAngles();

                    Invalidate();
                }
            }
        }
        ///// <summary>
        ///// 控件属性：
        /////     获取或设置是否活动状态
        ///// </summary>
        //public bool Active
        //{
        //    get
        //    {
        //        return m_IsTimerActive;
        //    }
        //    set
        //    {
        //        m_IsTimerActive = value;
        //    }
        //}
        /// <summary>
        /// 控件属性：
        ///     获取或设置辐条宽度
        /// </summary>
        public int SpokeThickness
        {
            get
            {
                if (m_SpokeThickness <= 0)
                    m_SpokeThickness = 4;

                return m_SpokeThickness;
            }
            set
            {
                m_SpokeThickness = value;
                Invalidate();
            }
        }
        int speed = 200;
        /// <summary>
        /// 控件属性：
        ///     获取或设置旋转速度
        /// </summary>
        public int RotationSpeed
        {
            get
            {
                return speed;
            }
            set
            {
                if (value > 0)
                    speed = value;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 创建<c>WSNLoadingBox</c>
        /// </summary>
        public WSNLoadingBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            m_Color = Color.Red;

            GenerateColorsPallet();
            GetSpokesAngles();
            GetControlCenterPoint();

            //CreateThread();
            //m_Timer = new Timer();
            //m_Timer.Tick += new EventHandler(m_Timer_Tick);
            //ActiveTimer();
            this.Resize += new EventHandler(WSNLoadingBox_Resize);
        }
        #endregion


        protected override void OnPaint(PaintEventArgs e)
        {
            DrawAnimation(e.Graphics);
            //base.OnPaint(e);
        }


        #region 事件处理方法
        void WSNLoadingBox_Resize(object sender, EventArgs e)
        {
            GetControlCenterPoint();
        }

        #endregion

        /// <summary>
        ///  调用开始旋转
        /// </summary>
        public void Start()
        {
            m_IsTimerActive = true;
            if (myThread == null)
            {
                CreateThread();
            }
            myThread.Start();
        }

        public void Stop()
        {
            try
            {
                m_IsTimerActive = false;
                if (myThread != null)
                {
                    myThread.Abort();
                    myThread = null;
                }
            }
            catch { }
        }
        #region 私有方法

        private void CreateThread()
        {
            GenerateColorsPallet();
            m_ProgressValue = 0;
            myThread = new Thread(new ThreadStart(Loop));
            myThread.IsBackground = true;
            myThread.SetApartmentState(ApartmentState.STA);
            //myThread.Start();
        }
        private void Loop()
        {
            while (m_IsTimerActive)
            {
                try
                {
                    m_ProgressValue = ++m_ProgressValue % m_NumberOfSpoke;
                    ReDraw();
                    Thread.Sleep(speed);
                }
                catch
                {
                    break;
                }
            }

        }
        delegate void ReDrawCallback();
        private void ReDraw()
        {
            if (InvokeRequired)
            {
                Invoke(new ReDrawCallback(ReDraw));
            }
            else
            {
                if (!m_IsTimerActive)
                    return;
                Invalidate();
            }
        }

        private void DrawAnimation(Graphics g)
        {
            if (m_NumberOfSpoke > 0)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                int intPosition = m_ProgressValue;
                for (int intCounter = 0; intCounter < m_NumberOfSpoke; intCounter++)
                {
                    intPosition = intPosition % m_NumberOfSpoke;
                    DrawLine(g,
                             GetCoordinate(m_CenterPoint, m_InnerCircleRadius, m_Angles[intPosition]),
                             GetCoordinate(m_CenterPoint, m_OuterCircleRadius, m_Angles[intPosition]),
                             m_Colors[intCounter], m_SpokeThickness);
                    intPosition++;
                }
            }

        }

        private void DrawLine(Graphics _objGraphics, PointF _objPointOne, PointF _objPointTwo,
                                  Color _objColor, int _intLineThickness)
        {
            using (Pen objPen = new Pen(new SolidBrush(_objColor), _intLineThickness))
            {
                objPen.StartCap = LineCap.Round;
                objPen.EndCap = LineCap.Round;
                _objGraphics.DrawLine(objPen, _objPointOne, _objPointTwo);
            }
        }

        /// <summary>
        /// Gets the coordinate.
        /// </summary>
        /// <param name="_objCircleCenter">The Circle center.</param>
        /// <param name="_intRadius">The radius.</param>
        /// <param name="_dblAngle">The angle.</param>
        /// <returns></returns>
        private PointF GetCoordinate(PointF _objCircleCenter, int _intRadius, double _dblAngle)
        {
            double dblAngle = Math.PI * _dblAngle / NumberOfDegreesInHalfCircle;

            return new PointF(_objCircleCenter.X + _intRadius * (float)Math.Cos(dblAngle),
                              _objCircleCenter.Y + _intRadius * (float)Math.Sin(dblAngle));
        }



        private void GenerateColorsPallet()
        {
            m_Colors = GenerateColorsPallet(m_Color, m_IsTimerActive, m_NumberOfSpoke);
        }

        private Color[] GenerateColorsPallet(Color _objColor, bool _blnShadeColor, int _intNbSpoke)
        {
            Color[] objColors = new Color[NumberSpoke];

            // Value is used to simulate a gradient feel... For each spoke, the 
            // color will be darken by value in intIncrement.
            byte bytIncrement = (byte)(byte.MaxValue / NumberSpoke);

            //Reset variable in case of multiple passes
            byte PERCENTAGE_OF_DARKEN = 0;

            for (int intCursor = 0; intCursor < NumberSpoke; intCursor++)
            {
                if (_blnShadeColor)
                {
                    if (intCursor == 0 || intCursor < NumberSpoke - _intNbSpoke)
                        objColors[intCursor] = _objColor;
                    else
                    {
                        // Increment alpha channel color
                        PERCENTAGE_OF_DARKEN += bytIncrement;

                        // Ensure that we don't exceed the maximum alpha
                        // channel value (255)
                        if (PERCENTAGE_OF_DARKEN > byte.MaxValue)
                            PERCENTAGE_OF_DARKEN = byte.MaxValue;

                        // Determine the spoke forecolor
                        objColors[intCursor] = Darken(_objColor, PERCENTAGE_OF_DARKEN);
                        //objColors[intCursor] = Darken(_objColor, 10);
                    }
                }
                else
                    objColors[intCursor] = _objColor;
            }

            return objColors;
        }

        private Color Darken(Color _objColor, int _intPercent)
        {
            int intRed = _objColor.R;
            int intGreen = _objColor.G;
            int intBlue = _objColor.B;
            return Color.FromArgb(_intPercent, Math.Min(intRed, byte.MaxValue), Math.Min(intGreen, byte.MaxValue), Math.Min(intBlue, byte.MaxValue));
        }

        private void GetSpokesAngles()
        {
            m_Angles = GetSpokesAngles(NumberSpoke);
        }

        private double[] GetSpokesAngles(int _intNumberSpoke)
        {
            double[] Angles = new double[_intNumberSpoke];
            double dblAngle = (double)NumberOfDegreesInCircle / _intNumberSpoke;

            for (int shtCounter = 0; shtCounter < _intNumberSpoke; shtCounter++)
                Angles[shtCounter] = (shtCounter == 0 ? dblAngle : Angles[shtCounter - 1] + dblAngle);

            return Angles;
        }
        private void GetControlCenterPoint()
        {
            m_CenterPoint = GetControlCenterPoint(this);
        }

        private PointF GetControlCenterPoint(Control _objControl)
        {
            return new PointF(_objControl.Width / 2, _objControl.Height / 2 - 1);
        }


        #endregion

    }
}
