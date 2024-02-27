// <copyright file="AisMessageType1Through3.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Ais.Models.Abstractions;
using Ais.Net;

namespace Ais.Models
{
    public record AisMessageType1Through3(
        float? CourseOverGroundDegrees,
        ManoeuvreIndicator ManoeuvreIndicator,
        int MessageType,
        uint Mmsi,
        NavigationStatus NavigationStatus,
        Position? Position,
        bool PositionAccuracy,
        uint RadioSlotTimeout,
        uint RadioSubMessage,
        RadioSyncState RadioSyncState,
        int RateOfTurn,
        bool RaimFlag,
        uint RepeatIndicator,
        uint SpareBits145,
        float? SpeedOverGround,
        uint TimeStampSecond,
        uint TrueHeadingDegrees,
        string Channel,
        string OriginalMessage) :
            AisMessageBase(MessageType, Mmsi, Channel, OriginalMessage),
            IAisMessageType1to3,
            IRaimFlag,
            IRepeatIndicator,
            IVesselNavigation;
}