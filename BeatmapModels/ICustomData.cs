﻿/// <summary>
///     Properties could be parsed ahead of time or
///     dynamically read
///     Proceed with caution
///     TODO: Define behaviour definitively
///     Newtonsoft does NOT parse JSON properties of dictionaries
/// </summary>
public interface ICustomData : IBeatmapJSON
{
    ICustomData ShallowClone();
    ICustomData DeepCopy();
}