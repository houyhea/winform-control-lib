using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSN.Ants.Controls.WSNContextMenuStrip
{
    /// <summary>
    ///  WSN控件：
    ///     WSNContextMenuStrip
    /// </summary>
    public class WSNContextMenuStrip:ContextMenuStrip
    {
        public WSNContextMenuStrip()
        {
            this.Renderer = new ContextMenuRender();
        }

    }
}
