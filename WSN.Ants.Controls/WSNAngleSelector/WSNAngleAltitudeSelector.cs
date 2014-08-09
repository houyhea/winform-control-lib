using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WSN.Ants.Controls.WSNAngleAltitudeSelector
{
    /// <summary>
    /// WSN控件:
    ///     WSNAngleAltitudeSelector 
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNAngleAltitudeSelector
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNAngleSelectorExamples.cs" region="WSNAngleAltitudeSelectorExample"/>
    /// </example>
    public partial class WSNAngleAltitudeSelector : UserControl
    {
        private int angle;
        private int altitude = 90;

        private Rectangle drawRegion;
        private Point origin;



        private void AngleAltitudeSelector_Load(object sender, EventArgs e)
        {
            setDrawRegion();
        }

        private void AngleAltitudeSelector_SizeChanged(object sender, EventArgs e)
        {
            this.Height = this.Width;
            setDrawRegion();
        }

        private void setDrawRegion()
        {
            drawRegion = new Rectangle(0, 0, this.Width, this.Height);
            drawRegion.X += 2;
            drawRegion.Y += 2;
            drawRegion.Width -= 4;
            drawRegion.Height -= 4;

            int offset = 2;
            origin = new Point(drawRegion.Width / 2 + offset, drawRegion.Height / 2 + offset);

            this.Refresh();
        }

        /// <summary>
        /// 创建新控件<c>WSNAngleAltitudeSelector</c>
        /// </summary>
        public WSNAngleAltitudeSelector()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置角度
        /// </summary>
        public int Angle
        {
            get { return angle; }
            set
            {
                angle = value;

                if (!this.DesignMode && AngleChanged != null)
                    AngleChanged(); //Raise event

                this.Refresh();
            }
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置高度
        /// </summary>
        public int Altitude
        {
            get { return altitude; }
            set
            {
                altitude = value;

                if (!this.DesignMode && AltitudeChanged != null)
                    AltitudeChanged(); //Raise event

                this.Refresh();
            }
        }

        /// <summary>
        /// 委托：
        ///     为角度改变事件定义的委托
        /// </summary>
        public delegate void AngleChangedDelegate();
        /// <summary>
        /// 事件：
        ///     角度改变时触发的事件
        /// </summary>
        public event AngleChangedDelegate AngleChanged;

        /// <summary>
        /// 委托：
        ///     为高度改变事件定义的委托
        /// </summary>
        public delegate void AltitudeChangedDelegate();
        /// <summary>
        /// 事件：
        ///     高度改变时触发的事件
        /// </summary>
        public event AltitudeChangedDelegate AltitudeChanged;

        private PointF DegreesToXY(float degrees, float radius, Point origin)
        {
            PointF xy = new PointF();
            double radians = degrees * Math.PI / 180.0;

            xy.X = (float)Math.Cos(radians) * radius + origin.X;
            xy.Y = (float)Math.Sin(-radians) * radius + origin.Y;

            return xy;
        }

        private float XYToDegrees(Point xy, Point origin)
        {
            double angle = 0.0;

            if (xy.Y < origin.Y)
            {
                if (xy.X > origin.X)
                {
                    angle = (double)(xy.X - origin.X) / (double)(origin.Y - xy.Y);
                    angle = Math.Atan(angle);
                    angle = 90.0 - angle * 180.0 / Math.PI;
                }
                else if (xy.X < origin.X)
                {
                    angle = (double)(origin.X - xy.X) / (double)(origin.Y - xy.Y);
                    angle = Math.Atan(-angle);
                    angle = 90.0 - angle * 180.0 / Math.PI;
                }
            }
            else if (xy.Y > origin.Y)
            {
                if (xy.X > origin.X)
                {
                    angle = (double)(xy.X - origin.X) / (double)(xy.Y - origin.Y);
                    angle = Math.Atan(-angle);
                    angle = 270.0 - angle * 180.0 / Math.PI;
                }
                else if (xy.X < origin.X)
                {
                    angle = (double)(origin.X - xy.X) / (double)(xy.Y - origin.Y);
                    angle = Math.Atan(angle);
                    angle = 270.0 - angle * 180.0 / Math.PI;
                }
            }

            if (angle > 180) angle -= 360; //Optional. Keeps values between -180 and 180
            return (float)angle;
        }

        private float getDistance(Point point1, Point point2)
        {
            return (float)Math.Sqrt(Math.Pow((point1.X - point2.X), 2) + Math.Pow((point1.Y - point2.Y), 2));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen outline = new Pen(Color.FromArgb(86, 103, 141), 2.0f);
            SolidBrush fill = new SolidBrush(Color.FromArgb(90, 255, 255, 255));

            float radius = (origin.X * (90.0f - altitude) / 100.0f);

            PointF anglePoint = DegreesToXY(angle, radius, origin);
            Rectangle originSquare = new Rectangle(origin.X - 1, origin.Y - 1, 3, 3);
            Rectangle pointSquare = new Rectangle((int)anglePoint.X, (int)anglePoint.Y, 1, 1);

            //Draw
            g.SmoothingMode = SmoothingMode.AntiAlias; //Smooth edges

            g.DrawEllipse(outline, drawRegion);
            g.FillEllipse(fill, drawRegion);

            g.SmoothingMode = SmoothingMode.HighSpeed; //Make the edges sharp

            //Draw point
            g.FillRectangle(Brushes.Black, pointSquare);

            int leftX0 = pointSquare.X - 3;
            if (leftX0 < 0) leftX0 = 0;

            int leftX = pointSquare.X - 2;
            if (leftX < 0) leftX = 0;

            int rightX0 = pointSquare.X + 2;
            if (rightX0 > drawRegion.Right) rightX0 = drawRegion.Right;

            int rightX = pointSquare.X + 3;
            if (rightX > drawRegion.Right) rightX = drawRegion.Right;

            int topY0 = pointSquare.Y - 3;
            if (topY0 < 0) topY0 = 0;

            int topY = pointSquare.Y - 2;
            if (topY < 0) topY = 0;

            int bottomY0 = pointSquare.Y + 2;
            if (bottomY0 > drawRegion.Bottom) bottomY0 = drawRegion.Bottom;

            int bottomY = pointSquare.Y + 3;
            if (bottomY > drawRegion.Bottom) bottomY = drawRegion.Bottom;

            g.DrawLine(Pens.Black, leftX0, pointSquare.Y, leftX, pointSquare.Y);
            g.DrawLine(Pens.Black, rightX0, pointSquare.Y, rightX, pointSquare.Y);

            g.DrawLine(Pens.Black, pointSquare.X, topY0, pointSquare.X, topY);
            g.DrawLine(Pens.Black, pointSquare.X, bottomY0, pointSquare.X, bottomY);

            g.FillRectangle(Brushes.Black, originSquare);

            fill.Dispose();
            outline.Dispose();

            base.OnPaint(e);
        }

        private void AngleAltitudeSelector_MouseDown(object sender, MouseEventArgs e)
        {
            int thisAngle = findNearestAngle(new Point(e.X, e.Y));
            int thisAltitude = findAltitude(new Point(e.X, e.Y));

            if (thisAngle != -1)
                this.Angle = thisAngle;

            this.Altitude = thisAltitude;

            this.Refresh();
        }

        private void AngleAltitudeSelector_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                int thisAngle = findNearestAngle(new Point(e.X, e.Y));
                int thisAltitude = findAltitude(new Point(e.X, e.Y));

                if (thisAngle != -1)
                    this.Angle = thisAngle;

                this.Altitude = thisAltitude;

                this.Refresh();
            }
        }

        private int findNearestAngle(Point mouseXY)
        {
            int thisAngle = (int)XYToDegrees(mouseXY, origin);
            if (thisAngle != 0)
                return thisAngle;
            else
                return -1;
        }

        private int findAltitude(Point mouseXY)
        {
            float distance = getDistance(mouseXY, origin);
            int alt = 90 - (int)(90.0f * (distance / origin.X));
            if (alt < 0) alt = 0;

            return alt;
        }
    }
}
