using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSN.Ants.Controls.WSNToolStrip
{
    /// <summary>
    /// WSN控件：
    ///     WSNToolStrip
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNToolStrip
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNToolStripExamples.cs" region="WSNToolStripExample"/>
    /// </example>
    public class WSNToolStrip:ToolStrip
    {
        private ToolStripRenderStyle _renderStyle = ToolStripRenderStyle.NormalRender;

        #region 构造函数
        /// <summary>
        /// 构造函数：
        ///     创建<c>WSNToolStrip</c>
        /// </summary>
        public WSNToolStrip()
        {
            SetRender();
        }
        #endregion

        /// <summary>
        /// 控件属性：
        ///     获取或设置工具栏外观样式
        /// </summary>
        public ToolStripRenderStyle RenderStyle
        {
            get { return _renderStyle; }
            set { 
                _renderStyle = value;
                SetRender();
            }
        }


        #region 私有方法
        private void SetRender()
        {
            ToolStripRenderer render = null;
            switch (RenderStyle)
            { 
                case ToolStripRenderStyle.NormalRender:
                    render=new WSNNormToolStripRender();
                    break;
                case ToolStripRenderStyle.StatusRender:
                    render = new WSNStatusToolStipRender();
                    break;
                case ToolStripRenderStyle.VideoTitleBar:
                    render = new WSNVideoTitleBarToolStripRender();
                    break;
                case ToolStripRenderStyle.VideoToolBar:
                    render = new WSNVideoToolBarToolStripRender();
                    break;
                default:
                    render = new WSNNormToolStripRender();
                    break;

            }
            this.Renderer = render;
            this.Invalidate();
        }

        #endregion

    }
}
