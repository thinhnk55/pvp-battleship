using System;

namespace Framework
{
    public static class EnumExtension
    {
        public static T FromString<T>(this Type @enum, string s) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), s);
        }
    }
}
