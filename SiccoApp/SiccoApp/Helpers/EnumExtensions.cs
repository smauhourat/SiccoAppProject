using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SiccoApp.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum GenericEnum) //Hint: Change the method signature and input paramter to use the type parameter T
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }



        ///// <summary>
        ///// Gets an attribute on an enum field value
        ///// </summary>
        ///// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        ///// <param name="enumVal">The enum value</param>
        ///// <returns>The attribute of type T that exists on the enum value</returns>
        ///// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        //public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        //{
        //    var type = enumVal.GetType();
        //    var memInfo = type.GetMember(enumVal.ToString());
        //    var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        //    return (attributes.Length > 0) ? (T)attributes[0] : null;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="TEnum"></typeparam>
        ///// <returns></returns>
        ///// <example></example>
        //public static List<KeyValuePair<TEnum, string>> GetList<TEnum>()
        //    where TEnum : struct
        //{
        //    if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();

        //    return ((TEnum[])Enum.GetValues(typeof(TEnum)))
        //        .ToDictionary(k => k, v => ((Enum)(object)v).GetAttributeOfType<DescriptionAttribute>().Description)
        //        .ToList();
        //}

    }
}