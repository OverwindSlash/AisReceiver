// <copyright file="AisMessageType19.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Ais.Models.Abstractions;
using Ais.Net;

namespace Ais.Models
{
    public record AisMessageType19(
        float? CourseOverGroundDegrees,
        uint DimensionToBow,
        uint DimensionToPort,
        uint DimensionToStarboard,
        uint DimensionToStern,
        bool IsAssigned,
        bool IsDteNotReady,
        uint Mmsi,
        Position? Position,
        bool PositionAccuracy,
        EpfdFixType PositionFixType,
        bool RaimFlag,
        int RegionalReserved139,
        int RegionalReserved38,
        uint RepeatIndicator,
        string ShipName,
        ShipType ShipType,
        uint Spare308,
        float? SpeedOverGround,
        uint TimeStampSecond,
        uint TrueHeadingDegrees,
        string Channel,
        string OriginalMessage) :
            AisMessageBase(MessageType: 19, Mmsi, Channel, OriginalMessage),
            IAisMessageType19,
            IAisIsAssigned,
            IAisIsDteNotReady,
            IAisPositionFixType,
            IRaimFlag,
            IRepeatIndicator,
            IShipType,
            IVesselDimensions,
            IVesselNavigation;
}