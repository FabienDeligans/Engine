using System;
using System.ComponentModel;
using System.Reflection;

namespace Engine.Enum
{
    public static class EnumExt
    {
        /// <summary>
        /// Use Description Attribute and GetName of Enum to return the description parameter
        /// 
        /// public enum truc
        /// {
        ///    [Description("Truc 1")]
        ///    Truc1,
        ///
        ///    [Description("Truc 2")]
        ///    Truc2,
        /// }
        ///
        /// </summary>
        /// <param name="enum1"></param>
        /// <returns></returns>
        public static string GetName(this System.Enum enum1)
        {
            FieldInfo field = enum1.GetType().GetField(enum1.ToString());

            DescriptionAttribute attribute
                = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                    as DescriptionAttribute;

            return attribute == null ? enum1.ToString() : attribute.Description;
        }
    }
}
