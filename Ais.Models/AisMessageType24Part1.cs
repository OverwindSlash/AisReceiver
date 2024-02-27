// <copyright file="AisMessageType24Part1.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Ais.Models.Abstractions;
using Ais.Net;

namespace Ais.Models
{
    public record AisMessageType24Part1(
        string CallSign,
        uint DimensionToBow,
        uint DimensionToPort,
        uint DimensionToStarboard,
        uint DimensionToStern,
        uint Mmsi,
        uint MothershipMmsi,
        uint PartNumber,
        uint RepeatIndicator,
        uint SerialNumber,
        uint Spare162,
        ShipType ShipType,
        uint UnitModelCode,
        string VendorIdRev3,
        string VendorIdRev4,
        string Channel,
        string OriginalMessage) :
            AisMessageBase(MessageType: 24, Mmsi, Channel, OriginalMessage),
            IAisMessageType24Part1,
            IAisMultipartMessage,
            ICallSign,
            IRepeatIndicator,
            IShipType,
            IVesselDimensions;
}