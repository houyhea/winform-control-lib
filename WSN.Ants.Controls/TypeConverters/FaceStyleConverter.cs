using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;

namespace WSN.Ants.Controls.TypeConverters
{
    /// <summary>
    /// 外观样式转换器
    /// </summary>
    /// <remarks>提供一种将值的类型转换为其他外观样式以及访问标准值和子属性的统一方法。</remarks>
    /// <example>
    /// 以下示例展示如何使用FaceStyleConverter
    /// <code lang="C#" source="CommonExample\Controls\WSN.Ants.Controls\TypeConvertersExamples.cs" region="FaceStyleConverterExample"/>
    /// </example>
    public class FaceStyleConverter : TypeConverter
    {
        /// <summary>
        /// 返回该转换器是否可以使用指定的上下文将给定类型的对象转换为此转换器的类型。
        /// </summary>
        /// <param name="context">提供格式上下文。</param>
        /// <param name="sourceType">表示要转换的类型。</param>
        /// <returns>
        /// 返回结果:
        /// 如果该转换器能够执行转换，则为 true；否则为 false。
        ///</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return false;

            //if (sourceType == typeof(String)) return true;

            //return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// 返回此转换器是否可以使用指定的上下文将该对象转换为指定的外观样式类型。
        /// </summary>
        /// <param name="context">提供格式上下文。</param>
        /// <param name="destinationType">表示要转换到的外观样式类型。</param>
        /// <returns>
        /// 返回结果:
        /// 如果该转换器能够执行转换，则为 true；否则为 false。
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                    System.Type destinationType)
        {
            return false;

            //if (destinationType == typeof(String)) return true;

            //if (destinationType == typeof(InstanceDescriptor)) return true;

            //return base.CanConvertTo(context, destinationType);
        }
        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的值对象转换为指定的外观样式类型。
        /// </summary>
        /// <param name="context">提供格式上下文。</param>
        /// <param name="culture">
        /// 用作当前区域性的 System.Globalization.CultureInfo。
        /// 如果传递 null，则采用当前区域性。</param>
        /// <param name="value">要转换的 System.Object。</param>
        /// <param name="destinationType">value 参数要转换成的 System.Type。</param>
        /// <returns>
        /// 返回结果:
        /// 表示转换的 value 的 System.Object。
        /// </returns>
        /// <exception cref="ArgumentNullException">当destinationType参数为NULL时抛出</exception>
        /// <exception cref="NotSupportedException">当不能进行转换时抛出</exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
    object value, System.Type destinationType)
        {
            return "UI参数";

            //return base.ConvertTo(context, culture, value, destinationType);
        }
        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的对象转换为此转换器的外观样式类型。
        /// </summary>
        /// <param name="context">提供格式上下文。</param>
        /// <param name="culture">用作当前区域性的 System.Globalization.CultureInfo。</param>
        /// <param name="value">要转换的 System.Object。</param>
        /// <returns>返回结果:
        /// 表示转换的 value 的 System.Object。
        /// </returns>
        /// <exception cref="NotSupportedException">当不能进行转换时抛出</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }
        /// <summary>
        /// 使用指定的上下文返回该对象是否支持属性 (Property)。
        /// </summary>
        /// <param name="context">提供格式上下文。</param>
        /// <returns>
        /// 返回结果:
        /// 如果应调用 System.ComponentModel.TypeConverter.GetProperties(System.Object) 来查找此对象的属性，
        /// 则为true；否则为 false。
        /// </returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        ///  使用指定的上下文和属性 (Attribute) 返回由 value 参数指定的数组类型的属性 (Property) 的集合。
        /// </summary>
        /// <param name="context">提供格式上下文。</param>
        /// <param name="value"> 一个 System.Object，指定要为其获取属性的数组类型。</param>
        /// <param name="attributes">用作筛选器的 System.Attribute 类型数组。</param>
        /// <returns>
        /// 返回结果:
        /// 具有为此数据类型公开的属性的 System.ComponentModel.PropertyDescriptorCollection；
        /// 或者如果没有属性，则为null。
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(FaceStyle), attributes);
        }
    }
}
