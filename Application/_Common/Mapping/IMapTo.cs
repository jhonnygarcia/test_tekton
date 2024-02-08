using AutoMapper;

namespace Application._Common.Mapping
{
    public interface IMapTo<T>
    {
        void Mapping(Profile profile) => profile.CreateMapping(GetType(), typeof(T));
    }
}
