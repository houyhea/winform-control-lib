using System;
using System.Windows.Forms;

namespace WSN.Ants.Controls.WSNAutoCompleteTextBox
{
    /// <summary>
    /// 自动完成状态触发器
    /// </summary>
    [Serializable]
    public abstract class AutoCompleteTrigger
    {
        /// <summary>
        /// 获取文本改变后触发器状态
        /// </summary>
        /// <param name="text">改变后的文本</param>
        /// <returns>触发器状态</returns>
        public virtual TriggerState OnTextChanged(string text)
        {
            return TriggerState.None;
        }
        /// <summary>
        /// 获取按下按钮后触发器状态
        /// </summary>
        /// <param name="keyData">按钮</param>
        /// <returns>触发器状态</returns>
        public virtual TriggerState OnCommandKey(Keys keyData)
        {
            return TriggerState.None;
        }
    }
}
