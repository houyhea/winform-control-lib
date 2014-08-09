using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WSN.Ants.Controls
{
    /// <summary>
    /// 单选框各状态图片列表
    /// </summary>
    public class CheckImageTable
    {
        private Image _checkedImage;
        private Image _unCheckedImage;
        private Image _checkedImageDisabled;
        private Image _unCheckedImageDisabled;
        private Image _IndeterminateImage;
        private Image _IndeterminateImageDisabled;

        /// <summary>
        /// 选中状态图片（可用状态）
        /// </summary>
        public Image CheckedImage
        {
            get { return _checkedImage; }
            set { _checkedImage = value; }
        }
        /// <summary>
        /// 未选中状态图片（可用状态）
        /// </summary>
        public Image UnCheckedImage
        {
            get { return _unCheckedImage; }
            set { _unCheckedImage = value; }
        }
        /// <summary>
        /// 选中状态图片（禁用状态）
        /// </summary>
        public Image CheckedImageDisabled
        {
            get { return _checkedImageDisabled; }
            set { _checkedImageDisabled = value; }
        }
        /// <summary>
        /// 未选中状态图片（禁用状态）
        /// </summary>
        public Image UnCheckedImageDisabled
        {
            get { return _unCheckedImageDisabled; }
            set { _unCheckedImageDisabled = value; }
        }
        /// <summary>
        /// 不确定状态图片（可用状态）
        /// </summary>
        public Image IndeterminateImage
        {
            get { return _IndeterminateImage; }
            set { _IndeterminateImage = value; }
        }
        /// <summary>
        /// 不确定状态图片（禁用状态）
        /// </summary>
        public Image IndeterminateImageDisabled
        {
            get { return _IndeterminateImageDisabled; }
            set { _IndeterminateImageDisabled = value; }
        }

    }
}
