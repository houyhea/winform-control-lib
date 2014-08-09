using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSN.Ants.Controls.WSNAutoCompleteTextBox
{
    /// <summary>
    /// 触发器状态
    /// </summary>
    [Serializable]
    public enum TriggerState : int
    {
        /// <summary>
        /// 无状态
        /// </summary>
        None = 0,
        /// <summary>
        /// 显示
        /// </summary>
        Show = 1,
        /// <summary>
        /// 显示并使用
        /// </summary>
        ShowAndConsume = 2,
        /// <summary>
        /// 隐藏
        /// </summary>
        Hide = 3,
        /// <summary>
        /// 隐藏并使用
        /// </summary>
        HideAndConsume = 4,
        /// <summary>
        /// 选择
        /// </summary>
        Select = 5,
        /// <summary>
        /// 选择并使用
        /// </summary>
        SelectAndConsume = 6
    }
}
