namespace SuitsAltering.Infrastructure.Extensions;

public static class EnumerableExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any<T>();

    public static bool IsEmpty<T>(this ICollection<T> source) => source.Count == 0;

    public static bool HasValue<T>(this IEnumerable<T> source) => source.Any<T>();

    public static bool HasValue<T>(this ICollection<T> source) => source.Count > 0;

    public static bool HasValue<T>(this IEnumerable<T> source, Func<T, bool> predicate) => source.Any<T>(predicate);

    public static bool IsSingle<T>(this ICollection<T> source) => source.Count == 1;

    public static bool IsSingle<T>(this IEnumerable<T> source) => source.Count<T>() == 1;

    public static IEnumerable<T> ToEnumerable<T>(this T item)
    {
        yield return item;
    }

    public static IReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> source) => (IReadOnlyCollection<T>)source.EnsureList<T>().AsReadOnly();

    public static List<T> AsList<T>(this IEnumerable<T> source) => source is List<T> objList ? objList : source.ToList<T>();

    public static T[] AsArray<T>(this IEnumerable<T> source) => source is T[] objArray ? objArray : source.ToArray<T>();

    public static List<T> EnsureList<T>(this IEnumerable<T> source) => source != null ? source.AsList<T>() : new List<T>();

    public static T[] EnsureArray<T>(this IEnumerable<T> source) => source != null ? source.AsArray<T>() : new T[0];

    public static bool ContainsOnly<T>(this IEnumerable<T> listOne, IEnumerable<T> listTwo) => !listOne.ToList<T>().Except<T>((IEnumerable<T>)listTwo.ToList<T>()).Any<T>();

    public static bool IsSequenceEqual<T>(this IEnumerable<T> source, IEnumerable<T> dest) => source.OrderBy<T, T>((Func<T, T>)(x => x)).SequenceEqual<T>((IEnumerable<T>)dest.OrderBy<T, T>((Func<T, T>)(x => x)));

    public static string JoinWith<T>(this IEnumerable<T> source, string separator = ", ") => string.Join<T>(separator, source);

    public static IEnumerable<string> GetDuplicates(this IEnumerable<string> source) => source.GroupBy<string, string>((Func<string, string>)(x => x)).Where<IGrouping<string, string>>((Func<IGrouping<string, string>, bool>)(g => g.Count<string>() > 1)).Select<IGrouping<string, string>, string>((Func<IGrouping<string, string>, string>)(y => y.Key));
}