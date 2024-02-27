// <copyright file="AisMessageType5.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Ais.Models.Abstractions;
using Ais.Net;

namespace Ais.Models
{
    public record AisMessageType5(
        uint AisVersion,
        string CallSign,
        string Destination,
        uint DimensionToBow,
        uint DimensionToPort,
        uint DimensionToStarboard,
        uint DimensionToStern,
        uint Draught10thMetres,
        uint EtaDay,
        uint EtaHour,
        uint EtaMinute,
        uint EtaMonth,
        bool IsDteNotReady,
        uint ImoNumber,
        uint Mmsi,
        EpfdFixType PositionFixType,
        uint RepeatIndicator,
        ShipType ShipType,
        uint Spare423,
        string VesselName,
        string Channel,
        string OriginalMessage) :
            AisMessageBase(MessageType: 5, Mmsi, Channel, OriginalMessage),
            IAisMessageType5,
            IAisIsDteNotReady,
            IAisPositionFixType,
            ICallSign,
            IRepeatIndicator,
            IShipType,
            IVesselDimensions,
            IVesselName;
}
