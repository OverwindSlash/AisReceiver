// <copyright file="AisMessageBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Ais.Models.Abstractions;

namespace Ais.Models
{
    public record AisMessageBase(int MessageType, uint Mmsi, string Channel, string OriginalMessage) : IAisMessage;
}