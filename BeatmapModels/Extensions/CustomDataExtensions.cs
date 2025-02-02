﻿using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

[PublicAPI]
public static class CustomDataExtensions
{
    public static JToken? GetJToken(this ICustomData customData, [NotNull] string key, [NotNull] string v2Key)
    {
        if (customData.isV3)
            return customData.UnserializedData.TryGetValue(key, out var value) ? value : null;
        else
            return customData.UnserializedData.TryGetValue(v2Key, out var value) ? value : null;
    }

    public static T? Get<T>(this ICustomData customData, [NotNull] string key, [NotNull] string v2Key) where T : class
    {
        if (customData.isV3)
            return customData.UnserializedData.TryGetValue(key, out var value) ? value.ToObject<T>() : null;
        else
            return customData.UnserializedData.TryGetValue(v2Key, out var value) ? value.ToObject<T>() : null;
    }

    public static T? GetStruct<T>(this ICustomData customData, [NotNull] string key, [NotNull] string v2Key)
        where T : struct
    {
        if (customData.isV3)
            return customData.UnserializedData.TryGetValue(key, out var value) ? value.ToObject<T?>() : null;
        else
            return customData.UnserializedData.TryGetValue(v2Key, out var value) ? value.ToObject<T?>() : null;
    }
}