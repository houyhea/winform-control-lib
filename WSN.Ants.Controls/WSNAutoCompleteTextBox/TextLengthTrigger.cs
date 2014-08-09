using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSN.Ants.Controls.WSNAutoCompleteTextBox
{
    /// <summary>
    ///  文本长度改变触发器
    ///     继承自<c>AutoCompleteTrigger</c>
    /// </summary>
    [Serializable]
    public class TextLengthTrigger : AutoCompleteTrigger
    {
        private int textLength = 2;
        /// <summary>
        /// 文本长度
        ///     初始默认为：2
        /// </summary>
        public int TextLength
        {
            get
            {
                return this.textLength;
            }
            set
            {
                this.textLength = value;
            }
        }
        /// <summary>
        /// 新建文本长度改变触发器
        /// </summary>
        public TextLengthTrigger()
        {
        }
        /// <summary>
        /// 新建文本长度改变触发器
        /// </summary>
        /// <param name="length">文本长度</param>
        public TextLengthTrigger(int length)
        {
            this.textLength = length;
        }

        public override TriggerState OnTextChanged(string text)
        {
            if (text.Length >= this.TextLength)
                return TriggerState.Show;
            else if (text.Length < this.TextLength)
                return TriggerState.Hide;

            return TriggerState.None;
        }


    }

}
