﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BeatmapParserRefactor.v3.beatmap;

public class V3BurstSliderData : V3BaseSliderData
{
    [JsonProperty("sc")]
    public int SliceCount { get; set; }

    [JsonProperty("s")]
    public float SquishAmount { get; set; }

    public V3BurstSliderData(IDictionary<string, JToken>? unserializedData, float time, V3NoteColorType colorType,
        int headLine, int headLayer, NoteCutDirection headCutDirection, float tailBeat, int tailLine, int tailLayer,
        int sliceCount, float squishAmount) : base(unserializedData, time, colorType, headLine, headLayer,
        headCutDirection, tailBeat, tailLine, tailLayer)
    {
        SliceCount = sliceCount;
        SquishAmount = squishAmount;
    }

    public override IBeatmapJSON Clone()
    {
        return new V3BurstSliderData(UnserializedData, Time, ColorType, HeadLine, HeadLayer, HeadCutDirection, TailBeat,
            TailLine, TailLayer, SliceCount, SquishAmount);
    }
}