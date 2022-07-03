﻿// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using BeatmapParserRefactor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using var stream = File.OpenRead("test_maps/FoolishOfMeEPlusLawless.dat");
using var streamReader = new StreamReader(stream, new UTF8Encoding());
using var jsonReader = new JsonTextReader(streamReader);

var options = new JsonSerializerSettings
{
    NullValueHandling = NullValueHandling.Ignore,
    MaxDepth = null,
    Culture = CultureInfo.InvariantCulture
};

var serializer = JsonSerializer.CreateDefault(options);

var stopwatch = Stopwatch.StartNew();
IBeatmap? beatmap = serializer.Deserialize<V2Beatmap>(jsonReader);
Debug.Assert(beatmap != null, nameof(beatmap) + " != null");

Console.WriteLine($"Parsed beatmap in {stopwatch.ElapsedMilliseconds}ms");

Console.WriteLine("Repeated runs for JIT warmup:");
stream.Seek(0, SeekOrigin.Begin);
var json = streamReader.ReadToEnd();

for (var i = 0; i < 10; i++)
{
    using var jsonReader2 = new JsonTextReader(new StringReader(json));
    
    stopwatch.Restart();
    serializer.Deserialize<V2Beatmap>(jsonReader2);
    stopwatch.Stop();

    Console.WriteLine($"Run {i} took: {stopwatch.ElapsedMilliseconds}ms");
}


Tests.CheckMutability(beatmap, streamReader, serializer);

beatmap.BasicEvents = beatmap.BasicEvents.OrderBy(e => e).ToList();
beatmap.Notes = beatmap.Notes.OrderBy(e => e).ToList();
beatmap.Obstacles = beatmap.Obstacles.OrderBy(e => e).ToList();
beatmap.Waypoints = beatmap.Waypoints.OrderBy(e => e).ToList();

if (beatmap.BeatmapCustomData != null)
{
    beatmap.BeatmapCustomData.CustomEvents = beatmap.BeatmapCustomData.CustomEvents?.OrderBy(e => e).ToList();
}

Debug.Assert(beatmap.BasicEvents.Any(e => e.CustomData?.Color != null));

Console.WriteLine("Note clone");
Tests.CheckClone(beatmap.Notes.First());
Console.WriteLine("Obstacle clone");
Tests.CheckClone(beatmap.Obstacles.First());

Console.WriteLine("Event clone");
Tests.CheckClone(beatmap.BasicEvents.First());