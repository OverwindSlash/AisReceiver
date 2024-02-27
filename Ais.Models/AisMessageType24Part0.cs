// <copyright file="AisMessageType24Part0.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Ais.Models.Abstractions;

namespace Ais.Models
{
    public record AisMessageType24Part0(
        uint Mmsi,
        uint PartNumber,
        uint RepeatIndicator,
        uint Spare160,
        string Channel,
        string OriginalMessage) :
            AisMessageBase(MessageType: 24, Mmsi, Channel, OriginalMessage),
            IAisMultipartMessage,
            IRepeatIndicator,
            IAisMessageType24Part0;
}