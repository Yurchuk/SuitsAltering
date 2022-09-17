using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SuitsAltering.Infrastructure.Extensions;

public static class SerializationExtensions
{
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = (IContractResolver)new CamelCasePropertyNamesContractResolver()
    };

    public static string ToJson<T>(this T obj) => JsonConvert.SerializeObject((object)obj, Formatting.None, Settings);

    public static T Deserialize<T>(this string value) => string.IsNullOrEmpty(value) ? default(T) : (T)JsonConvert.DeserializeObject(value, typeof(T), Settings);
}