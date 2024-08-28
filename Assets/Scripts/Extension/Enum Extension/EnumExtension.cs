using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Xi.Extension
{
    public static class EnumExtension
    {
        private static readonly ConcurrentDictionary<Type, int[]> _enumValuesCache = new();

        public static unsafe TEnum StepEnum<TEnum>(this TEnum value, int indexDelta) where TEnum : unmanaged, Enum
        {
            int intValue = *(int*)&value;
            int[] values = GetEnumValues<TEnum>();
            int index = Array.IndexOf(values, intValue);
            index = (index + indexDelta) % values.Length;
            if (index < 0)
            {
                index += values.Length;
            }

            int newValue = values[index];
            return *(TEnum*)&newValue;
        }

        private static int[] GetEnumValues<TEnum>() where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            if (!_enumValuesCache.TryGetValue(enumType, out int[] values))
            {
                values = Enum.GetValues(enumType).Cast<int>().ToArray();
                _enumValuesCache[enumType] = values;
            }

            return values;
        }
    }
}
