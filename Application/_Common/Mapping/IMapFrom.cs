using AutoMapper;

namespace Application._Common.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMapping(typeof(T), GetType());
    }
}

