using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace FakeStockProxy.Application.Miscellaneous.Json.ContractResolvers;

public class CustomAttributeContractResolver<T> : DefaultContractResolver
                                    where T : Attribute
{
    Type _customAttribute;

    public CustomAttributeContractResolver()
    {
        _customAttribute = typeof(T);
    }

    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var list = type.GetProperties()
                    .Where(x => x.GetCustomAttributes().Any(a => a.GetType() == _customAttribute))
                    .Select(p => new JsonProperty()
                    {
                        PropertyName = p.Name,
                        PropertyType = p.PropertyType,
                        Readable = true,
                        Writable = true,
                        ValueProvider = base.CreateMemberValueProvider(p)
                    }).ToList();

        return list;
    }
}
