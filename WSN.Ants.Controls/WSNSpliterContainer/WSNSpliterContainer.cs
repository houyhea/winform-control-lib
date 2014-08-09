using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WSN.Ants.Controls.WSNSpliterContainer
{
    /// <summary>
    /// WSN控件：
    ///     WSNSpliterContainer
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNSpliterContainer
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNSpliterContainerExamples.cs" region="WSNSpliterContainerExample"/>
    /// </example>
    public class WSNSpliterContainer : SplitContainer
    {
        private CollapsePanel _collapsePanel = CollapsePanel.Panel1;
        private SpliterPanelState _spliterPanelState = SpliterPanelState.Expanded;
        private ControlState _mouseState;
        private int _lastDistance;
        private int _minSize;
        private HistTest _histTest;

        public event EventHandler CollapseClick;


        public WSNSpliterContainer()
        {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            _lastDistance = base.SplitterDistance;
        }
        /// <summary>
        /// 控件属性：
        ///     获取或设置折叠的面板
        /// </summary>
        /// <remarks>默认为：panel1折叠</remarks>
        [DefaultValue(typeof(CollapsePanel), "1")]
        public CollapsePanel CollapsePanel
        {
            get { return _collapsePanel; }
            set
            {
                if (_collapsePanel != value)
                {
                    Expand();
                    _collapsePanel = value;
                }
            }
        }

        protected virtual int DefaultCollapseWidth
        {
            get { return 80; }
        }

        protected virtual int DefaultArrowWidth
        {
            get { return 16; }
        }


        protected Rectangle CollapseRect
        {
            get
            {
                if (_collapsePanel == CollapsePanel.None)
                {
                    return Rectangle.Empty;
                }

                Rectangle rect = base.SplitterRectangle;
                if (base.Orientation == Orientation.Horizontal)
                {
                    rect.X = (base.Width - DefaultCollapseWidth) / 2;
                    rect.Width = DefaultCollapseWidth;
                }
                else
                {
                    rect.Y = (base.Height - DefaultCollapseWidth) / 2;
                    rect.Height = DefaultCollapseWidth;
                }

                return rect;
            }
        }

        /// <summary>
        /// 控件属性：
        ///     获取或设置面板状态
        /// </summary>
        public SpliterPanelState SpliterPanelState
        {
            get { return _spliterPanelState; }
            set
            {
                if (_spliterPanelState != value)
                {
                    switch (value)
                    {
                        case SpliterPanelState.Expanded:
                            Expand();
                            break;
                        case SpliterPanelState.Collapsed:
                            Collapse();
                            break;

                    }
                    _spliterPanelState = value;
                }
            }
        }

        internal ControlState MouseState
        {
            get { return _mouseState; }
            set
            {
                if (_mouseState != value)
                {
                    _mouseState = value;
                    base.Invalidate(CollapseRect);
                }
            }
        }


        #region 公共方法
        /// <summary>
        /// 折叠
        /// </summary>
        public void Collapse()
        {
            if (_collapsePanel != CollapsePanel.None &&
                _spliterPanelState == SpliterPanelState.Expanded)
            {
                _lastDistance = base.SplitterDistance;
                if (_collapsePanel == CollapsePanel.Panel1)
                {
                    _minSize = base.Panel1MinSize;
                    base.Panel1MinSize = 0;
                    base.SplitterDistance = 0;
                }
                else
                {
                    int width = base.Orientation == Orientation.Horizontal ?
                        base.Height : base.Width;
                    _minSize = base.Panel2MinSize;
                    base.Panel2MinSize = 0;
                    base.SplitterDistance = width - base.SplitterWidth - base.Padding.Vertical;
                }
                base.Invalidate(base.SplitterRectangle);
            }
        }
        /// <summary>
        /// 打开折叠的面板
        /// </summary>
        public void Expand()
        {
            if (_collapsePanel != CollapsePanel.None &&
               _spliterPanelState == SpliterPanelState.Collapsed)
            {
                if (_collapsePanel == CollapsePanel.Panel1)
                {
                    base.Panel1MinSize = _minSize;
                }
                else
                {
                    base.Panel2MinSize = _minSize;
                }
                base.SplitterDistance = _lastDistance;
                base.Invalidate(base.SplitterRectangle);
            }
        }

        #endregion

        #region 重载方法

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            base.Invalidate(base.SplitterRectangle);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (base.Panel1Collapsed || base.Panel2Collapsed)
            {
                return;
            }

            Graphics g = e.Graphics;
            Rectangle rect = base.SplitterRectangle;
            bool bHorizontal = base.Orientation == Orientation.Horizontal;

            LinearGradientMode gradientMode = bHorizontal ?
                LinearGradientMode.Vertical : LinearGradientMode.Horizontal;

            if (SpliterPanelState == SpliterPanelState.Expanded)
            {
                if (rect.Width > 0 && rect.Height > 0)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        rect, Color.FromArgb(213, 213, 213),
                        Color.FromArgb(213, 213, 213), gradientMode))
                    {
                        Blend blend = new Blend();
                        blend.Positions = new float[] { 0f, .5f, 1f };
                        blend.Factors = new float[] { .5F, 1F, .5F };

                        brush.Blend = blend;
                        g.FillRectangle(brush, rect);
                    }
                }
            }
            else
            {
                if (rect.Width > 0 && rect.Height > 0)
                {

                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        rect, Color.FromArgb(255, 187, 50),
                        Color.FromArgb(255, 187, 50), gradientMode))
                    {
                        Blend blend = new Blend();
                        blend.Positions = new float[] { 0f, .5f, 1f };
                        blend.Factors = new float[] { .5F, 1F, .5F };

                        brush.Blend = blend;
                        g.FillRectangle(brush, rect);
                    }
                }
            }
            if (_collapsePanel == CollapsePanel.None)
            {
                return;
            }

            Rectangle arrowRect;
            Rectangle topLeftRect;
            Rectangle bottomRightRect;

            CalculateRect(
                CollapseRect,
                out arrowRect,
                out topLeftRect,
                out bottomRightRect);

            ArrowDirection direction = ArrowDirection.Left;

            switch (_collapsePanel)
            {
                case CollapsePanel.Panel1:
                    if (bHorizontal)
                    {
                        direction =
                            _spliterPanelState == SpliterPanelState.Collapsed ?
                            ArrowDirection.Down : ArrowDirection.Up;
                    }
                    else
                    {
                        direction =
                            _spliterPanelState == SpliterPanelState.Collapsed ?
                            ArrowDirection.Right : ArrowDirection.Left;
                    }
                    break;
                case CollapsePanel.Panel2:
                    if (bHorizontal)
                    {
                        direction =
                            _spliterPanelState == SpliterPanelState.Collapsed ?
                            ArrowDirection.Up : ArrowDirection.Down;
                    }
                    else
                    {
                        direction =
                            _spliterPanelState == SpliterPanelState.Collapsed ?
                            ArrowDirection.Left : ArrowDirection.Right;
                    }
                    break;
            }

            Color foreColor = _mouseState == ControlState.Hover ?
                Color.FromArgb(21, 66, 139) : Color.FromArgb(80, 136, 228);
            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
            {
                RenderHelper.RenderGrid(g, topLeftRect, new Size(3, 3), foreColor);
                RenderHelper.RenderGrid(g, bottomRightRect, new Size(3, 3), foreColor);

                using (Brush brush = new SolidBrush(foreColor))
                {
                    RenderHelper.RenderArrowInternal(
                        g,
                        arrowRect,
                        direction,
                        brush);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //如果鼠标的左键没有按下，重置HistTest
            if (e.Button != MouseButtons.Left)
            {
                _histTest = HistTest.None;
            }

            Rectangle collapseRect = CollapseRect;
            Point mousePoint = e.Location;

            //鼠标在Button矩形里，并且不是在拖动
            if (collapseRect.Contains(mousePoint) &&
                _histTest != HistTest.Spliter)
            {
                base.Capture = false;
                SetCursor(Cursors.Hand);
                MouseState = ControlState.Hover;
                return;
            }//鼠标在分隔栏矩形里
            else if (base.SplitterRectangle.Contains(mousePoint))
            {
                MouseState = ControlState.Normal;

                //如果已经在按钮按下了鼠标或者已经收缩，就不允许拖动了
                if (_histTest == HistTest.Button ||
                    (_collapsePanel != CollapsePanel.None &&
                    _spliterPanelState == SpliterPanelState.Collapsed))
                {
                    base.Capture = false;
                    base.Cursor = Cursors.Default;
                    return;
                }

                //鼠标没有按下，设置Split光标
                if (_histTest == HistTest.None &&
                    !base.IsSplitterFixed)
                {
                    if (base.Orientation == Orientation.Horizontal)
                    {
                        SetCursor(Cursors.HSplit);
                    }
                    else
                    {
                        SetCursor(Cursors.VSplit);
                    }
                    return;
                }
            }

            MouseState = ControlState.Normal;

            //正在拖动分隔栏
            if (_histTest == HistTest.Spliter &&
                !base.IsSplitterFixed)
            {
                if (base.Orientation == Orientation.Horizontal)
                {
                    SetCursor(Cursors.HSplit);
                }
                else
                {
                    SetCursor(Cursors.VSplit);
                }
                base.OnMouseMove(e);
                return;
            }

            base.Cursor = Cursors.Default;
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.Cursor = Cursors.Default;
            MouseState = ControlState.Normal;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Rectangle collapseRect = CollapseRect;
            Point mousePoint = e.Location;

            if (collapseRect.Contains(mousePoint) ||
                (_collapsePanel != CollapsePanel.None &&
                _spliterPanelState == SpliterPanelState.Collapsed))
            {
                _histTest = HistTest.Button;
                return;
            }

            if (base.SplitterRectangle.Contains(mousePoint))
            {
                _histTest = HistTest.Spliter;
            }

            base.OnMouseDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            base.Invalidate(base.SplitterRectangle);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            base.Invalidate(base.SplitterRectangle);

            Rectangle collapseRect = CollapseRect;
            Point mousePoint = e.Location;

            if (_histTest == HistTest.Button &&
                e.Button == MouseButtons.Left &&
                collapseRect.Contains(mousePoint))
            {
                OnCollapseClick(EventArgs.Empty);
            }
            _histTest = HistTest.None;
        }
        #endregion

        #region 私有方法

        private void CalculateRect(
            Rectangle collapseRect,
            out Rectangle arrowRect,
            out Rectangle topLeftRect,
            out Rectangle bottomRightRect)
        {
            int width;
            if (base.Orientation == Orientation.Horizontal)
            {
                width = (collapseRect.Width - DefaultArrowWidth) / 2;
                arrowRect = new Rectangle(
                    collapseRect.X + width,
                    collapseRect.Y,
                    DefaultArrowWidth,
                    collapseRect.Height);

                topLeftRect = new Rectangle(
                    collapseRect.X,
                    collapseRect.Y + 1,
                    width,
                    collapseRect.Height - 2);

                bottomRightRect = new Rectangle(
                    arrowRect.Right,
                    collapseRect.Y + 1,
                    width,
                    collapseRect.Height - 2);
            }
            else
            {
                width = (collapseRect.Height - DefaultArrowWidth) / 2;
                arrowRect = new Rectangle(
                    collapseRect.X,
                    collapseRect.Y + width,
                    collapseRect.Width,
                    DefaultArrowWidth);

                topLeftRect = new Rectangle(
                    collapseRect.X + 1,
                    collapseRect.Y,
                    collapseRect.Width - 2,
                    width);

                bottomRightRect = new Rectangle(
                    collapseRect.X + 1,
                    arrowRect.Bottom,
                    collapseRect.Width - 2,
                    width);
            }
        }
        private void SetCursor(Cursor cursor)
        {
            if (base.Cursor != cursor)
            {
                base.Cursor = cursor;
            }
        }
        #endregion

        protected virtual void OnCollapseClick(EventArgs e)
        {
            if (_spliterPanelState == SpliterPanelState.Collapsed)
            {
                SpliterPanelState = SpliterPanelState.Expanded;
            }
            else
            {
                SpliterPanelState = SpliterPanelState.Collapsed;
            }

            if (CollapseClick != null)
            {
                CollapseClick(this, e);
            }
        }
    }
}
