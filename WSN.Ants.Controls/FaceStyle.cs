using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSN.Ants.Controls.TypeConverters;
using System.ComponentModel;

namespace WSN.Ants.Controls
{
    /// <summary>
    /// 控件的外观相关样式，包括背景不同状态的样式，边框的样式，text的样式
    /// </summary>
    [TypeConverter(typeof(FaceStyleConverter))]
    public class FaceStyle
    {
        BrushParameter _backCheckedStyle=new BrushParameter();
        BrushParameter _backDisabledStyle = new BrushParameter();
        BrushParameter _backHoverStyle = new BrushParameter();
        BrushParameter _backNormalStyle = new BrushParameter();

        BrushParameter _borderCheckedStyle = new BrushParameter();
        BrushParameter _borderDisabledStyle = new BrushParameter();
        BrushParameter _borderHoverStyle = new BrushParameter();
        BrushParameter _borderNormalStyle = new BrushParameter();

        BrushParameter _textCheckedStyle = new BrushParameter();
        BrushParameter _textDisabledStyle = new BrushParameter();
        BrushParameter _textHoverStyle = new BrushParameter();
        BrushParameter _textNormalStyle = new BrushParameter();

        /// <summary>
        /// 文本正常状态样式
        /// </summary>
        [Description("文本正常状态样式")]
        public BrushParameter TextNormalStyle
        {
            get { return _textNormalStyle; }
            set { _textNormalStyle = value; }
        }
        /// <summary>
        /// 文本Hover状态样式
        /// </summary>
        [Description("文本Hover状态样式")]
        public BrushParameter TextHoverStyle
        {
            get { return _textHoverStyle; }
            set { _textHoverStyle = value; }
        }
        /// <summary>
        /// 文本禁用状态样式
        /// </summary>
        [Description("文本禁用状态样式")]
        public BrushParameter TextDisabledStyle
        {
            get { return _textDisabledStyle; }
            set { _textDisabledStyle = value; }
        }
        /// <summary>
        /// 文本选中状态样式
        /// </summary>
        [Description("文本选中状态样式")]
        public BrushParameter TextCheckedStyle
        {
            get { return _textCheckedStyle; }
            set { _textCheckedStyle = value; }
        }

        /// <summary>
        /// 边框正常状态样式
        /// </summary>
        [Description("边框正常状态样式")]
        public BrushParameter BorderNormalStyle
        {
            get { return _borderNormalStyle; }
            set { _borderNormalStyle = value; }
        }
        /// <summary>
        /// 边框Hover状态样式
        /// </summary>
        [Description("边框Hover状态样式")]
        public BrushParameter BorderHoverStyle
        {
            get { return _borderHoverStyle; }
            set { _borderHoverStyle = value; }
        }
        /// <summary>
        /// 边框禁用状态样式
        /// </summary>
        [Description("边框禁用状态样式")]
        public BrushParameter BorderDisabledStyle
        {
            get { return _borderDisabledStyle; }
            set { _borderDisabledStyle = value; }
        }
        /// <summary>
        /// 边框选中状态样式
        /// </summary>
        [Description("边框选中状态样式")]
        public BrushParameter BorderCheckedStyle
        {
            get { return _borderCheckedStyle; }
            set { _borderCheckedStyle = value; }
        }
        /// <summary>
        /// 背景正常状态样式
        /// </summary>
        [Description("背景正常状态样式")]
        public BrushParameter BackNormalStyle
        {
            get { return _backNormalStyle; }
            set { _backNormalStyle = value; }
        }
        /// <summary>
        /// 背景Hover状态样式
        /// </summary>
        [Description("背景Hover状态样式")]
        public BrushParameter BackHoverStyle
        {
            get { return _backHoverStyle; }
            set { _backHoverStyle = value; }
        }
        /// <summary>
        /// 背景禁用状态样式
        /// </summary>
        [Description("背景禁用状态样式")]
        public BrushParameter BackDisabledStyle
        {
            get { return _backDisabledStyle; }
            set { _backDisabledStyle = value; }
        }
        /// <summary>
        /// 背景选中状态样式
        /// </summary>
        [Description("背景选中状态样式")]
        public BrushParameter BackCheckedStyle
        {
            get { return _backCheckedStyle; }
            set { _backCheckedStyle = value; }
        }
    }
}
