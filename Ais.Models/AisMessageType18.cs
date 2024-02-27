// <copyright file="AisMessageType18.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Ais.Models.Abstractions;
using Ais.Net;

namespace Ais.Models
{
    public record AisMessageType18(
        bool CanAcceptMessage22ChannelAssignment,
        bool CanSwitchBands,
        float? CourseOverGroundDegrees,
        ClassBUnit CsUnit,
        bool HasDisplay,
        bool IsAssigned,
        bool IsDscAttached,
        uint Mmsi,
        Position? Position,
        bool PositionAccuracy,
        ClassBRadioStatusType RadioStatusType,
        bool RaimFlag,
        int RegionalReserved139,
        int RegionalReserved38,
        uint RepeatIndicator,
        float? SpeedOverGround,
        uint TimeStampSecond,
        uint TrueHeadingDegrees,
        string Channel,
        string OriginalMessage) :
            AisMessageBase(MessageType: 18, Mmsi, Channel, OriginalMessage),
            IAisMessageType18,
            IAisIsAssigned,
            IRaimFlag,
            IRepeatIndicator,
            IVesselNavigation;
}