using Newtonsoft.Json.Linq;

namespace SuitsAltering.Infrastructure.Extensions;

public static class ObjectExtensions
{
    public static bool In<T>(this T value, params T[] items) => ((IEnumerable<T>)items).Contains<T>(value);

    public static bool NotIn<T>(this T value, params T[] items) => !((IEnumerable<T>)items).Contains<T>(value);

    public static Dictionary<string, object> ToDictionary<T>(this T value) => JObject.FromObject((object)value).ToObject<Dictionary<string, object>>();
}