using System;
using System.ComponentModel;

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
            var field = enum1.GetType().GetField(enum1.ToString());

            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute ? enum1.ToString() : attribute.Description;
        }
    }
}
