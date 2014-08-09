using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace WSN.Ants.Controls.WSNAutoCompleteTextBox
{
    /// <summary>
    /// 自动完成状态触发器的集合
    /// </summary>
    [Serializable]
    public class AutoCompleteTriggerCollection : CollectionBase
    {
        /// <summary>
        /// 自动完成状态触发器集合的编辑器
        /// </summary>
        public class AutoCompleteTriggerCollectionEditor : CollectionEditor
        {
            /// <summary>
            /// 新建编辑器
            /// </summary>
            /// <param name="type">触发器对应的类型</param>
            public AutoCompleteTriggerCollectionEditor(Type type)
                : base(type)
            {
            }

            protected override bool CanSelectMultipleInstances()
            {
                return false;
            }

            protected override Type[] CreateNewItemTypes()
            {
                return new Type[] { typeof(ShortCutTrigger) };
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(ShortCutTrigger);
            }
        }
        /// <summary>
        /// 根据索引查找对应触发器
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>返回的触发器</returns>
        public AutoCompleteTrigger this[int index]
        {
            get
            {
                return this.InnerList[index] as AutoCompleteTrigger;
            }
        }
        /// <summary>
        /// 将添加的单个触发器添加到集合中
        /// </summary>
        /// <param name="item">单个触发器</param>
        public void Add(AutoCompleteTrigger item)
        {
            this.InnerList.Add(item);
        }

        /// <summary>
        /// 从集合中移除特定触发器的第一个匹配项
        /// </summary>
        /// <param name="item">要移除的触发器</param>
        public void Remove(AutoCompleteTrigger item)
        {
            this.InnerList.Remove(item);
        }

        /// <summary>
        /// 获取文本改变后触发器状态
        /// </summary>
        /// <param name="text">改变后的文本</param>
        /// <returns>触发器状态</returns>
        public virtual TriggerState OnTextChanged(string text)
        {
            foreach (AutoCompleteTrigger trigger in this.InnerList)
            {
                TriggerState state = trigger.OnTextChanged(text);
                if (state != TriggerState.None)
                {
                    return state;
                }
            }
            return TriggerState.None;
        }
        /// <summary>
        /// 获取按下按钮后触发器状态
        /// </summary>
        /// <param name="keyData">按钮</param>
        /// <returns>触发器状态</returns>
        public virtual TriggerState OnCommandKey(Keys keyData)
        {
            foreach (AutoCompleteTrigger trigger in this.InnerList)
            {
                TriggerState state = trigger.OnCommandKey(keyData);
                if (state != TriggerState.None)
                {
                    return state;
                }
            }
            return TriggerState.None;
        }



    }

}
