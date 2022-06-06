using System.Data;
using System.Resources.NetStandard;

namespace NothingToForgetBot.Core;

public static class ResXResourceReaderExtension
{
    public static string? GetString(this ResXResourceReader resourceReader, string key)
    {
        var dictionary = resourceReader.GetEnumerator();

        while (dictionary.MoveNext())
        {
            var currentKey = dictionary.Key as string ??
                             throw new DataException("Not correct data in resource keys");

            if (currentKey == key)
            {
                return dictionary.Value as string ?? dictionary.Value!.ToString();
            }
        }

        return null;
    }
}