using System.Collections.Concurrent;
using System.Reflection;

namespace Outbox.Internal;

internal static class SqlQueriesReader
{
    private static readonly ConcurrentDictionary<string, string> Queries = new();

    public static string ReadWithCache(string fileName)
    {
        return Queries.GetOrAdd(fileName, Read);
    }

    private static string Read(string fileName)
    {
        var assembly = typeof(SqlQueriesReader).GetTypeInfo().Assembly;
        var resourceName = assembly.GetManifestResourceNames().Single(p => p.EndsWith(fileName));
        using var resource = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(resource);
        return reader.ReadToEnd();
    }
}