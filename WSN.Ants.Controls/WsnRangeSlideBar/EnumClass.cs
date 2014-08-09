using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSN.Ants.Controls.WsnRangeSlideBar
{
    /// <summary>
    /// 激活类型
    /// </summary>
    public enum ActiveMarkType
    {
        /// <summary>
        ///  没有激活控制条。
        /// </summary>
        none,
        /// <summary>
        ///  激活左边控制条。
        /// </summary>
        left,
        /// <summary>
        ///  激活右边控制条。
        /// </summary>
        right,
        /// <summary>
        ///  激活中间控制条。
        /// </summary>
        middle
    };

    /// <summary>
    /// 选择显示模式
    /// </summary>
    public enum SelectShowMode
    {
        /// <summary>
        ///  整型选择模式。
        /// </summary>
        TInteger,
        /// <summary>
        ///  时间选择模式。
        /// </summary>
        TDateTime
    };
}
