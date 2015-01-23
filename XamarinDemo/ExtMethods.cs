using System;

namespace XamarinDemo
{
    public static class ExtMethods
    {
        public static bool Contains(this string source, string toCheck, StringComparison comparisonType)
        {
            if (source == null)
            {
                return false;
            }
            return (source.IndexOf(toCheck, comparisonType) >= 0);
        }
    }
}