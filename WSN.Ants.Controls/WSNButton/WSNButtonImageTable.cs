using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WSN.Ants.Controls.WSNButton
{
    /// <summary>
    /// 但按钮是图片模式时，该类保存各状态下的图片
    /// </summary>
    /// <example>
    /// 以下示例展示如何使用WSNButtonImageTable
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\WSNButtonExamples.cs" region="WSNButtonImageTableExample"/>
    /// </example>
    public class WSNButtonImageTable
    {
        private Image _imageNormal;
        private Image _imageHover;
        private Image _imageChecked;
        private Image _imageDisalbed;
        /// <summary>
        /// 禁用状态的图片
        /// </summary>
        public Image ImageDisalbed
        {
            get { return _imageDisalbed; }
            set { _imageDisalbed = value; }
        }

        /// <summary>
        /// 选择状态的图片
        /// </summary>
        public Image ImageChecked
        {
            get { return _imageChecked; }
            set { _imageChecked = value; }
        }

        /// <summary>
        /// 鼠标停留状态的图片
        /// </summary>
        public Image ImageHover
        {
            get { return _imageHover; }
            set { _imageHover = value; }
        }

        /// <summary>
        /// 普通状态的图片
        /// </summary>
        public Image ImageNormal
        {
            get { return _imageNormal; }
            set { _imageNormal = value; }
        }
        
    }
}
