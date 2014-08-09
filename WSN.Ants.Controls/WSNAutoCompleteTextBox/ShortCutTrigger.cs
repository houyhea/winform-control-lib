using System;
using System.Windows.Forms;


namespace WSN.Ants.Controls.WSNAutoCompleteTextBox
{
    /// <summary>
    /// 快捷键触发器
    ///     继承自<c>AutoCompleteTrigger</c>
    /// </summary>
    [Serializable]
    public class ShortCutTrigger : AutoCompleteTrigger
    {
        private Keys shortCut = Keys.None;
        /// <summary>
        /// 快捷键
        ///     默认为：Keys.None
        /// </summary>
        public Keys ShortCut
        {
            get
            {
                return this.shortCut;
            }
            set
            {
                this.shortCut = value;
            }
        }

        private TriggerState result = TriggerState.None;
        /// <summary>
        /// 触发器状态
        /// </summary>
        public TriggerState ResultState
        {
            get
            {
                return this.result;
            }
            set
            {
                this.result = value;
            }
        }
        /// <summary>
        /// 新建快捷键触发器
        /// </summary>
        public ShortCutTrigger()
        {
        }
        /// <summary>
        /// 新建快捷键触发器
        /// </summary>
        /// <param name="shortCutKeys">触发该触发器的快捷键</param>
        /// <param name="resultState">触发该触发器后触发器的状态</param>
        public ShortCutTrigger(Keys shortCutKeys, TriggerState resultState)
        {
            this.shortCut = shortCutKeys;
            this.result = resultState;
        }
        /// <summary>
        /// 获取按下按钮后触发器状态
        /// </summary>
        /// <param name="keyData">按钮</param>
        /// <returns>触发器状态</returns>
        public override TriggerState OnCommandKey(Keys keyData)
        {
            if (keyData == this.ShortCut)
                return this.ResultState;

            return TriggerState.None;
        }


    }

}
