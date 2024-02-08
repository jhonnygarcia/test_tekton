using System.ComponentModel;

namespace Application.Utility
{
    public class EnumService
    {
        public static List<KeyValuePair<T, string>> GetEnumItems<T>() where T : Enum
        {
            var enumItems = new List<KeyValuePair<T, string>>();

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                var descriptionAttribute = typeof(T).GetField(value.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    as DescriptionAttribute[];

                string description = descriptionAttribute != null && descriptionAttribute.Length > 0
                    ? descriptionAttribute[0].Description
                    : value.ToString();

                enumItems.Add(new KeyValuePair<T, string>(value, description));
            }

            return enumItems;
        }
        public static string GetDescriptionEnum<T>(T enumValue) where T : Enum
        {
            var descriptionAttribute = enumValue.GetType()
                    .GetField(enumValue.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    as DescriptionAttribute[];

            var description = descriptionAttribute != null && descriptionAttribute.Length > 0
                    ? descriptionAttribute[0].Description
                    : enumValue.ToString();

            return description;
        }
    }
}
