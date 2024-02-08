using System.Reflection;
using AutoMapper;

namespace Application._Common.Mapping
{
    public class MappingProfile : Profile
    {
        private const string METHOD_MAPPING = "Mapping";
        public MappingProfile(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(IMapFrom<>) || i.GetGenericTypeDefinition() == typeof(IMapTo<>))))
                .ToList();

            var typeIMapFrom = typeof(IMapFrom<>).GetGenericTypeDefinition();
            var typeIMapTo = typeof(IMapTo<>).GetGenericTypeDefinition();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod(METHOD_MAPPING);

                if (methodInfo == null)
                {
                    var interfaces = type.GetInterfaces();

                    var interfaceMapFromType = interfaces.FirstOrDefault(
                        t => t.IsGenericType && t.GetGenericTypeDefinition() == typeIMapFrom);
                    var interfaceMapToType = interfaces.FirstOrDefault(
                        t => t.IsGenericType && t.GetGenericTypeDefinition() == typeIMapTo);

                    if (interfaceMapFromType != null)
                    {
                        methodInfo = interfaceMapFromType.GetMethod(METHOD_MAPPING);
                        methodInfo?.Invoke(instance, new object[] { this });
                    }
                    if (interfaceMapToType != null)
                    {
                        methodInfo = interfaceMapToType.GetMethod(METHOD_MAPPING);
                        methodInfo?.Invoke(instance, new object[] { this });
                    }
                }
                else
                {
                    methodInfo.Invoke(instance, new object[] { this });
                }
            }
        }
    }
}