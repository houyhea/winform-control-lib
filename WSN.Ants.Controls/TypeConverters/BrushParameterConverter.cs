using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;

namespace WSN.Ants.Controls.TypeConverters
{
    class BrushParameterConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return false;
            //if (sourceType == typeof(String)) return true;

            //return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                    System.Type destinationType)
        {
            return false;

            //if (destinationType == typeof(String)) return true;

            //if (destinationType == typeof(InstanceDescriptor)) return true;

            //return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
    object value, System.Type destinationType)
        {
            return "画刷参数";

           // return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(BrushParameter), attributes);
        }
    }

}
