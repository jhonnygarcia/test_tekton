using AutoMapper;

namespace Application._Common.Mapping
{
    public static class MappingExtensions
    {
        public static IMappingExpression<TSource, TDestination> CreateMapping<TSource, TDestination>(this Profile profile)
        {
            var mapExpresion = profile.CreateMap<TSource, TDestination>();

            typeof(TDestination).GetProperties()
                .Where(p => !p.PropertyType.IsPrimitive()).ToList()
                .ForEach(prop => mapExpresion.ForMember(prop.Name, opc => opc.Ignore()));

            return mapExpresion;
        }

        public static IMappingExpression CreateMapping(this Profile profile, Type typeSource, Type typeDestination)
        {
            var mapExpresion = profile.CreateMap(typeSource, typeDestination);

            typeDestination.GetProperties()
                .Where(p => !p.PropertyType.IsPrimitive()).ToList()
                .ForEach(prop => mapExpresion.ForMember(prop.Name, opc => opc.Ignore()));

            return mapExpresion;
        }

        public static bool IsPrimitive(this Type type)
        {
            if (type.IsArray) type = type.GetElementType();

            if (type.IsPrimitive || type == typeof(string)
                || type == typeof(decimal) || type == typeof(DateTime) || type == typeof(Guid))
            {
                return true;
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return underlyingType.IsPrimitive || underlyingType == typeof(string)
                    || underlyingType == typeof(decimal) ||
                    underlyingType == typeof(DateTime) || underlyingType.IsEnum || underlyingType == typeof(Guid);
            }

            return type.IsEnum;
        }

        public static void Assign(this IMapper _, object currentObject, object source)
        {
            SetExtraProperties(currentObject, source, false);
        }
        public static T MapTo<T>(this IMapper mapper, object source, Action<T> action)
        {
            var destination = mapper.Map<T>(source);
            action(destination);
            return destination;
        }
        public static T MapTo<T>(this IMapper mapper, object source, object @override, bool skipNull = false)
        {
            var destination = mapper.Map<T>(source);
            SetExtraProperties(destination, @override, skipNull);
            return destination;
        }
        private static void SetExtraProperties(object current, object extraInfo, bool skipNull)
        {
            if (extraInfo == null || current == null) return;

            var currentProperties = current.GetType().GetProperties()
                .Where(p => p.PropertyType.IsPrimitive()).ToArray();
            var extraInfoProperties = extraInfo.GetType().GetProperties()
                .Where(p => p.PropertyType.IsPrimitive()).ToArray();

            foreach (var prop in currentProperties)
            {
                var propertyExtra = extraInfoProperties.FirstOrDefault(p => p.Name == prop.Name);
                if (propertyExtra == null) continue;

                if (prop.PropertyType == propertyExtra.PropertyType)
                {
                    var value = propertyExtra.GetValue(extraInfo);
                    if (value == null && skipNull) continue;
                    prop.SetValue(current, value);
                }

                var type1 = prop.PropertyType;
                var type1IsNullable = false;
                if (Nullable.GetUnderlyingType(type1) != null)
                {
                    type1 = Nullable.GetUnderlyingType(type1);
                    type1IsNullable = true;
                }

                var type2 = propertyExtra.PropertyType;
                if (Nullable.GetUnderlyingType(type2) != null)
                {
                    type2 = Nullable.GetUnderlyingType(type2);
                }

                if (type1 != type2) continue;
                var propValueExtraInfo = propertyExtra.GetValue(extraInfo);
                if (!type1IsNullable && propValueExtraInfo == null) continue;

                if (propValueExtraInfo == null && skipNull) continue;
                prop.SetValue(current, propValueExtraInfo);
            }
        }
    }
}
