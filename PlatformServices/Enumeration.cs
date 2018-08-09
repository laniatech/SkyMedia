﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AzureSkyMedia.PlatformServices
{
    internal enum MediaEntity
    {
        Asset,
        Transform,
        TransformJob,
        ContentKeyPolicy,
        StreamingPolicy,
        StreamingEndpoint,
        StreamingLocator,
        LiveEvent,
        LiveEventOutput
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MediaTransformPreset
    {
        AdaptiveStreaming,
        VideoAnalyzer,
        AudioAnalyzer
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MediaProtection
    {
        AES,
        PlayReady,
        Widevine,
        FairPlay
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MediaImageFormat
    {
        JPG,
        PNG,
        BMP
    }
}